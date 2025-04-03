using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.MatchManagement.PoolEight;
using System;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace Kborod.MatchManagement
{
    public class Match
    {
        public event Action<MatchState> StateChanged;

        public event Action TurningPlayerChanged;
        public event Action BallTypesSelected;

        public MatchState State { get; private set; }

        public Engine Engine { get; protected set; }

        public PoolEightPlayer TurningPlayer { get; protected set; }

        public TurnSettings CurrTurnSettings { get; protected set; }

        public bool CanIManageTurningPlayer => true;

        private PoolEightPlayer player1;
        private PoolEightPlayer player2;

        private PoolEightRules rules;

        public Match(PoolEightPlayer player1, PoolEightPlayer player2)
        {
            this.player1 = player1;
            this.player2 = player2;

            TurningPlayer = player1;

            Engine = new Engine();
            Engine.prepeareNewGame(2);
            rules = new PoolEightRules();
            CurrTurnSettings = rules.GetFirstTurnSettings(Engine, BallType.None);

            ChangeState(MatchState.PrepeareTurn);
        }

        public async void MakeShot(int ballNumber, Vector2 direction, float spinX, float spinY, Action<float> processHandler, Action<ShotResultData> completeHandler)
        {
            ChangeState(MatchState.Animation);

            Engine.makeShot(direction.x, direction.y, ballNumber, spinX, spinY);

            while (true)
            {
                var deltaMS = (int)(Time.deltaTime * 1000);
                var shotResultOrNull = Engine.UpdateModel(deltaMS);
                processHandler?.Invoke(deltaMS);
                if (shotResultOrNull != null) 
                {
                    var rulesResult = rules.ProcessShot(shotResultOrNull, Engine, TurningPlayer.BallType);
                    UnityEngine.Debug.Log(JsonConvert.SerializeObject(rulesResult));

                    CurrTurnSettings = rules.GetTurnSettings(Engine, BallType.None, rulesResult.FoulOrNull != null);
                    UnityEngine.Debug.Log(JsonConvert.SerializeObject(CurrTurnSettings));

                    rulesResult.ReturnedBalls.ForEach(ball => { Engine.returnPocketedBall(ball); });

                    var shotData = new ShotResultData() { ShotResult = shotResultOrNull, ReturnedPocketedBalls = rulesResult.ReturnedBalls };
                    completeHandler?.Invoke(shotData);

                    TrySelectBallTypes(rulesResult);
                    TrySwitchTurningPlayer(rulesResult);

                    ChangeState(MatchState.PrepeareTurn);
                    break;
                }

                await UniTask.NextFrame();
            }
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
            if (TurningPlayer.BallType != BallType.None || rulesResult.BallTypeSelectedOrNull == null)
                return;
            TurningPlayer.BallType = rulesResult.BallTypeSelectedOrNull.Value;
            GetOpponentOf(TurningPlayer).BallType = TurningPlayer.BallType == BallType.Striped ? BallType.Solid : BallType.Striped;

            BallTypesSelected?.Invoke();
        }

        private PoolEightPlayer GetOpponentOf(PoolEightPlayer player)
        {
            if (player1 == player && player2 != null)
                return player2;
            else
                return player1;
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
