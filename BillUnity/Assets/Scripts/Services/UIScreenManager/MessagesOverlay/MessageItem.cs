using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kborod.Services.UIScreenManager
{

    public class MessageItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<MessageItem> OnDestroy;

        private const float _timeLife = 4f;
        private const float _appearTime = 0.2f;
        private const float _destroyTime = 0.2f;
        private const float _destroyDist = 1000f;
        private const float _returnTime = 0.2f;
        private const float _moveTime = 0.2f;

        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _background;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Color _errorBgColor;
        [SerializeField] private ModalIconsSO _iconsSettings;

        public RectTransform RT { get; private set; }

        private Color _defaultBgColor;

        private Tweener _appearTweener;
        private Tweener _alphaDeathTweener;
        private Tweener _positionTweener;
        private Sequence _timerSequence;


        private void Awake()
        {
            RT = GetComponent<RectTransform>();
            _canvasGroup.alpha = 0;
            _defaultBgColor = _background.color;

            StartTween_Appear();
            StartTween_AutoDeathTimer();
        }

        public void Init(string text, bool isError, IconType iconType)
        {
            _text.text = text;
            _background.color = isError ? _errorBgColor : _defaultBgColor;
            _icon.gameObject.SetActive(iconType != IconType.None);
            if (iconType != IconType.None)
            {
                var iconSettings = _iconsSettings.GetIcon(iconType);
                _icon.sprite = iconSettings.Icon;
                _icon.color = iconSettings.Color;
            }
        }
        
        public void ChangePosition(Vector2 pos)
        {
            _basePosition = pos;

            if (_alphaDeathTweener.IsActive())
                return;

            StartTween_Move(pos, _moveTime);
        }

        #region Swipe        
        private Vector2 _basePosition;
        private Vector2 _tempPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _basePosition = RT.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _tempPosition = RT.anchoredPosition;
            _tempPosition.x += eventData.delta.x;
            RT.anchoredPosition = _tempPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CheckSwipeDistance(eventData, out var isRemoved);
            if (!isRemoved)
                Item_RestorePosition();
        }

        private void CheckSwipeDistance(PointerEventData eventData, out bool isRemoved)
        {
            const float swipeDist = 110.0f;

            var dist = RT.anchoredPosition.x - _basePosition.x;

            if (Mathf.Abs(dist) > swipeDist)
            {
                var sign = Mathf.Sign(dist);
                var finalPos = RT.anchoredPosition + new Vector2(_destroyDist * sign, 0);
                Item_Destroy(finalPos);

                _background.raycastTarget = false;

                isRemoved = true;
            }
            else
            {
                isRemoved = false;
            }
        }

        private void Item_Destroy(Vector2 finalPos)
        {
            if (_alphaDeathTweener.IsActive())
                return;

            StartTween_Move(finalPos, _destroyTime);
            StartTween_FadeAndDestroy(_destroyTime);
        }

        private void Item_RestorePosition()
        {
            StartTween_Move(_basePosition, _returnTime);
        }
        #endregion

        #region TweenAnimations
        private void StartTween_Appear()
        {
            _appearTweener.Kill();
            //_appearTweener = _canvasGroup.DOFade(1, _appearTime);
        }

        private void StartTween_AutoDeathTimer()
        {
            _timerSequence.Kill();
            _timerSequence = DOTween.Sequence();

            _timerSequence.AppendInterval(_timeLife);
            _timerSequence.onComplete = () => StartTween_FadeAndDestroy(_destroyTime);
        }

        private void StartTween_Move(Vector2 finalPos, float time)
        {
            _positionTweener.Kill();

            //_positionTweener = RT.DOAnchorPos(finalPos, time);
            _positionTweener.OnStart(() =>
            {
                _background.raycastTarget = false;
            });
            _positionTweener.OnComplete(() =>
            {
                _background.raycastTarget = true;
            });
        }

        private void StartTween_FadeAndDestroy(float time)
        {
            OnDestroy?.Invoke(this);

            if (_timerSequence.IsActive())
                _timerSequence.Kill();

            if (_appearTweener.IsActive())
                _appearTweener.Kill();

            _alphaDeathTweener.Kill();

            //_alphaDeathTweener = _canvasGroup.DOFade(0, time);
            _alphaDeathTweener.OnStart(() =>
            {
                _background.raycastTarget = false;
            });
            _alphaDeathTweener.OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }
        #endregion
    }
}

/* Copyright: Made by Appfox */