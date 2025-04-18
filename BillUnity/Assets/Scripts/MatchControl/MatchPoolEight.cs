using Kborod.BilliardCore;
using Kborod.MatchManagement.PoolEight;
using System;
using UnityEngine;

namespace Kborod.MatchManagement
{
    public class MatchPoolEight : MatchBase
    {
        public event Action BallTypesSelected;

        public override GameType GameType => GameType.PoolEight;
        public override Player TurningPlayer => _turningPlayer;
        public override Player Player1 => _player1;
        public override Player Player2 => _player2;
        public PoolEightRules PoolEightRules { get; private set; }

        private PoolEightPlayer _turningPlayer;
        private PoolEightPlayer _player1;
        private PoolEightPlayer _player2;

        public void StartNew(PoolEightPlayer player1, PoolEightPlayer player2)
        {
            this._player1 = player1;
            this._player2 = player2;

            _turningPlayer = player1;

            _engine.PrepeareNewGame(2);

            _enginePlayer.ShotTickCompleted += ShotTickCompletedHandler;
            _enginePlayer.ShotCompleted += ShotCompletedHandler;

            PoolEightRules = new PoolEightRules();
            CurrTurnSettings = PoolEightRules.GetFirstTurnSettings(_engine.Balls, PoolBallType.None);

            CanIManageTurningPlayer = true;

            ChangeState(MatchState.PrepeareTurn);
        }

        public void Dispose()
        {
            _enginePlayer.ShotTickCompleted -= ShotTickCompletedHandler;
            _enginePlayer.ShotCompleted -= ShotCompletedHandler;
        }

        public override void MakeShot(int ballNumber, Vector2 direction, float power, float spinX, float spinY, int? pocket = null)
        {
            ChangeState(MatchState.Animation);

            _enginePlayer.MakeShot(ballNumber, direction * power, spinX, spinY);

            InvokeCueBallHittedWithPower(power);
        }

        private void ShotTickCompletedHandler(ShotTickResult tickResult)
        {
            InvokeShotTick(tickResult);
        }

        private void ShotCompletedHandler(ShotResult shotResult)
        {
            var rulesResult = PoolEightRules.ProcessShot(shotResult, _engine.Balls, _turningPlayer.BallType);
            //UnityEngine.Debug.Log(JsonConvert.SerializeObject(rulesResult, Formatting.Indented));

            rulesResult.ReturnedBalls.ForEach(ball => { _engine.ReturnPocketedBall(ball); });

            TrySelectBallTypes(rulesResult);

            var shotData = new ShotResultData(shotResult, rulesResult.Foul, rulesResult.ReturnedBalls, rulesResult.UserWin);
            InvokeShotCompleted(shotData);

            TrySwitchTurningPlayer(rulesResult);

            CurrTurnSettings = PoolEightRules.GetTurnSettings(_engine.Balls, _turningPlayer.BallType, rulesResult.Foul != FoulType.None);

            ChangeState(MatchState.PrepeareTurn);
        }

        public TurnSettings GetCurrTurnSettings()
        {
            if (State != MatchState.PrepeareTurn)
                throw new Exception("TurnSettings available during player turn");
            return CurrTurnSettings;
        }

        private void TrySwitchTurningPlayer(RulesShotResult rulesResult)
        {
            if (rulesResult.TurnTransition)
            {
                _turningPlayer = GetOpponentOf(_turningPlayer);
                InvokeTurningPlayerChanged();
            }
        }

        private void TrySelectBallTypes(RulesShotResult rulesResult)
        {
            if (_turningPlayer.BallType != PoolBallType.None || rulesResult.BallTypeSelected == PoolBallType.None)
                return;
            _turningPlayer.BallType = rulesResult.BallTypeSelected;
            GetOpponentOf(_turningPlayer).BallType = _turningPlayer.BallType == PoolBallType.Striped ? PoolBallType.Solid : PoolBallType.Striped;

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
}
