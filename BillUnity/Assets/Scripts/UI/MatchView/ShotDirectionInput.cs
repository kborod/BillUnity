using Kborod.BilliardCore;
using Kborod.Extensions;
using Kborod.MatchManagement;
using Kborod.UI.Screens.Table;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens
{
    public class ShotDirectionInput : MonoBehaviour
    {
        [SerializeField] private MouseInput mouseInput;
        [SerializeField] private Camera tableCamera;
        [SerializeField] private Transform ballsRoot;

        [Inject] private MatchServices _matchServices;
        private IEngineForUI _engine => _matchServices.EngineForUI;
        private MatchBase _match => _matchServices.Match;
        private MyInput _myShotInput => _matchServices.MyInput;

        private Vector2 ballPosition => ballsRoot.TransformPoint(_engine.Balls[_myShotInput.CurrentCueBallOrNull.Value].v.p0.ToVector2() * Config.MODEL_COORD_TO_WORLD_KOEF);
        private Vector2 lastCursorScreenPosition;

        private void Start()
        {
            mouseInput.MouseDown += MouseDownHandler;
            mouseInput.MouseMove += MouseMoveHandler;
            _match.StateChanged += RefreshByMatchState;

            RefreshByMatchState(_match.State);
        }

        private void OnDestroy()
        {
            mouseInput.MouseDown -= MouseDownHandler;
            mouseInput.MouseMove -= MouseMoveHandler;
            _match.StateChanged -= RefreshByMatchState;
        }

        private void RefreshByMatchState(MatchState state)
        {
            if (state == MatchState.PrepeareTurn && _myShotInput.CanIManageTurningPlayer)
            {
                mouseInput.enabled = true;
                gameObject.SetActive(true);
            }
            else
            {
                mouseInput.enabled = false;
                gameObject.SetActive(false);
            }
        }

        private void MouseDownHandler(Vector2 position)
        {
            lastCursorScreenPosition = position;
        }

        private void MouseMoveHandler(Vector2 delta)
        {
            Vector2 newScreenPosition = lastCursorScreenPosition + delta;
            Vector2 newWorldPosition = tableCamera.ScreenToWorldPoint(newScreenPosition);

            Vector2 direction = newWorldPosition - ballPosition;
            Vector2 normalizedDirection = direction.normalized;

            lastCursorScreenPosition = newScreenPosition;

            _myShotInput.ChangeDirection(normalizedDirection.x, normalizedDirection.y);
        }
    }
}
