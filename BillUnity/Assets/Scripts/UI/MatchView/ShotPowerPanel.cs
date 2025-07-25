using Kborod.MatchManagement;
using Kborod.UI.Screens.Table;
using Kborod.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens
{
    public class ShotPowerPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform handleArea;
        [SerializeField] private PointerEventsWrapper handleAreaEvents;
        [SerializeField] private Transform handle;
        [SerializeField] private Image scale;

        private const float MIN_POWER = 0.02f;

        [Inject] private MatchBase _match;
        [Inject] private MyShotInput _myShotInput;

        private float _areaHeight;
        private float _handleY;

        private float _lastY;

        private float _currPower => -_handleY / _areaHeight;

        private float _resolutionKoeff => Screen.height >= 1080f ? 1 : 1080f / Screen.height;
        private float _moveKoeff => 2f;

        private void Start()
        {
            ResetPanel();
            _areaHeight = handleArea.rect.height;

            _match.StateChanged += MatchStateChangedHandler;
            handleAreaEvents.BeginDrag += PointerDownHandler;
            handleAreaEvents.Drag += PointerMoveHandler;
            handleAreaEvents.EndDrag += PointerUpHandler;
        }

        private void OnDestroy()
        {
            _match.StateChanged -= MatchStateChangedHandler;
            handleAreaEvents.BeginDrag -= PointerDownHandler;
            handleAreaEvents.Drag -= PointerMoveHandler;
            handleAreaEvents.EndDrag -= PointerUpHandler;
        }

        private void MatchStateChangedHandler(MatchState state)
        {
            gameObject.SetActive(state == MatchState.PrepeareTurn && _match.CanIManageTurningPlayer);
        }

        private void PointerUpHandler(PointerEventData data)
        {
            if (_currPower >  MIN_POWER) 
            {
                _myShotInput.SelectPower(_currPower);
            }

            ResetPanel();
        }

        private void PointerMoveHandler(PointerEventData data)
        {
            var delta = (data.position.y - _lastY) * _resolutionKoeff * _moveKoeff;
            _lastY = data.position.y;
            _handleY = Mathf.Clamp(_handleY + delta, -_areaHeight, 0);
            //Debug.Log($"Move: {data.position.y} -- {_handleY} -- {_lastY} -- {delta} -- {_resolutionKoeff}");
            RefreshByHandleY();
            _myShotInput.ChangePower(_currPower);
        }

        private void PointerDownHandler(PointerEventData data)
        {
            _lastY = data.position.y;
            //Debug.Log($"Down: {data.position.y} {_handleY} {_lastY}");
        }

        private void ResetPanel()
        {
            _handleY = 0;
            _lastY = 0;
            RefreshByHandleY();
        }

        private void RefreshByHandleY()
        {
            handle.localPosition = new Vector2(handle.localPosition.x, _handleY);
            scale.fillAmount = _currPower;
        }
    }
}
