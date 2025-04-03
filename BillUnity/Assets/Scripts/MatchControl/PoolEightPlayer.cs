using System.Collections.Generic;

namespace Kborod.MatchManagement
{
    public class PoolEightPlayer : Player
    {
        public BallType BallType = BallType.None;
        public List<int> PocketedBalls = new List<int>();

        public PoolEightPlayer(string id, string name) : base(id, name)
        {
        }
    }
}
