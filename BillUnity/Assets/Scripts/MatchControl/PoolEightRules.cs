using Kborod.BilliardCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kborod.MatchManagement.PoolEight
{
    public class PoolEightRules
    {
        private ReadOnlyCollection<int> SolidBalls = new ReadOnlyCollection<int>(new List<int>() { 1, 2, 3, 4, 5, 6, 7 });
        private ReadOnlyCollection<int> StripedBalls = new ReadOnlyCollection<int>(new List<int>() { 9, 10, 11, 12, 13, 14, 15 });
        private ReadOnlyCollection<int> AllAimBalls = new ReadOnlyCollection<int>(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 12, 13, 14, 15 });
        private ReadOnlyCollection<int> EightBalls = new ReadOnlyCollection<int>(new List<int>() { 8 });

        public BallType GetBallType(int bNumber)
        {
            if (SolidBalls.Contains(bNumber))
                return BallType.Solid;
            else if (StripedBalls.Contains(bNumber))
                return BallType.Striped;
            else
                return BallType.None;
        }

        public ReadOnlyCollection<int> GetBallsByType(BallType ballType)
        {
            return ballType switch
            {
                BallType.Solid => SolidBalls,
                BallType.Striped => StripedBalls,
                BallType.None => null,
                _ => throw new System.NotImplementedException(),
            };
        }

        public RulesShotResult ProcessShot(ShotResult shotResult, List<Ball> balls, BallType playerBallType) 
        {
            var result = new RulesShotResult();
            result.ReturnedBalls = GetReturnedBallsAfterShot(shotResult);

            var foulOrNull = GetFoulInShotOrNull(shotResult, balls, playerBallType);
            var isTurnTransition = foulOrNull != FoulType.None || shotResult.pocketedBalls.Count == 0;
            var ballTypeSelectedOrNull = GetSelectedBallTypeInShotOrNull(shotResult, playerBallType);


            result.TurnTransition = isTurnTransition;
            result.Foul = foulOrNull;
            result.BallTypeSelected = ballTypeSelectedOrNull;
            result.UserWin = foulOrNull != FoulType.None && shotResult.pocketedBalls.Contains(8) && IsAllBallsPocketed(balls, playerBallType);

            return result;
        }

        public TurnSettings GetFirstTurnSettings(List<Ball> balls, BallType playerBallType)
        {
            var result = GetTurnSettings(balls, playerBallType);
            result.CanMoveBall = 0;
            result.MoveOnlyInKitchen = true;
            return result;
        }

        public TurnSettings GetTurnSettings(List<Ball> balls, BallType playerBallType, bool afterFoul)
        {
            var result = GetTurnSettings(balls, playerBallType);
            result.CanMoveBall = afterFoul ? 0 : null;
            result.MoveOnlyInKitchen = false;
            return result;
        }

        private TurnSettings GetTurnSettings(List<Ball> balls, BallType playerBallType)
        {
            var result = new TurnSettings
            {
                BallsAvailableToSelectAsCueball = new List<int>(0),
                BallsAvailableToAim = GetBallsAvailableToAim(balls, playerBallType).ToList()
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

        private FoulType GetFoulInShotOrNull(ShotResult shotResult, List<Ball> balls, BallType playerBallType)
        {
            if (IsFatalFoul())
                return FoulType.P8_EightPocketed;
            if (shotResult.firstCollisionBallNum == null)
                return FoulType.P8_NoCollision;
            else if (playerBallType != BallType.None && GetBallType(shotResult.firstCollisionBallNum.Value) != playerBallType)
                return FoulType.P8_FirstCollision;

            return FoulType.None;

            bool IsFatalFoul()
            {
                return shotResult.pocketedBalls.Contains(8) && !IsAllBallsPocketed(balls, playerBallType);
            }
        }

        private BallType GetSelectedBallTypeInShotOrNull(ShotResult shotResult, BallType playerBallType)
        {
            return (playerBallType == BallType.None && shotResult.pocketedBalls.Count > 0) ? GetBallType(shotResult.pocketedBalls[0]) : BallType.None;
        }

        private ReadOnlyCollection<int> GetBallsAvailableToAim(List<Ball> balls, BallType playerBallType)
        {
            if (playerBallType == BallType.None)
                return AllAimBalls;
            if (IsAllBallsPocketed(balls, playerBallType))
                return EightBalls;

            return GetBallsByType(playerBallType);
        }

        private bool IsAllBallsPocketed(List<Ball> balls, BallType ballType)
        {
            foreach (var bNumber in GetBallsByType(ballType))
            {
                if (balls[bNumber].isRemoved == false)
                    return false;
            }
            return true;
        }
    }

    public class RulesShotResult
    {
        public List<int> ReturnedBalls;
        public bool TurnTransition;
        public FoulType Foul;
        public BallType BallTypeSelected;
        public bool UserWin;
    }
}
