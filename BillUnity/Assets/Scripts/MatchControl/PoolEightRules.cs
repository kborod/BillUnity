using Kborod.BilliardCore;
using System.Collections.Generic;

namespace Kborod.MatchManagement.PoolEight
{
    public class PoolEightRules
    {
        private List<int> solidBalls = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };
        private List<int> stripedBalls = new List<int>() { 9, 10, 11, 12, 13, 14, 15 };
        private List<int> allAimBalls = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15 };

        public RulesShotResult ProcessShot(ShotResult shotResult, Engine engine, BallType playerBallType) 
        {
            var result = new RulesShotResult();
            result.ReturnedBalls = GetReturnedBallsAfterShot(shotResult);

            var foulOrNull = GetFoulInShotOrNull(shotResult, engine, playerBallType);
            var isTurnTransition = foulOrNull != null || shotResult.pocketedBalls.Count == 0;
            var ballTypeSelectedOrNull = GetSelectedBallTypeInShotOrNull(shotResult, playerBallType);


            result.TurnTransition = isTurnTransition;
            result.FoulOrNull = foulOrNull;
            result.BallTypeSelectedOrNull = ballTypeSelectedOrNull;
            result.UserWin = shotResult.pocketedBalls.Contains(8) && IsAllBallsPocketed(engine, playerBallType);

            return result;
        }

        public TurnSettings GetFirstTurnSettings(Engine engine, BallType playerBallType)
        {
            var result = GetTurnSettings(engine, playerBallType);
            result.CanMoveBall = 0;
            result.MoveOnlyInKitchen = true;
            return result;
        }

        public TurnSettings GetTurnSettings(Engine engine, BallType playerBallType, bool afterFoul)
        {
            var result = GetTurnSettings(engine, playerBallType);
            result.CanMoveBall = afterFoul ? 0 : null;
            result.MoveOnlyInKitchen = false;
            return result;
        }

        private TurnSettings GetTurnSettings(Engine engine, BallType playerBallType)
        {
            var result = new TurnSettings
            {
                BallsAvailableToSelectAsCueball = new List<int>(0),
                BallsAvailableToAim = GetBallsAvailableToAim(engine, playerBallType)
            };
            return result;
        }


        private List<int> GetReturnedBallsAfterShot(ShotResult shotResult)
        {
            var returnedBalls = new List<int>();
            if (shotResult.pocketedBalls.Contains(0))
                returnedBalls.Add(0);
            return returnedBalls;
        }        

        private FoulType? GetFoulInShotOrNull(ShotResult shotResult, Engine engine, BallType playerBallType)
        {
            if (IsFatalFoul())
                return FoulType.P8_EightPocketed;
            if (shotResult.firstCollisionBallNum == null)
                return FoulType.P8_NoCollision;
            else if (playerBallType != BallType.None && GetBallType(shotResult.firstCollisionBallNum.Value) != playerBallType)
                return FoulType.P8_FirstCollision;

            return null;

            bool IsFatalFoul()
            {
                return shotResult.pocketedBalls.Contains(8) && !IsAllBallsPocketed(engine, playerBallType);
            }
        }

        private BallType? GetSelectedBallTypeInShotOrNull(ShotResult shotResult, BallType playerBallType)
        {
            return (playerBallType == BallType.None && shotResult.pocketedBalls.Count > 0) ? GetBallType(shotResult.pocketedBalls[0]) : null;
        }

        private List<int> GetBallsAvailableToAim(Engine engine, BallType playerBallType)
        {
            if (playerBallType != BallType.None && IsAllBallsPocketed(engine, playerBallType))
                return new List<int>() { 8 };

            return GetBallsByType(playerBallType);
        }

        private List<int> GetBallsByType(BallType playerBallType)
        {
            return playerBallType switch
            {
                BallType.Solid => solidBalls,
                BallType.Striped => stripedBalls,
                BallType.None => allAimBalls,
                _ => throw new System.NotImplementedException(),
            };
        }

        private bool IsAllBallsPocketed(Engine engine, BallType ballType)
        {
            foreach (var bNumber in GetBallsByType(ballType))
            {
                if (engine.balls[bNumber].isRemoved == false)
                    return false;
            }
            return true;
        }
        
        private BallType GetBallType(int bNumber)
        {
            if (solidBalls.Contains(bNumber))
                return BallType.Solid;
            else if (stripedBalls.Contains(bNumber))
                return BallType.Striped;
            else
                return BallType.None;
        }
    }

    public class RulesShotResult
    {
        public List<int> ReturnedBalls;
        public bool TurnTransition;
        public FoulType? FoulOrNull;
        public BallType? BallTypeSelectedOrNull;
        public bool UserWin;
    }
}
