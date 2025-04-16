using Kborod.BilliardCore;
using Kborod.MatchManagement.PoolEight;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Kborod.MatchManagement
{
    public class Match
    {
        public event Action<MatchState> StateChanged;

        public event Action<ShotResultData> ShotCompleted;
        public event Action TurningPlayerChanged;
        public event Action BallTypesSelected;

        public MatchState State { get; private set; }

        public PoolEightPlayer TurningPlayer { get; protected set; }

        public TurnSettings CurrTurnSettings { get; protected set; }

        public bool CanIManageTurningPlayer => true;

        public PoolEightRules PoolEightRules;

        [Inject] private Engine _engine;
        [Inject] private EnginePlayer _enginePlayer;

        private PoolEightPlayer _player1;
        private PoolEightPlayer _player2;

        public void StartNew(PoolEightPlayer player1, PoolEightPlayer player2)
        {
            this._player1 = player1;
            this._player2 = player2;

            TurningPlayer = player1;

            _engine.PrepeareNewGame(2);

            _enginePlayer.ShotCompleted += ShotCompletedHandler;

            PoolEightRules = new PoolEightRules();
            CurrTurnSettings = PoolEightRules.GetFirstTurnSettings(_engine.Balls, BallType.None);

            ChangeState(MatchState.PrepeareTurn);
        }


        public void MakeShot(int ballNumber, Vector2 direction, float spinX, float spinY)
        {
            ChangeState(MatchState.Animation);

            _enginePlayer.MakeShot(ballNumber, direction, spinX, spinY);
        }

        private void ShotCompletedHandler(ShotResult shotResult)
        {
            var rulesResult = PoolEightRules.ProcessShot(shotResult, _engine.Balls, TurningPlayer.BallType);
            //UnityEngine.Debug.Log(JsonConvert.SerializeObject(rulesResult, Formatting.Indented));

            CurrTurnSettings = PoolEightRules.GetTurnSettings(_engine.Balls, BallType.None, rulesResult.Foul != FoulType.None);
            //UnityEngine.Debug.Log(JsonConvert.SerializeObject(CurrTurnSettings, Formatting.Indented));

            rulesResult.ReturnedBalls.ForEach(ball => { _engine.ReturnPocketedBall(ball); });

            TrySelectBallTypes(rulesResult);

            var shotData = new ShotResultData() { ShotResult = shotResult, Foul = rulesResult.Foul, ReturnedPocketedBalls = rulesResult.ReturnedBalls };
            ShotCompleted?.Invoke(shotData);

            TrySwitchTurningPlayer(rulesResult);

            ChangeState(MatchState.PrepeareTurn);
        }

        public TurnSettings GetCurrTurnSettings()
        {
            if (State != MatchState.PrepeareTurn)
                throw new Exception("TurnSettings available during player turn");
            return CurrTurnSettings;
        }

        private void ChangeState(MatchState state)
        {
            State = state;
            StateChanged?.Invoke(state);
        }

        private void TrySwitchTurningPlayer(RulesShotResult rulesResult)
        {
            if (rulesResult.TurnTransition)
            {
                TurningPlayer = GetOpponentOf(TurningPlayer);
                TurningPlayerChanged?.Invoke();
            }
        }

        private void TrySelectBallTypes(RulesShotResult rulesResult)
        {
            if (TurningPlayer.BallType != BallType.None || rulesResult.BallTypeSelected == BallType.None)
                return;
            TurningPlayer.BallType = rulesResult.BallTypeSelected;
            GetOpponentOf(TurningPlayer).BallType = TurningPlayer.BallType == BallType.Striped ? BallType.Solid : BallType.Striped;

            BallTypesSelected?.Invoke();
        }

        private PoolEightPlayer GetOpponentOf(PoolEightPlayer player)
        {
            if (_player1 == player && _player2 != null)
                return _player2;
            else
                return _player1;
        }
    }

    public enum MatchState
    {
        Init,
        PrepeareTurn,
        Animation,
    }

    public struct TurnSettings
    {
        public int? CanMoveBall { get;set; }
        public bool MoveOnlyInKitchen { get; set; }

        public List<int> BallsAvailableToAim { get; set; }
        public List<int> BallsAvailableToSelectAsCueball { get; set; }
    }

    public enum BallType
    {
        None,
        Striped,
        Solid
    }
}
