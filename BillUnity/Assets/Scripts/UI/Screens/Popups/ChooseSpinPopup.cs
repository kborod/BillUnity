using Kborod.Utils;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kborod.UI.Screens
{
    public class ChooseSpinPopup : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private PointerEventsWrapper ball;
        [SerializeField] private RectTransform point;

        private Action<float, float> callback;

        private float ballRadius; 
        private float maxRadius;

        private void Awake()
        {
            closeButton.onClick.AddListener(Close);
            ball.PointerDown += PointerEventHandler;
            ball.Drag += PointerEventHandler;
            ball.PointerUp += PointerUpHandler;

            ballRadius = ball.GetComponent<RectTransform>().sizeDelta.x * 0.33f;
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
            ball.PointerDown -= PointerEventHandler;
            ball.Drag -= PointerEventHandler;
            ball.PointerUp -= PointerUpHandler;
        }

        private void PointerEventHandler(PointerEventData data)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ball.GetComponent<RectTransform>(), data.position, null, out var localPoint);
            Vector2 clampedPosition = localPoint.magnitude > maxRadius ? localPoint.normalized * maxRadius : localPoint;
            point.localPosition = clampedPosition;
        }

        private void PointerUpHandler(PointerEventData data)
        {
            Close();
        }

        public void Show(float currSpinX, float currSpinY, float maxSpin, Action<float, float> callback)
        {
            gameObject.SetActive(true);

            this.callback = callback;

            maxRadius = ballRadius * maxSpin;
            point.localPosition = new Vector2(currSpinX * ballRadius, currSpinY * ballRadius);
        }

        private void Close()
        {
            callback?.Invoke(point.localPosition.x / ballRadius, point.localPosition.y / ballRadius);
            gameObject.SetActive(false);
        }

    }
}
