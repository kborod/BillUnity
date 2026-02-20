using Kborod.BilliardCore.Enums;

namespace Kborod.MatchManagement
{
    public class PoolEightPlayer : Player
    {
        public PoolBallType BallType = PoolBallType.None;

        public PoolEightPlayer(string id, string name) : base(id, name)
        {
        }
    }
}
