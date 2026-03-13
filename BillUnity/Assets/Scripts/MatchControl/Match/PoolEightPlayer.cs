using Kborod.BilliardCore.Enums;
using Kborod.SharedDto;

namespace Kborod.MatchManagement
{
    public class PoolEightPlayer : Player
    {
        public PoolBallType BallType { get; private set; } = PoolBallType.None;

        public PoolEightPlayer(UserProfile profile) : base(profile)
        {
        }

        public void SetBallType(PoolBallType ballType)
        {
            BallType = ballType;
        }
    }
}
