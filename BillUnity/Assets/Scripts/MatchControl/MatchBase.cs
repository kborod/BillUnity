using Kborod.BilliardCore;
using Kborod.BilliardCore.Enums;
using Kborod.BilliardCore.Rules;
using System;
using Zenject;

namespace Kborod.MatchManagement
{
    public abstract class MatchBase
    {
        public event Action<MatchState> StateChanged;

        public event Action<float> CueBallHittedWithPower;
        public event Action<ShotTickResult> ShotTickCompleted;
        public event Action<ShotResultData> ShotCompleted;
        public event Action TurningPlayerChanged;
        public event Action<AimInfo> AimInfoReceived;

        public abstract GameType GameType { get; }
        public abstract Player TurningPlayer { get; }
        public abstract Player Player1 { get; }
        public abstract Player Player2 { get; }

        public MatchState State { get; private set; }
        public TurnSettings TurnSettings { get; protected set; }
        public bool CanIManageTurningPlayer { get; protected set; }

        public AimInfo AimInfo => _aimInfo;

        [Inject] protected Engine _engine { get; }
        [Inject] protected EngineShotMaker _engineShotMaker { get; }

        private AimInfo _aimInfo;

        public abstract void MakeShot(AimInfo aimInfo);

        public void ChangeAimInfo(AimInfo info)
        {
            _aimInfo = info;

            if (info.IsBallMovingNow && info.CueBallX != null && info.CueBallY != null)
                _engine.ReplaceBall(info.CueBall.Value, info.CueBallX.Value, info.CueBallY.Value, TurnSettings.MoveOnlyInKitchen, correctionAllowed: true);

            AimInfoReceived?.Invoke(info);
        }

        protected void ChangeState(MatchState state)
        {
            if (State == state)
                throw new Exception($"Already in this state ({state})");
            State = state;

            if (state == MatchState.PrepeareTurn)
                _aimInfo = new AimInfo();

            StateChanged?.Invoke(state);
        }

        protected void InvokeCueBallHittedWithPower(float power)
        {
            CueBallHittedWithPower?.Invoke(power);
        }

        protected void InvokeShotTick(ShotTickResult tickResult)
        {
            ShotTickCompleted?.Invoke(tickResult);
        }

        protected void InvokeShotCompleted(ShotResultData shotResultData) 
        {
            ShotCompleted?.Invoke(shotResultData);
        }

        protected void InvokeTurningPlayerChanged() 
        {
            TurningPlayerChanged?.Invoke();
        }
    }
}
