using Kborod.MatchManagement;
using System.Collections.Generic;

namespace Kborod.BilliardCore
{
    public class ShotResult
    {
        /// <summary>
        /// Номер шара, с которым произошло первое касание
        /// </summary>
        public int? firstCollisionBallNum = null;

        /// <summary>
        /// Список забитых шаров в порядке забивания
        /// </summary>
        public List<int> pocketedBalls = new List<int>();

        /// <summary>
        /// Список луз, в которые забиты шары, параллельно pocketedBalls
        /// </summary>
        public List<int> pocketedBallsPockets = new List<int>();

        /// <summary>
        /// Количество столкновений между шарами
        /// </summary>
        public int ballsCollisionCount = 0;

        /// <summary>
        /// Всего количество столкновений с бортами
        /// </summary>
        public int wallsCollisionCount = 0;
        
        /// <summary>
        /// Количество столкновений с бортами после первого соударения с шаром
        /// </summary>
        public int wallsCollisionAfterBallsCollisionCount = 0;

        /// <summary>
        /// Продолжительность удара
        /// </summary>
        public float shotDuration;
		
		public string toString()
		{
			return "ShotCalcResult:duration:" + shotDuration + ";firstCollisionBallNum:" + firstCollisionBallNum + ";wallsCollisionCount:" + wallsCollisionCount 
				+ ";pocketedBalls:" + pocketedBalls + ";pocketedBallsPockets:" + pocketedBallsPockets;
		}
}
}
