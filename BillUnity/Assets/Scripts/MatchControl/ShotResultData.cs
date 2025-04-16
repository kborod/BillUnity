using Kborod.BilliardCore;
using System.Collections.Generic;

namespace Kborod.MatchManagement
{
    public class ShotResultData
    {
        public ShotResult ShotResult;

        public FoulType Foul = FoulType.None;

        public List<int> ReturnedPocketedBalls;
	}
}
