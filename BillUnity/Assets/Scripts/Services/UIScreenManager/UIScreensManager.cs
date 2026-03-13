using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Kborod.Services.UIScreenManager
{

    public class UIScreensManager : MonoBehaviour
    {
        private DiContainer _diContainer;
        private UIScreensLoader _screensLoader;
        private TransitionManager _transitionManager;

        private Dictionary<Type, UIScreenBase> _instances = new Dictionary<Type, UIScreenBase>();
        private Stack<UIScreenBase> _stack = new Stack<UIScreenBase>();

        [Inject]
        public void Construct(DiContainer container, UIScreensLoader screensLoader, TransitionManager transitionManager)
        {
            _diContainer = container;
            _screensLoader = screensLoader;
            _transitionManager = transitionManager;
        }

        public async UniTask<TScreen> Open<TScreen>(bool leaveCurrentInStack = false) where TScreen : UIScreenBase
        {
            await WaitCurrentWork();

            var prevScreen = GetCurrent(leaveCurrentInStack);
            var screen = await GetScreenInstance<TScreen>();
            _stack.Push(screen);
            _transitionManager.SwitchScreen(prevScreen, screen).Forget();
            return screen;
        }

        public async UniTask<UIScreenBase> Open(Type screenType, bool leaveCurrentInStack = false)
        {
            await WaitCurrentWork();

            var prevScreen = GetCurrent(leaveCurrentInStack);
            var screen = await GetScreenInstance(screenType);
            _stack.Push(screen);
            _transitionManager.SwitchScreen(prevScreen, screen).Forget();
            return screen;
        }

        public async UniTask Clear()
        {
            await WaitCurrentWork();

            foreach (var pair in _instances)
            {
                var screen = pair.Value;
                UnsubscribeFromScreen(screen);
                Destroy(screen.gameObject);
            }

            _instances.Clear();
            _stack.Clear();
        }

        public async UniTask WaitCurrentWork()
        {
            await UniTask.WaitUntil(() => !_transitionManager.IsWorkingNow && !_screensLoader.IsWorkingNow);
        }

        public bool IsActiveScreensExist()
        {
            return _stack.Count > 0;
        }

        private async UniTask CloseCurrentAndShowPrev()
        {
            var prevScreen = GetCurrent(false);
            var screen = GetLastFromStackOrNull();
            await _transitionManager.SwitchScreen(prevScreen, screen);
        }

        private UIScreenBase GetCurrent(bool leaveInStack)
        {
            if (_stack.IsEmpty)
                return null;

            var screen = leaveInStack ? _stack.Peek() : _stack.Pop();
            return screen;
        }

        private UIScreenBase GetLastFromStackOrNull()
        {
            if (_stack.IsEmpty)
                return null;
            var screen = _stack.Peek();
            return screen;
        }

        private async UniTask<TScreen> GetScreenInstance<TScreen>() where TScreen : UIScreenBase
        {
            var instanceScreenBase = await GetScreenInstance(typeof(TScreen));
            return (TScreen)instanceScreenBase;
        }

        private async UniTask<UIScreenBase> GetScreenInstance(Type type)
        {
            if (_instances.TryGetValue(type, out var instance) == false)
            {
                var prefab = await _screensLoader.Load(type);
                var go = _diContainer.InstantiatePrefab(prefab, transform);
                instance = go.GetComponent<UIScreenBase>();
                _instances.Add(type, instance);
                SubscribeOnScreen(instance);
            }
            return instance;
        }

        private async void CloseCalledHandler(UIScreenBase screen)
        {
            await WaitCurrentWork();

            if (_stack.Peek() != screen)
                throw new Exception("Visible screen is not first element in stack()");
            await CloseCurrentAndShowPrev();
        }

        private async void ReleaseCalledHandler(UIScreenBase screen)
        {
            await WaitCurrentWork();

            if (_stack.Contains(screen))
            {
                if (_stack.Peek() == screen)
                    await CloseCurrentAndShowPrev();
                else
                    _stack.Remove(screen);
            }
            var pair = _instances.Where(pair => pair.Value == screen).First();
            _instances.Remove(pair.Key);
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
    }
}