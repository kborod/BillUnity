using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Kborod.Utils
{
    public class PointerEventsWrapper : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> BeginDrag;
        public event Action<PointerEventData> Drag;
        public event Action<PointerEventData> EndDrag;
        public event Action<PointerEventData> PointerDown;
        public event Action<PointerEventData> PointerMove;
        public event Action<PointerEventData> PointerUp;

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag?.Invoke(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDown?.Invoke(eventData);
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            PointerMove?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(eventData);
        }
    }
}
