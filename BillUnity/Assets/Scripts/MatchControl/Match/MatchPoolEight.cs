using Kborod.BilliardCore;
using Kborod.BilliardCore.Enums;
using Kborod.BilliardCore.Rules;
using Kborod.BilliardCore.Rules.PoolEight;
using System;
using UnityEngine;

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

            TurnSettings = PoolEightRules.GetFirstTurnSettings(Engine.Balls);

            ChangeState(MatchState.Inited);
        }

        public override void MakeShot(AimInfo aimInfo, int cuePower)
        {
            CalculateShot(aimInfo, cuePower);

            base.MakeShot(aimInfo, cuePower);
        }

        protected override void ShotCompletedHandler(ShotResult shotResult)
        {
            PoolEightTurnResults turnResults = shotResult.CompleteTurnWithRules_P8(
                Engine,
                _turningPlayer.Id,
                _turningPlayer.BallType,
                GetOpponentOf(_turningPlayer).Id,
                MatchShotsCount <= 1
                );

            TrySelectBallTypes(
                turnResults.RulesResult.CurrTurnPlayerId, 
                turnResults.PoolEightRulesResult.BallTypeSelected);

            SetTurningPlayer(turnResults.RulesResult.NextTurnPlayerId);

            TurnSettings = turnResults.NextTurnSettings;

            if (turnResults.RulesResult.WinUserIdOrNull != null)
                ChangeState(MatchState.Over);
            else
                ChangeState(MatchState.WaitingStartTurn);

            CheckResult(turnResults);

            InvokeShotCompleted(turnResults.RulesResult);
        }

        private void SetTurningPlayer(string id)
        {
            if (_turningPlayer.Id == id)
                return;
            _turningPlayer = _player1.Id == id ? _player1 : GetOpponentOf(_player1);
            InvokeTurningPlayerChanged();
        }

        private void TrySelectBallTypes(string playerId, PoolBallType selectedBallType)
        {
            if (_player1.BallType != PoolBallType.None || selectedBallType == PoolBallType.None)
                return;

            var player = GetPlayer(playerId);
            player.BallType = selectedBallType;
            GetOpponentOf(player).BallType = player.BallType.GetOpposite();

            BallTypesSelected?.Invoke();
        }

        private PoolEightPlayer GetOpponentOf(PoolEightPlayer player)
        {
            if (_player1 == player && _player2 != null)
                return _player2;
            else
                return _player1;
        }

        private PoolEightPlayer GetPlayer(string playerId)
        {
            if (_player1.Id == playerId)
                return _player1;
            else
                return _player2;
        }

        private PoolEightTurnResults _calcResult;
        private PoolShotCalculator _shotCalculator = new PoolShotCalculator();
        private void CalculateShot(AimInfo aimInfo, int cuePower)
        {
            return;
            var ballDatas = Engine.GetBallDatas();
            var moveOnlyKitchen = TurnSettings.MoveOnlyInKitchen;

            var context = new CalculatePoolShotContext(Id, GameType, ballDatas, aimInfo, _turningPlayer.Id, _turningPlayer.BallType,
                GetOpponentOf(_turningPlayer).Id, MatchShotsCount == 0, TurnSettings.MoveOnlyInKitchen, cuePower);
            
            //var _shotCalculator = new ShotCalculator();
            _calcResult = _shotCalculator.CalculateShot(context);
        }

        private void CheckResult(PoolEightTurnResults turnResults)
        {
            return;
            Debug.Log("------CHECKING-------");
            var differences = turnResults.RulesResult.GetDifferences(_calcResult.RulesResult);
            if (!string.IsNullOrEmpty(differences))
                Debug.Log(differences);
            Debug.Log("------COMPLETE-------");
        }
    }
}
