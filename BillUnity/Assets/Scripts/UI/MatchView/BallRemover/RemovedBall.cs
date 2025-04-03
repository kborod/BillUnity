using Kborod.BilliardCore;

namespace Kborod.UI.Screens
{
    public class RemovedBall
    {
        //Забитый шар
        public Ball ball;

        //Луза, в которую забит шар
        public Pocket pocket;
		
		public RemoveState state;
		
		public float currPathDis = -1;
		public float maxPathDis;
		
		public int removeNumber;


		public RemovedBall(Ball b) 
		{
            this.ball = b;
            this.pocket = b.pocketRemoveTo;
        }

        /// <summary>
		/// Изменить вектор перемещения щара в зависимости от состояния
		/// </summary>
        public void ChangeMoveVector(RemoveState state)
		{
			var length = ball.v.len;
			
			if (state == RemoveState.STATE_TO_POCKET_POINT)
			{
				if (length < 15) length = 15;
				//else if (length > 30) length = 30;
				
				ball.v.p1.x = pocket.pRemove.x;
				ball.v.p1.y = pocket.pRemove.y;
				ball.v.updateComponentsFromPoints();
				ball.v.makeVector();
				ball.v.p1.x = ball.v.p0.x + ball.v.dx* length;
				ball.v.p1.y = ball.v.p0.y + ball.v.dy* length;
				ball.v.updateComponentsFromPoints();
				ball.v.makeVector();
			}
			else if (state == RemoveState.STATE_FROM_POCKET)
			{
				//length = 2;
				if (length > 20) length = 20;
				ball.v.p1.x = ball.v.p0.x + pocket.vRemove.dx* pocket.r * 3;
				ball.v.p1.y = ball.v.p0.y + pocket.vRemove.dy* pocket.r * 3;
				ball.v.updateComponentsFromPoints();
				ball.v.makeVector();
				ball.v.p1.x = ball.v.p0.x + ball.v.dx* length;
				ball.v.p1.y = ball.v.p0.y + ball.v.dy* length;
				ball.v.updateComponentsFromPoints();
				ball.v.makeVector();
				ball.vVertSpin.vx = ball.v.vx;
				ball.vVertSpin.vy = ball.v.vy;
				ball.vVertSpin.updatePointsFromComponents();
			}
		}

        /// <summary>
        /// Изменить состояние анимации забития шара
        /// </summary>
        public void SetState(RemoveState state)
		{
			this.state = state;
		}
    }
}
