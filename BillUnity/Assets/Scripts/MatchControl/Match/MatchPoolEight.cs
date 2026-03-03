using Kborod.BilliardCore;
using Kborod.BilliardCore.Enums;
using Kborod.BilliardCore.Rules;
using Kborod.BilliardCore.Rules.PoolEight;
using System;

namespace Kborod.MatchManagement
{
    public class MatchPoolEight : MatchBase
    {
        public event Action BallTypesSelected;

        public override string Id { get; protected set; }
        public override GameType GameType { get; protected set; } = GameType.PoolEight;
        public override Player TurningPlayer => _turningPlayer;
        public override Player Player1 => _player1;
        public override Player Player2 => _player2;
        public PoolEightRules PoolEightRules { get; private set; }

        private PoolEightPlayer _turningPlayer;
        private PoolEightPlayer _player1;
        private PoolEightPlayer _player2;

        public void Init(string matchId, int posNum, PoolEightPlayer player1, PoolEightPlayer player2, 
            string turningPlayerId)
        {
            Id = matchId;

            _player1 = player1;
            _player2 = player2;

            _turningPlayer = player1.Id == turningPlayerId ? player1 : player2;

            var ballDatas = Config.GetBallsPositionsForNewGame(GameType, posNum);
            Engine.SetBallDatas(ballDatas);

            PoolEightRules = new PoolEightRules();
            TurnSettings = PoolEightRules.GetFirstTurnSettings(Engine.Balls, PoolBallType.None);

            ChangeState(MatchState.Inited);
        }

        protected override void ShotCompletedHandler(ShotResult shotResult)
        {
            var rulesResult = PoolEightRules.ProcessShot(shotResult, MatchShotsCount <= 1, Engine.Balls, _turningPlayer.BallType);
            var shotData = new ShotResultByRules(
                shotResult, 
                rulesResult.Foul, 
                rulesResult.ReturnedBalls,
                nextTurnPlayerId: rulesResult.TurnTransition ? GetOpponentOf(_turningPlayer).Id : _turningPlayer.Id,
                winUserIdOrNull: rulesResult.GameOver 
                    ? rulesResult.UserWin ? _turningPlayer.Id : GetOpponentOf(_turningPlayer).Id
                    : null
                );

            rulesResult.ReturnedBalls.ForEach(ball => { Engine.ReturnPocketedBall(ball); });

            TrySelectBallTypes(rulesResult);

            TrySwitchTurningPlayer(rulesResult);

            TurnSettings = PoolEightRules.GetTurnSettings(Engine.Balls, _turningPlayer.BallType, rulesResult.Foul != FoulType.None);

            if (rulesResult.GameOver)
                ChangeState(MatchState.Over);
            else
                ChangeState(MatchState.WaitingStartTurn);

            
            InvokeShotCompleted(shotData);
        }

        private void TrySwitchTurningPlayer(PoolEightRulesShotResult rulesResult)
        {
            if (_player1 == null || _player2 == null)
                return;
            if (rulesResult.TurnTransition)
            {
                _turningPlayer = GetOpponentOf(_turningPlayer);
                InvokeTurningPlayerChanged();
            }
        }

        private void TrySelectBallTypes(PoolEightRulesShotResult rulesResult)
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
