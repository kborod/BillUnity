using Kborod.BilliardCore;
using Kborod.MatchManagement;
using System;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens.Table
{
    public class MyShotInput : MonoBehaviour
    {
        public int? CurrentCueBallOrNull => _aimInfo.CueBall;

        [Inject] private MatchBase _match;

        private AimInfo _aimInfo;

        private void Awake()
        {
            _match.StateChanged += MatchStateChangedHandler;
        }

        private void OnDestroy()
        {
            _match.StateChanged -= MatchStateChangedHandler;
        }

        private void MatchStateChangedHandler(MatchState state)
        {
            if (state == MatchState.PrepeareTurn && _match.CanIManageTurningPlayer)
                ResetAimInfo();
        }

        public void ChangePower(float power)
        {
            _aimInfo.Power = power;
            _match.ChangeAimInfo(_aimInfo);

            //cue.localPosition = new Vector3(Mathf.Lerp(CUE_MIN_X, CUE_MAX_X, power), 0, -0.2f);
        }

        public void SelectPower(float power)
        {
            _aimInfo.Power = power;
            
            //HitReady?.Invoke(currDirection * 300, power);

            _match.MakeShot(_aimInfo);
        }

        public void ChangeDirection(float directionX, float directionY)
        {
            _aimInfo.DirectionX = directionX;
            _aimInfo.DirectionY = directionY;
            _match.ChangeAimInfo(_aimInfo);
        }

        public void MoveCueBall(float? x, float? y)
        {
            _aimInfo.CueBallX = x;
            _aimInfo.CueBallY = y;
            _aimInfo.IsBallMovingNow = true;
            _match.ChangeAimInfo(_aimInfo);
        }

        public void MoveCueBallCompleted()
        {
            _aimInfo.IsBallMovingNow = false;
            _match.ChangeAimInfo(_aimInfo);
        }

        public void ChangeSpin(float spinX, float spinY)
        {
            _aimInfo.SpinX = spinX;
            _aimInfo.SpinY = spinY;
            _match.ChangeAimInfo(_aimInfo);
        }

        private void ResetAimInfo()
        {
            var turnSettings = _match.TurnSettings;

            _aimInfo = new AimInfo();
            if (turnSettings.BallsAvailableToSelectAsCueball.Count != 1)
                throw new NotImplementedException();

            _aimInfo.CueBall = turnSettings.BallsAvailableToSelectAsCueball.Count == 1 ? turnSettings.BallsAvailableToSelectAsCueball[0] : null;
            _aimInfo.DirectionX = 1;
            _aimInfo.DirectionY = 0;
            _match.ChangeAimInfo(_aimInfo);
        }
    }
}
