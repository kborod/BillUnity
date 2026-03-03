using System;
using System.Collections.Generic;

namespace Kborod.BilliardCore
{
    public class ShotCalculator
    {
        private Engine _engine = new Engine();

        public (ShotResult, List<BallData>) CalculateShot(
            List<BallData> balls, AimInfo aimInfo, bool onlyKitchen, float cuePower)
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
                    onlyKitchen, correctionAllowed: false) == false)
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

            return (shotResult, _engine.GetBallDatas());
        }

    }
}
