using Kborod.BilliardCore.Enums;
using System.Collections.Generic;

namespace Kborod.BilliardCore.Rules
{
    public class ShotResultByRules
    {
        public bool IsShotSkipped => Foul == FoulType.ShotSkipped;

        public ShotResult ShotResult { get; private set; }

        public FoulType Foul { get; private set; } = FoulType.None;

        public List<int> ReturnedPocketedBalls { get; private set; }

        public string NextTurnPlayerId{ get; private set; }

        public string WinUserIdOrNull { get; private set; }

        public ShotResultByRules(ShotResult shotResult, FoulType foul, List<int> returnedPocketedBalls, string nextTurnPlayerId, string winUserIdOrNull)
        {
            ShotResult = shotResult;
            Foul = foul;
            ReturnedPocketedBalls = returnedPocketedBalls;
            NextTurnPlayerId = nextTurnPlayerId;
            WinUserIdOrNull = winUserIdOrNull;
        }
    }
}
