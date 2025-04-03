using Kborod.Services.ServerHTTPCommunication;
using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Kborod.Services.UIScreenManager.LoadOverlay
{
    public class LoadingOverlayHelper : MonoBehaviour
    {
        [Inject] private LoadingOverlay _loadingOverlay;
        [Inject] private UIScreensLoader _screensLoader;
        //[Inject] private HttpService _httpService;

        private void Awake()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _screensLoader.LoadingStarted += ScreensManagerWorkStartedHandler;
            _screensLoader.LoadingCompleted += ScreensManagerWorkFinishedHandler;

            //_httpService.OnCurrentMessageChanged += HttpMessageChanged;
        }

        private void RemoveListeners()
        {
            _screensLoader.LoadingStarted -= ScreensManagerWorkStartedHandler;
            _screensLoader.LoadingCompleted -= ScreensManagerWorkFinishedHandler;

            //_httpService.OnCurrentMessageChanged -= HttpMessageChanged;
        }

        //Отображение загрузки для каких-то функций, которые долго отрабатывают
        public void ExecuteWithLoading(Action action)
        {
            _loadingOverlay.Show(withFade: false);
            StartCoroutine(ActionInNextFrameCoroutine(action));
        }

        private IEnumerator ActionInNextFrameCoroutine(Action action)
        {
            yield return null;
            action?.Invoke();
            _loadingOverlay.Hide();
        }

        private void ScreensManagerWorkStartedHandler()
        {
            _loadingOverlay.Show();
        }

        private void ScreensManagerWorkFinishedHandler()
        {
            _loadingOverlay.Hide();
        }

        private void HttpMessageChanged(HttpMessageBase msg)
        {
            if (msg != null)
                _loadingOverlay.Show(msg.TextWhileSending);
            else
                _loadingOverlay.Hide();
        }
    }
}

/* Copyright: Made by Appfox */