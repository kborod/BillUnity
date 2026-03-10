using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kborod.UI.Screens.Table.BallsMove
{
    public class MoveIcon : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public event Action<Vector2> PointerDownEvent;
        public event Action PointerUpEvent;
        public event Action<Vector2> DragEvent;


        [SerializeField] private Image icon;

        public void Show()
        {
            icon.color = new Color(1, 1, 1, 1);
        }

        public void Hide()
        {
            icon.color = new Color(1, 1, 1, 0);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PointerDownEvent?.Invoke(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragEvent?.Invoke(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            icon.gameObject.SetActive(true);
            PointerUpEvent?.Invoke();
        }
    }
}
