using Kborod.BilliardCore;
using System;
using UnityEngine;
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

        public abstract GameType GameType { get; }
        public abstract Player TurningPlayer { get; }
        public abstract Player Player1 { get; }
        public abstract Player Player2 { get; }

        public MatchState State { get; private set; }
        public TurnSettings CurrTurnSettings { get; protected set; }
        public bool CanIManageTurningPlayer { get; protected set; }

        [Inject] protected Engine _engine { get; }
        [Inject] protected EnginePlayer _enginePlayer { get; }


        public abstract void MakeShot(int ballNumber, Vector2 direction, float power, float spinX, float spinY, int? pocket = null);


        protected void ChangeState(MatchState state)
        {
            if (State == state)
                throw new Exception($"Already in this state ({state})");
            State = state;
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
