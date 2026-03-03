using Kborod.BilliardCore;
using Kborod.Extensions;
using Kborod.MatchManagement;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens
{
    public class CueOverlay : MonoBehaviour
    {
        [SerializeField] private Transform cueHolder;
        [SerializeField] private Transform cue;
        [SerializeField] private Camera tableCamera;
        [SerializeField] private Transform ballsRoot;

        private const float CUE_MIN_X = -0.35f;
        private const float CUE_MAX_X = -1.2f;

        [Inject] private MatchServices _matchServices;
        private IEngineForUI _engine => _matchServices.EngineForUI;
        private MatchBase _match => _matchServices.Match;

        private void Start()
        {
            _match.StateChanged += MatchStateChangedHandler;
            _match.AimInfoReceived += AimInfoChangedHandler;

            Refresh();
        }

        private void OnDestroy()
        {
            _match.StateChanged -= MatchStateChangedHandler;
            _match.AimInfoReceived -= AimInfoChangedHandler;
        }

        private void MatchStateChangedHandler(MatchState state)
        {
            Refresh();
        }

        private void AimInfoChangedHandler(AimInfo info)
        {
            Refresh();
        }

        private void Refresh()
        {
            if (_match.State != MatchState.PrepeareTurn)
            {
                gameObject.SetActive(false);
                return;
            }

            var info = _match.AimInfo;

            if (info.CueBall.HasValue == false || info.IsBallMovingNow)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            var cueBall = _match.AimInfo.CueBall.Value;

            var ballPosition = ballsRoot.TransformPoint(_engine.Balls[cueBall].v.p0.ToVector2() * Config.MODEL_COORD_TO_WORLD_KOEF);

            cueHolder.transform.localPosition = ballPosition;

            float angle = Mathf.Atan2(info.DirectionY, info.DirectionX) * Mathf.Rad2Deg;
            cueHolder.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            cue.transform.localPosition = new Vector3(CUE_MIN_X, 0, -1.5f);
        }
    }
}
