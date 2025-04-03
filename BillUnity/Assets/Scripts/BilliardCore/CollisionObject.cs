using UnityEngine;

namespace Kborod.BilliardCore
{
    public class CollisionObject
    {
        public float timeToCollision;
		
		public int ballsCollisionCount = 0;
		
		public Ball b1coll1 = null;
		public Ball b2coll1 = null;
		
		public Ball b1coll2 = null;
		public Ball b2coll2 = null;
		
		public Ball b1coll3 = null;
		public Ball b2coll3 = null;
		
		public int ballWallCollisionCount = 0;
		
		public Ball bWallColl1 = null;
		public MyVector wWallColl1 	= null;
		
		public Ball bWallColl2 = null;
		public MyVector wWallColl2 = null;
		
		public Ball bWallColl3 		= null;
		public MyVector wWallColl3	= null;
		
		//с углами может быть только одно столкновение в единицу времени
		public Ball bAngleColl		= null;	//шар в столкновении угол-шар
		public Angle aAngleColl		= null;	//угол в стлкновении угол-шар
		
		//с лузами может быть только одно столкновение в единицу времени
		public Ball bPocketColl		= null;	//шар в столкновении угол-шар
		public Pocket pPocketColl	= null;	//угол в стлкновении угол-шар
		
		
		public CollisionObject()
        {
            //TODO Подумать, может ли быть больше 3-х коллизий в момент времени, потестить утечки памяти (может лучше использовать массив для хранения столкновений)
        }

        /**
		 * добавить коллизию в список коллизий
		 * @param	b1
		 * @param	b2
		 * @param	dt
		 */
        public void process2BallsCollision(Ball b1, Ball b2, float dt)
		{
			if (dt == timeToCollision)
			{
				//if (ballsCollisionCount == 1) return; //TODO Для теста, Убрать
				
				ballsCollisionCount++;
				
				if (ballsCollisionCount > 3)
				{
					toLog("ERROR processCollision: collisionCount > 3");
				}
				else
				{
					if (ballsCollisionCount == 1)
					{
						b1coll1 = b1;
						b2coll1 = b2;
					}
					else if (ballsCollisionCount == 2)
                    {
                        b1coll2 = b1;
                        b2coll2 = b2;
                    }
                    else if (ballsCollisionCount == 3)
                    {
                        b1coll3 = b1;
                        b2coll3 = b2;
                    }
				}
			}

            else
			{
				clearFull();
				timeToCollision = dt;
				ballsCollisionCount = 1;
				b1coll1 = b1;
				b2coll1 = b2;
			}
		}
		
		public void processBallWithWallCollision(Ball b, MyVector w, float dt)
		{
			if (dt == timeToCollision)
			{
				//if (ballsCollisionCount == 1) return; //TODO Для теста, Убрать
				
				ballWallCollisionCount++;

				if (ballWallCollisionCount > 3)
				{
					toLog("ERROR processCollision: ballWallCollisionCount > 3");
				}
				else
				{
                    if(ballWallCollisionCount == 1)
                    {
                        bWallColl1 = b;
                        wWallColl1 = w;
                    }
                    else if (ballWallCollisionCount == 2)
                    {
                        bWallColl2 = b;
                        wWallColl2 = w;
                    }
                    else if (ballWallCollisionCount == 3)
                    {
                        bWallColl3 = b;
                        wWallColl3 = w;
                    }
				}
			}
            else
			{
				//TODO КПодумать над оптимизацией (чтоб все поля не обнулять при обновлении времени)
				clearFull();

				timeToCollision = dt;
				ballWallCollisionCount = 1;
				bWallColl1 = b;
				wWallColl1 = w;
			}
		}
		
		public void processBallWithAngleCollision(Ball b, Angle a, float dt)
		{
			if (dt != timeToCollision)
			{
				//TODO КПодумать над оптимизацией (чтоб все поля не обнулять при обновлении времени)
				clearFull();
				timeToCollision = dt;
			}
			bAngleColl = b;
            aAngleColl = a;
        }
		
		public void processBallWithPocketCollision(Ball b, Pocket p, float dt)
		{
			if (dt != timeToCollision)
			{
				//TODO КПодумать над оптимизацией (чтоб все поля не обнулять при обновлении времени)
				clearFull();
				timeToCollision = dt;
			}
			bPocketColl = b;
			pPocketColl = p;
		}
		
		/**
		 * Очистить список коллизий
		 */
		public void clear()
		{
			timeToCollision = float.MaxValue;
			ballsCollisionCount = 0;
			ballWallCollisionCount = 0;

			bAngleColl = null;
			aAngleColl = null;

			bPocketColl = null;
			pPocketColl = null;
		}
		
		/**
		 * Полностью очистить поля объекта
		 */
		public void clearFull()
		{
			clear();

			b1coll1 = null;
			b2coll1 = null;

			b1coll2 = null;
			b2coll2 = null;

			b1coll3 = null;
			b2coll3 = null;

			bWallColl1 = null;
			wWallColl1 = null;
			bWallColl2 = null;
			wWallColl2 = null;
			bWallColl3 = null;
			wWallColl3 = null;

			bAngleColl = null;
			aAngleColl = null;

			bPocketColl = null;
			pPocketColl = null;
		}
		
		public string getString()
		{
			var s = "";
			s += "timTo:" + timeToCollision;
			s += " BALLS:";
			if (b1coll1 != null) s += " b11:" + b1coll1.Number;
			if (b2coll1 != null) s += " b21:" + b2coll1.Number;
			if (b1coll2 != null) s += " b12:" + b1coll2.Number;
			if (b2coll2 != null) s += " b22:" + b2coll2.Number;
			if (b1coll3 != null) s += " b13:" + b1coll3.Number;
			if (b2coll3 != null) s += " b23:" + b2coll3.Number;
			s += " WALL:";
			if (bWallColl1 != null) s += " b1:" + bWallColl1.Number + " w1:" + wWallColl1.p0.x + ";" + wWallColl1.p0.y;
			if (bWallColl2 != null) s += " b2:" + bWallColl2.Number + " w2:" + wWallColl2.p0.x + ";" + wWallColl2.p0.y;
			if (bWallColl3 != null) s += " b3:" + bWallColl3.Number + " w3:" + wWallColl3.p0.x + ";" + wWallColl3.p0.y;

			s += " ANGLE:";
			if (bAngleColl != null) s += " b:" + bAngleColl.Number + " a:" + aAngleColl.x + ";" + aAngleColl.y;

			s += " POCKET:";
			if (bPocketColl != null) s += " b:" + bPocketColl.Number + " p:" + pPocketColl.x + ";" + pPocketColl.y;

			return s;
		}

		private void toLog(string s)
		{
			Debug.Log(s);
		}
    }
}
