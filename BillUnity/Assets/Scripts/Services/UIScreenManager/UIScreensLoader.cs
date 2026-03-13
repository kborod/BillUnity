using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Kborod.Services.UIScreenManager
{
    public class UIScreensLoader : MonoBehaviour
    {
        public event Action LoadingStarted;
        public event Action LoadingCompleted;

        //After rename fields you need to edit UIScreensLoaderEditor
        [Header("Initial search for screens in all assemblies, or selectively")]
        [SerializeField] private bool _searchInAllAssemblies;
        [Tooltip("It is better to select assemblies manually to speed up the search of screens classes")]
        [SerializeField] private string[] _assemblyNames;

        public bool IsWorkingNow { get; private set; }

        private readonly Dictionary<Type, UIScreenAttribute> _declarations = new Dictionary<Type, UIScreenAttribute>();
        private readonly Dictionary<Type, UIScreenBase> _prefabs = new Dictionary<Type, UIScreenBase>();

        public async UniTask Initialize()
        {
            PreloadDeclarations();
            await PreloadPrefabs();
        }

        public async UniTask<TScreen> Load<TScreen>() where TScreen : UIScreenBase
        {
            await WaitCurrentLoading();
            var type = typeof(TScreen);
            var resultScreenBase = await Load(type);
            return (TScreen)resultScreenBase;
        }

        public async UniTask<UIScreenBase> Load(Type type)
        {
            if (_declarations.Count == 0)
                await Initialize();

            if (_prefabs.TryGetValue(type, out var result) == false)
            {
                LoadingStarted?.Invoke();
                IsWorkingNow = true;
                result = await LoadPrefab(type, GetDeclaration(type).AddressableKey);
                IsWorkingNow = false;
                LoadingCompleted?.Invoke();
            }
            return result;
        }

        public async UniTask WaitCurrentWork()
        {
            if (IsWorkingNow)
                await UniTask.WaitUntil(() => !IsWorkingNow);
        }

        private async UniTask<UIScreenBase> LoadPrefab(Type type, string key)
        {
            if (!type.IsSubclassOf(typeof(UIScreenBase)))
                throw new ArgumentException($"{type} must be subclass of {typeof(UIScreenBase)}");

            var prefabGO = await Addressables.LoadAssetAsync<GameObject>(key);
            var prefab = prefabGO.GetComponent(type);
            if (prefab == null)
                throw new NullReferenceException($"Screen type ({type}) not found in loaded prefab");
            var castedToBase = (UIScreenBase)prefab;
            _prefabs.Add(type, castedToBase);
            return castedToBase;
        }

        private UIScreenAttribute GetDeclaration(Type screenType)
        {
            if (!_declarations.TryGetValue(screenType, out var declaration))
                throw new Exception($"Unknown screen type: {screenType}.\nMaybe there is no {typeof(UIScreenAttribute)} attribute on the screen class");
            return declaration;
        }

        private void PreloadDeclarations()
        {
            var assemblies = GetAssembliesForSearchIn();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    var attr = type.GetCustomAttributes(typeof(UIScreenAttribute), false);
                    if (attr.Length == 0) continue;
                    _declarations.Add(type, (UIScreenAttribute)attr[0]);
                }
            }
        }

        private Assembly[] GetAssembliesForSearchIn()
        {
            var allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (_searchInAllAssemblies)
                return allAssemblies;

            var names = _assemblyNames
                .Where(a => !string.IsNullOrEmpty(a))
                .Distinct();

            return allAssemblies
                .Where(a => names.Contains(a.GetName().Name))
                .ToArray();
        }

        private async UniTask PreloadPrefabs()
        {
            var needPreload = _declarations.Where(pair => pair.Value.LazyLoad == false).ToList();

            var tasks = new List<UniTask>();
            foreach (var pair in needPreload)
                tasks.Add(LoadPrefab(pair.Key, pair.Value.AddressableKey));
            IsWorkingNow = true;
            await UniTask.WhenAll(tasks);
            IsWorkingNow = false;
        }

        private async UniTask WaitCurrentLoading()
        {
            await UniTask.WaitUntil(() => !IsWorkingNow);
        }
    }
}