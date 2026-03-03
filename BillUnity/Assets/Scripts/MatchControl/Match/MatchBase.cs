using Kborod.BilliardCore;
using Kborod.BilliardCore.Enums;
using Kborod.BilliardCore.Rules;
using System;
using UnityEngine;

namespace Kborod.MatchManagement
{
    public abstract class MatchBase
    {
        public event Action<MatchState> StateChanged;

        public event Action<float> CueBallHittedWithPower;
        public event Action<ShotTickResult> ShotTickCompleted;
        public event Action<ShotResultByRules> ShotCompleted;
        public event Action TurningPlayerChanged;
        public event Action<AimInfo> AimInfoReceived;

        public abstract string Id { get; protected set; }
        public abstract GameType GameType { get; protected set; }
        public abstract Player TurningPlayer { get; }
        public abstract Player Player1 { get; }
        public abstract Player Player2 { get; }

        public MatchState State { get; private set; }
        public TurnSettings TurnSettings { get; protected set; }
        public long TurnEndTime { get; private set; }

        public AimInfo AimInfo => _aimInfo;

        public IEngineForUI EngineForUI => _engine;

        protected Engine _engine;

        private EngineShotMaker _engineShotMaker;

        private AimInfo _aimInfo;

        public MatchBase()
        {
            _engine = new Engine();
            _engineShotMaker = new EngineShotMaker(_engine);

            _engineShotMaker.ShotTickCompleted += ShotTickCompletedHandler;
            _engineShotMaker.ShotCompleted += ShotCompletedHandler;
        }

        public void Dispose()
        {
            _engineShotMaker.ShotTickCompleted -= ShotTickCompletedHandler;
            _engineShotMaker.ShotCompleted -= ShotCompletedHandler;
        }

        public void StartTurn(StartTurnData startTurnData)
        {
            TurnEndTime = startTurnData.TurnEndTimestamp;
            ChangeState(MatchState.PrepeareTurn);
        }

        public void MakeShot(AimInfo aimInfo, float cuePower = 300)
        {
            ChangeState(MatchState.Animation);

            _aimInfo = aimInfo;
            ReplaceBallFromAim();

            _ = _engineShotMaker.MakeShot(aimInfo.CueBall.Value, aimInfo.DirectionX * aimInfo.Power * cuePower, aimInfo.DirectionY * aimInfo.Power * cuePower, aimInfo.SpinX, aimInfo.SpinY);

            CueBallHittedWithPower?.Invoke(aimInfo.Power);
        }

        public void ChangeAimInfo(AimInfo info)
        {
            _aimInfo = info;
            ReplaceBallFromAim();
            AimInfoReceived?.Invoke(info);
        }

        protected virtual void ShotTickCompletedHandler(ShotTickResult tickResult)
        {
            ShotTickCompleted?.Invoke(tickResult);
        }

        protected abstract void ShotCompletedHandler(ShotResult shotResult);

        protected void ChangeState(MatchState state)
        {
            if (State == state)
                throw new Exception($"Already in this state ({state})");
            State = state;

            if (state == MatchState.PrepeareTurn)
                _aimInfo = new AimInfo();

            Debug.Log($"State changed to {state}");

            StateChanged?.Invoke(state);
        }

        protected void InvokeShotCompleted(ShotResultByRules shotResultData) 
        {
            ShotCompleted?.Invoke(shotResultData);
        }

        protected void InvokeTurningPlayerChanged() 
        {
            TurningPlayerChanged?.Invoke();
        }

        private void ReplaceBallFromAim()
        {
            if (_aimInfo.CueBall.HasValue && _aimInfo.CueBallX.HasValue && _aimInfo.CueBallY != null)
                _engine.ReplaceBall(
                    _aimInfo.CueBall.Value,
                    _aimInfo.CueBallX.Value,
                    _aimInfo.CueBallY.Value,
                    TurnSettings.MoveOnlyInKitchen,
                    correctionAllowed: true);
        }
    }
}
