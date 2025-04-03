using System.Collections;
using TMPro;
using UnityEngine;

namespace Kborod.Services.UIScreenManager.LoadOverlay
{
    public class LoadingOverlay : MonoBehaviour
    {
        private const float FADE_DURATION = 0.1f;
        private const float MAX_ALPHA = 1f;
        private const float MIN_ALPHA = 0f;
        private const string DEFAULT_MESSAGE_TEXT = "Loading";

        [SerializeField] private TMP_Text messageText = default;
        [SerializeField] private GameObject bg = default;
        
        private CanvasGroup _canvasGroup = default;
        private IEnumerator _currFadeCoroutine = null;

        private void Awake()
        {
            gameObject.SetActive(false);
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
            bg.SetActive(false);
        }

        public void Show(string message = "", bool withFade = true, bool withBG = false)
        {
            if (string.IsNullOrEmpty(message))
                message = DEFAULT_MESSAGE_TEXT;

            SetMessage(message);

            gameObject.SetActive(true);

            bg.SetActive(withBG);

            StopCurrentFade();
            if (withFade)
                StartCoroutine(_currFadeCoroutine = FadeUp());
            else
                _canvasGroup.alpha = MAX_ALPHA;
        }

        public void Hide()
        {
            StopCurrentFade();
            if(gameObject.activeSelf)
                StartCoroutine(_currFadeCoroutine = FadeDown());
        }

        private IEnumerator FadeUp()
        {
            while (_canvasGroup.alpha < MAX_ALPHA)
            {
                yield return null;
                _canvasGroup.alpha += Time.deltaTime / FADE_DURATION;
            }
            _currFadeCoroutine = null;
        }

        private IEnumerator FadeDown()
        {
            if (gameObject.activeSelf == false)
                yield break;

            while (_canvasGroup.alpha > MIN_ALPHA)
            {
                yield return null;
                _canvasGroup.alpha -= Time.deltaTime / FADE_DURATION;
            }
            gameObject.SetActive(false);
            _currFadeCoroutine = null;
        }

        private void StopCurrentFade()
        {
            if (_currFadeCoroutine != null)
            {
                StopCoroutine(_currFadeCoroutine);
                _currFadeCoroutine = null;
            }
        }

        private void SetMessage(string msg)
        {
            messageText.text = msg;
        }
    }
}

/* Copyright: Made by Appfox */