using Kborod.BilliardCore.Enums;
using System;
using System.Collections.Generic;

namespace Kborod.BilliardCore
{
    public class ShotCalculator
    {
        private Engine _engine = new Engine();

        public PoolEightTurnResults CalculateShot(
            string turningPlayerId, string oppPlayerId,
            List<BallData> balls, AimInfo aimInfo, bool isFirstSHot, 
            bool onlyKitchen, float cuePower, PoolBallType turnPlayerBallType)
        {
            if (aimInfo.CueBall.HasValue == false)
            {
                throw new Exception("cueBallNum is null");
            }

            _engine = new Engine();

            _engine.SetBallDatas(balls);

            if (aimInfo.CueBallX.HasValue && aimInfo.CueBallY.HasValue)
            {
                if (_engine.ReplaceBall(aimInfo.CueBall.Value, aimInfo.CueBallX.Value, aimInfo.CueBallY.Value, 
                    onlyKitchen, correctionAllowed: true) == false)
                {
                    throw new Exception("ReplaceBall error");
                }
            }
            _engine.MakeShot(
                aimInfo.DirectionX * aimInfo.Power * cuePower,
                aimInfo.DirectionY * aimInfo.Power * cuePower, 
                aimInfo.CueBall.Value, 
                aimInfo.SpinX, 
                aimInfo.SpinY);

            _engine.UpdateModel(int.MaxValue, out var tickResult, out var shotResult);

            return shotResult.CompleteTurnWithRules_P8(
                _engine,
                turningPlayerId,
                turnPlayerBallType,
                oppPlayerId,
                isFirstSHot
                );

            //var poolEightRulesResult = PoolEightRules.ProcessShot(shotResult, isFirstSHot, _engine.Balls, playerBallType);
            //poolEightRulesResult.ReturnedBalls.ForEach(ball => { _engine.ReturnPocketedBall(ball); });

            //var nextTurningPlayer = !poolEightRulesResult.GameOver && poolEightRulesResult.TurnTransition ? oppPlayerId : turningPlayerId;
            //var winUserId = poolEightRulesResult.GameOver
            //        ? poolEightRulesResult.UserWin ? turningPlayerId : oppPlayerId
            //        : null;

            //var shotResultByRules = new RulesShotResult(
            //    poolEightRulesResult.Foul,
            //    _engine.GetBallDatas(),
            //    shotResult.PocketedBalls,
            //    poolEightRulesResult.ReturnedBalls,
            //    nextTurningPlayer,
            //    winUserId
            //    );

            //var nextTurningPlayerBallType = PoolBallType.None;
            //if (poolEightRulesResult.BallTypeSelected != PoolBallType.None)
            //{
            //    nextTurningPlayerBallType = poolEightRulesResult.BallTypeSelected;
            //}
            //else if (playerBallType != PoolBallType.None)
            //{
            //    nextTurningPlayerBallType = nextTurningPlayer == turningPlayerId ? playerBallType : playerBallType == PoolBallType.Striped ? PoolBallType.Solid : PoolBallType.Striped;
            //}

            //var nextTurnSettings = PoolEightRules.GetTurnSettings(_engine.Balls, nextTurningPlayerBallType, poolEightRulesResult.Foul != FoulType.None);


            //return (shotResultByRules, nextTurnSettings);
        }
    }
}
