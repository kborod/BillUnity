using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Kborod.Services.UIScreenManager
{

    public class UIModalsManager : MonoBehaviour
    {
        private DiContainer _diContainer;
        private UIScreensLoader _screensLoader;
        private TransitionManager _transitionManager;

        private Dictionary<Type, List<UIScreenBase>> _cache = new Dictionary<Type, List<UIScreenBase>>();

        [Inject]
        public void Construct(DiContainer container, UIScreensLoader screensLoader, TransitionManager transitionManager)
        {
            _diContainer = container;
            _screensLoader = screensLoader;
            _transitionManager = transitionManager;
        }

        public async UniTask<TScreen> Open<TScreen>() where TScreen : UIScreenBase
        {
            await WaitCurrentWork();

            var screen = await GetScreenInstance<TScreen>();
            _transitionManager.SwitchScreen(null, screen).Forget();
            return screen;
        }

        public async UniTask<UIScreenBase> Open(Type screenType)
        {
            await WaitCurrentWork();

            var screen = await GetScreenInstance(screenType);
            _transitionManager.SwitchScreen(null, screen).Forget();
            return screen;
        }

        public async UniTask Clear()
        {
            await WaitCurrentWork();

            foreach (var pair in _cache)
            {
                var modals = pair.Value;
                foreach (var modal in modals)
                {
                    UnsubscribeFromScreen(modal);
                    Destroy(modal.gameObject);
                }
            }

            _cache.Clear();
        }

        public async UniTask WaitCurrentWork()
        {
            var transitionTask = UniTask.WaitUntil(() => !_transitionManager.TransitionNow);
            var screenLoadTask = _screensLoader.WaitCurrentWork();
            await UniTask.WhenAll(transitionTask, screenLoadTask);
        }

        private async UniTask<TScreen> GetScreenInstance<TScreen>() where TScreen : UIScreenBase
        {
            var instanceScreenBase = await GetScreenInstance(typeof(TScreen));
            return (TScreen)instanceScreenBase;
        }

        private async UniTask<UIScreenBase> GetScreenInstance(Type type)
        {
            var instance = GetScreenFromCacheOrNull(type);
            instance ??= await CreateScreen(type);
            instance.transform.SetAsLastSibling();
            return instance;
        }

        private async void CloseCalledHandler(UIScreenBase screen)
        {
            await WaitCurrentWork();
            await _transitionManager.SwitchScreen(screen, null);
            AddScreenToCache(screen);
        }

        private async void ReleaseCalledHandler(UIScreenBase screen)
        {
            await WaitCurrentWork();
            await _transitionManager.SwitchScreen(screen, null);
            UnsubscribeFromScreen(screen);
            Destroy(screen.gameObject);
        }

        private void SubscribeOnScreen(UIScreenBase screen)
        {
            screen.OnCloseCalled += CloseCalledHandler;
            screen.OnReleaseCalled += ReleaseCalledHandler;
        }

        private void UnsubscribeFromScreen(UIScreenBase screen)
        {
            screen.OnCloseCalled -= CloseCalledHandler;
            screen.OnReleaseCalled -= ReleaseCalledHandler;
        }

        private void AddScreenToCache(UIScreenBase screen)
        {
            var screenType = screen.GetType();

            if (_cache.TryGetValue(screenType, out var result) == false)
                _cache.Add(screenType, result = new List<UIScreenBase>());

            result.Add(screen);
        }

        private UIScreenBase GetScreenFromCacheOrNull(Type type)
        {
            if (_cache.TryGetValue(type, out var list) && list.Count > 0)
            {
                var result = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return result;
            }
            return null;
        }

        private async UniTask<UIScreenBase> CreateScreen(Type type)
        {
            var prefab = await _screensLoader.Load(type);
            var go = _diContainer.InstantiatePrefab(prefab, transform);
            var instance = go.GetComponent<UIScreenBase>();
            SubscribeOnScreen(instance);
            return instance;
        }
    }
}