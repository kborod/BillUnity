using Kborod.BilliardCore;
using Kborod.BilliardCore.Enums;
using Kborod.BilliardCore.Rules;
using Kborod.SharedDto;
using System;
using UnityEngine;

namespace Kborod.MatchManagement
{
    public abstract class MatchBase
    {
        public event Action<MatchState> StateChanged;

        public event Action<float> CueBallHittedWithPower;
        public event Action<ShotTickResult> ShotTickCompleted;
        public event Action<RulesShotResult> ShotCompleted;
        public event Action TurningPlayerChanged;
        public event Action<AimInfo> AimInfoReceived;

        public abstract string Id { get; protected set; }
        public abstract GameType GameType { get; protected set; }
        public abstract Player TurningPlayer { get; }
        public abstract Player Player1 { get; }
        public abstract Player Player2 { get; }

        public MatchState State { get; private set; } = MatchState.Initializing;
        public TurnSettings TurnSettings { get; protected set; }
        public long TurnEndTime { get; private set; }

        public AimInfo AimInfo => _aimInfo;

        public IEngineForUI EngineForUI => Engine;

        protected Engine Engine;
        protected int MatchShotsCount = 0;

        private EngineShotMaker _engineShotMaker;

        private AimInfo _aimInfo;

        public MatchBase()
        {
            Engine = new Engine();
            _engineShotMaker = new EngineShotMaker(Engine);

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

        public virtual void MakeShot(AimInfo aimInfo, int cuePower)
        {
            MatchShotsCount++;

            ChangeState(MatchState.Animation);

            _aimInfo = aimInfo;
            ReplaceBallFromAim();

            _ = _engineShotMaker.MakeShot(aimInfo, cuePower);

            CueBallHittedWithPower?.Invoke(aimInfo.PowerRaw);
        }

        public void ChangeAimInfo(AimInfo info)
        {
            _aimInfo = info;
            ReplaceBallFromAim();
            AimInfoReceived?.Invoke(info);
        }

        public abstract int GetScore(string playerId);

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

        protected void InvokeShotCompleted(RulesShotResult shotResultData) 
        {
            ShotCompleted?.Invoke(shotResultData);
        }

        protected void InvokeTurningPlayerChanged() 
        {
            TurningPlayerChanged?.Invoke();
        }

        private void ReplaceBallFromAim()
        {
            if (_aimInfo.CueBall.HasValue && _aimInfo.CueBallXraw.HasValue && _aimInfo.CueBallYraw.HasValue)
                Engine.ReplaceBall(
                    _aimInfo.CueBall.Value,
                    new Fixed64(_aimInfo.CueBallXraw.Value),
                    new Fixed64(_aimInfo.CueBallYraw.Value),
                    TurnSettings.MoveOnlyInKitchen,
                    correctionAllowed: true);
        }
    }
}
