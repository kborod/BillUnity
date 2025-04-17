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

        public RulesShotResult ProcessShot(ShotResult shotResult, List<Ball> balls, PoolBallType playerBallType) 
        {
            var result = new RulesShotResult();
            result.ReturnedBalls = GetReturnedBallsAfterShot(shotResult);

            var foul = GetFoulInShot(shotResult, balls, playerBallType);
            var ballTypeSelected = GetSelectedBallTypeInShot(shotResult, playerBallType);

            result.TurnTransition = IsTurnTransition(shotResult, foul, ballTypeSelected != PoolBallType.None, playerBallType);
            result.Foul = foul;
            result.BallTypeSelected = ballTypeSelected;
            result.UserWin = foul != FoulType.None && shotResult.PocketedBalls.Contains(8) && IsAllBallsPocketed(balls, playerBallType);

            return result;
        }

        public TurnSettings GetFirstTurnSettings(List<Ball> balls, PoolBallType playerBallType)
        {
            var result = GetTurnSettings(balls, playerBallType);
            result.CanMoveBall = 0;
            result.MoveOnlyInKitchen = true;
            return result;
        }

        public TurnSettings GetTurnSettings(List<Ball> balls, PoolBallType playerBallType, bool afterFoul)
        {
            var result = GetTurnSettings(balls, playerBallType);
            result.CanMoveBall = afterFoul ? 0 : null;
            result.MoveOnlyInKitchen = false;
            return result;
        }

        private ReadOnlyCollection<int> GetBallsByType(PoolBallType ballType)
        {
            return ballType switch
            {
                PoolBallType.Solid => SolidBalls,
                PoolBallType.Striped => StripedBalls,
                PoolBallType.None => null,
                _ => throw new System.NotImplementedException(),
            };
        }

        private TurnSettings GetTurnSettings(List<Ball> balls, PoolBallType playerBallType)
        {
            var result = new TurnSettings
            {
                BallsAvailableToSelectAsCueball = new List<int>() { 0 },
                BallsAvailableToAim = GetBallsAvailableToAim(balls, playerBallType).ToList()
            };
            return result;
        }


        private List<int> GetReturnedBallsAfterShot(ShotResult shotResult)
        {
            var returnedBalls = new List<int>();
            if (shotResult.PocketedBalls.Contains(0))
                returnedBalls.Add(0);
            return returnedBalls;
        }        

        private FoulType GetFoulInShot(ShotResult shotResult, List<Ball> balls, PoolBallType playerBallType)
        {
            if (IsFatalFoul())
                return FoulType.P8_EightPocketed;
            if (shotResult.FirstCollisionBallNum == null)
                return FoulType.P8_NoCollision;
            else if (playerBallType != PoolBallType.None && shotResult.FirstCollisionBallNum.Value.GetPoolBallType() != playerBallType)
                return FoulType.P8_FirstCollision;
            else if (shotResult.PocketedBalls.Contains(0))
                return FoulType.P8_CueBallPocketed;

            return FoulType.None;

            bool IsFatalFoul()
            {
                return shotResult.PocketedBalls.Contains(8) && !IsAllBallsPocketed(balls, playerBallType);
            }
        }

        private bool IsTurnTransition(ShotResult shotResult, FoulType foul, bool ballTypeSelected, PoolBallType playerBallType)
        {
            if (foul != FoulType.None)
                return true;
            if (shotResult.PocketedBalls.Count == 0)
                return true;
            if (ballTypeSelected)
                return false;
            if (playerBallType != PoolBallType.None)
            {
                foreach (var pocketedBall in shotResult.PocketedBalls)
                {
                    if (pocketedBall.GetPoolBallType() == playerBallType)
                        return false;
                }
            }
            return true;
        }

        private PoolBallType GetSelectedBallTypeInShot(ShotResult shotResult, PoolBallType playerBallType)
        {
            return (playerBallType == PoolBallType.None && shotResult.PocketedBalls.Count > 0) ? shotResult.PocketedBalls[0].GetPoolBallType() : PoolBallType.None;
        }

        private ReadOnlyCollection<int> GetBallsAvailableToAim(List<Ball> balls, PoolBallType playerBallType)
        {
            if (playerBallType == PoolBallType.None)
                return AllAimBalls;
            if (IsAllBallsPocketed(balls, playerBallType))
                return EightBalls;

            return GetBallsByType(playerBallType);
        }

        private bool IsAllBallsPocketed(List<Ball> balls, PoolBallType ballType)
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
        public PoolBallType BallTypeSelected;
        public bool UserWin;
    }
}
