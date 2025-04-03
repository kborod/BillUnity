using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kborod.UI.Screens
{
    public class MouseInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public event Action<Vector2> MouseDown;
        public event Action<Vector2> MouseUp;
        public event Action<Vector2> MouseMove;

        public Vector2 CurrPosition => Input.mousePosition;

        private bool isMouseDown;
        private Vector3 lastPosition;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isMouseDown)
                return;

            isMouseDown = true;
            lastPosition = Input.mousePosition;
            MouseDown?.Invoke(lastPosition);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!isMouseDown)
                return;

            MouseMove?.Invoke(Input.mousePosition - lastPosition);
            lastPosition = Input.mousePosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isMouseDown)
                return;

            isMouseDown = false;
            MouseUp?.Invoke(lastPosition);
        }
    }
}
