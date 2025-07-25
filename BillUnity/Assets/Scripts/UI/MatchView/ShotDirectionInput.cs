using Kborod.BilliardCore;
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

        [Inject] private IEngineForUI _engine;
        [Inject] private MatchBase _match;
        [Inject] private MyShotInput _myShotInput;

        private Vector2 ballPosition => ballsRoot.TransformPoint(_engine.Balls[_myShotInput.CurrentCueBallOrNull.Value].v.p0 * Config.MODEL_COORD_TO_WORLD_KOEF);
        private Vector2 lastCursorScreenPosition;

        private void Start()
        {
            mouseInput.MouseDown += MouseDownHandler;
            mouseInput.MouseMove += MouseMoveHandler;
            _match.StateChanged -= MatchStateChangedHandler;
        }

        private void OnDestroy()
        {
            mouseInput.MouseDown -= MouseDownHandler;
            mouseInput.MouseMove -= MouseMoveHandler;
            _match.StateChanged -= MatchStateChangedHandler;
        }

        private void MatchStateChangedHandler(MatchState state)
        {
            if (state == MatchState.PrepeareTurn && _match.CanIManageTurningPlayer)
            {
                gameObject.SetActive(true);
            }
            else
            {
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
