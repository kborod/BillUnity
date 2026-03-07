using Kborod.BilliardCore;
using Kborod.DomainModel;
using System;
using System.Collections.Generic;

namespace Kborod.MatchManagement
{
    public class MyInput : IInputProvider
    {
        public event Action<AimInfo> AimInfoChanged;
        public event Action<AimInfo> ShotMade;

        public bool CanIManageTurningPlayer => _canManagePlayers.Contains(_match.TurningPlayer.Id);
        public int? CurrentCueBallOrNull => _aimInfo.CueBall;
        public AimInfo CurrentAimInfo => _aimInfo;

        private MatchBase _match;
        private CuesModel _cuesModel;

        private AimInfo _aimInfo;

        private List<string> _canManagePlayers;

        public MyInput(MatchBase match, CuesModel cuesModel, List<string> canManagePlayerIds)
        {
            _match = match;
            _cuesModel = cuesModel;
            _canManagePlayers = canManagePlayerIds;

            _match.StateChanged += MatchStateChangedHandler;
            _cuesModel.CueChanged += CueChangedHandler;

            ResetAimInfo();
        }

        public void Dispose()
        {
            _match.StateChanged -= MatchStateChangedHandler;
            _cuesModel.CueChanged -= CueChangedHandler;
        }

        private void MatchStateChangedHandler(MatchState state)
        {
            if (state == MatchState.PrepeareTurn)
                ResetAimInfo();
        }

        public void ChangePower(float power)
        {
            _aimInfo.Power = power;
            AimInfoChanged?.Invoke(_aimInfo);
        }

        public void SelectPower(float power)
        {
            _aimInfo.Power = power;
            ShotMade?.Invoke(_aimInfo);
        }

        public void ChangeDirection(float directionX, float directionY)
        {
            _aimInfo.DirectionX = directionX;
            _aimInfo.DirectionY = directionY;
            AimInfoChanged?.Invoke(_aimInfo);
        }

        public void MoveCueBall(float? x, float? y)
        {
            _aimInfo.CueBallX = x;
            _aimInfo.CueBallY = y;
            _aimInfo.IsBallMovingNow = true;
            AimInfoChanged?.Invoke(_aimInfo);
        }

        public void MoveCueBallCompleted()
        {
            _aimInfo.IsBallMovingNow = false;
            AimInfoChanged?.Invoke(_aimInfo);
        }

        public void ChangeSpin(float spinX, float spinY)
        {
            _aimInfo.SpinX = spinX;
            _aimInfo.SpinY = spinY;
            AimInfoChanged?.Invoke(_aimInfo);
        }

        private void CueChangedHandler()
        {
            _aimInfo.CueId = _cuesModel.CurrentCueId;
            AimInfoChanged?.Invoke(_aimInfo);
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
            _aimInfo.CueId = _cuesModel.CurrentCueId;
            AimInfoChanged?.Invoke(_aimInfo);
        }
    }
}
