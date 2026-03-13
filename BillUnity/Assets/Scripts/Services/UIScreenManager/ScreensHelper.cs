using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace Kborod.Services.UIScreenManager
{

    public class ScreensHelper
    {
        [Inject] public UIScreensManager ScreensManager { get; private set; }
        [Inject] public UIModalsManager ModalsManager { get; private set; }
        [Inject] public MessagesOverlay MessagesOverlay { get; private set; }

        public async UniTask<TScreen> OpenScreen<TScreen>() where TScreen : UIScreenBase
        {
            return await ScreensManager.Open<TScreen>();
        }

        public async UniTask<TScreen> OpenModal<TScreen>() where TScreen : UIScreenBase
        {
            return await ModalsManager.Open<TScreen>();
        }

        public async UniTask ClearAll()
        {
            await ScreensManager.Clear();
            await ModalsManager.Clear();
        }

        public async UniTask WaitAllCurrentWork()
        {
            await UniTask.WhenAll(ScreensManager.WaitCurrentWork(), ModalsManager.WaitCurrentWork());
        }

    }
}