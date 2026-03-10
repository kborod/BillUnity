using Kborod.BilliardCore;
using Kborod.DomainModel;
using System;
using System.Collections.Generic;
using UnityEngine;

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
            _aimInfo.PowerRaw = Fixed64.FromDouble(power).Raw;
            AimInfoChanged?.Invoke(_aimInfo);
        }

        public void SelectPower(float power)
        {
            _aimInfo.PowerRaw = Fixed64.FromDouble(power).Raw;
            ShotMade?.Invoke(_aimInfo);
        }

        public void ChangeDirection(float directionX, float directionY)
        {
            _aimInfo.DirectionXraw = Fixed64.FromDouble(directionX).Raw;
            _aimInfo.DirectionYraw = Fixed64.FromDouble(directionY).Raw;
            AimInfoChanged?.Invoke(_aimInfo);
        }

        public void MoveCueBall(float? x, float? y)
        {
            _aimInfo.CueBallXraw = Fixed64.FromDouble(x.Value).Raw;
            _aimInfo.CueBallYraw = Fixed64.FromDouble(y.Value).Raw;
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
            _aimInfo.SpinXraw = Fixed64.FromDouble(spinX).Raw;
            _aimInfo.SpinYraw = Fixed64.FromDouble(spinY).Raw;
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
            _aimInfo.DirectionXraw = Fixed64.FromDouble(0.995037d).Raw;
            _aimInfo.DirectionYraw = Fixed64.FromDouble(0.0995037d).Raw;
            _aimInfo.CueId = _cuesModel.CurrentCueId;
            AimInfoChanged?.Invoke(_aimInfo);
        }
    }
}