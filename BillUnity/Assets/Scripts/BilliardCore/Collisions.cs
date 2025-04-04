using System.Collections.Generic;

namespace Kborod.BilliardCore
{
    public class Collisions
    {
        public float timeToCollision;

        public List<Ball[]> BallsCollisions = new List<Ball[]>();
		public List<KeyValuePair<Ball, MyVector>> BallWallCollisions = new List<KeyValuePair<Ball, MyVector>>();
		public List<KeyValuePair<Ball, Angle>> BallAngleCollisions = new List<KeyValuePair<Ball, Angle>>();
		public List<KeyValuePair<Ball, Pocket>> BallPocketCollisions = new List<KeyValuePair<Ball, Pocket>>();

        /**
		 * добавить коллизию в список коллизий
		 * @param	b1
		 * @param	b2
		 * @param	dt
		 */
        public void process2BallsCollision(Ball b1, Ball b2, float dt)
		{
			if (dt != timeToCollision)
			{
				Clear();
				timeToCollision = dt;
			}
            BallsCollisions.Add(new Ball[] { b1, b2 });
        }
		
		public void processBallWithWallCollision(Ball b, MyVector w, float dt)
		{
			if (dt != timeToCollision)
			{
                Clear();
                timeToCollision = dt;
			}

			BallWallCollisions.Add(new KeyValuePair<Ball, MyVector>(b, w));
		}
		
		public void processBallWithAngleCollision(Ball b, Angle a, float dt)
		{
			if (dt != timeToCollision)
			{
				Clear();
				timeToCollision = dt;
			}
			BallAngleCollisions.Add(new KeyValuePair<Ball, Angle>(b, a));
        }
		
		public void processBallWithPocketCollision(Ball b, Pocket p, float dt)
		{
			if (dt != timeToCollision)
			{
				Clear();
				timeToCollision = dt;
			}
            BallPocketCollisions.Add(new KeyValuePair<Ball, Pocket>(b, p));
		}

		public void Clear()
		{
            timeToCollision = float.MaxValue;

			BallsCollisions.Clear();
			BallWallCollisions.Clear();
			BallAngleCollisions.Clear();
			BallPocketCollisions.Clear();
		}
    }
}
