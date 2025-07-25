using Kborod.BilliardCore;
using Kborod.MatchManagement;
using System;
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

        [Inject] private IEngineForUI _engine;
        [Inject] private MatchBase _match;

        

        private Vector2 currDirection => cueHolder.right.normalized;

        private void Start()
        {
            _match.StateChanged += MatchStateChangedHandler;
            _match.AimInfoReceived += Refresh;

            Refresh(_match.AimInfo);
        }

        private void OnDestroy()
        {
            _match.StateChanged -= MatchStateChangedHandler;
            _match.AimInfoReceived -= Refresh;
        }

        private void MatchStateChangedHandler(MatchState state)
        {
            gameObject.SetActive(state == MatchState.PrepeareTurn);
        }

        private void Refresh(AimInfo info)
        {
            if (_match.AimInfo.CueBall.HasValue == false || _match.AimInfo.IsBallMovingNow)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            var cueBall = _match.AimInfo.CueBall.Value;

            var ballPosition = ballsRoot.TransformPoint(_engine.Balls[cueBall].v.p0 * Config.MODEL_COORD_TO_WORLD_KOEF);

            cueHolder.transform.localPosition = ballPosition;

            float angle = Mathf.Atan2(info.DirectionY, info.DirectionX) * Mathf.Rad2Deg;
            cueHolder.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            cue.transform.localPosition = new Vector3(CUE_MIN_X, 0, -1.5f);
        }
    }
}
