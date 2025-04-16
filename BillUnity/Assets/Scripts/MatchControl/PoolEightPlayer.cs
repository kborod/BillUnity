using System.Collections.Generic;

namespace Kborod.MatchManagement
{
    public class PoolEightPlayer : Player
    {
        public BallType BallType = BallType.None;
        public List<int> PocketedBalls = new List<int>();
        public int MaxPocketedBallsCount = 7;

        public PoolEightPlayer(string id, string name) : base(id, name)
        {
        }
    }
}
