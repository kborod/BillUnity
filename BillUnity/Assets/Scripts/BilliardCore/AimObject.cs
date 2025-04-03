namespace Kborod.BilliardCore
{
    public class AimObject
    {
		/// <summary>
		/// Координата X прицельного шара (начальная точка для отрисовки линии удара)s
		/// </summary>
		public float aimBallX0;
		/**
		 * Координата Y прицельного шара (начальная точка для отрисовки линии удара)
		 */
		public float aimBallY0;
        /**
		 * Координата X положения шара в момент соприкосновения (конечная точка для отрисовки линии удара)
		 */
        public float aimBallX;
		/**
		 * Координата Y положения шара в момент соприкосновения (конечная точка для отрисовки линии удара)
		 */
		public float aimBallY;
		/**
		 * Номер шара, с которым сталкивается сначала (если стена, то -1)
		 */
		public float firstCollBallNum = -1;
		/**
		 * Координата Х прицельного шара
		 */
		public float collBallX0;
		/**
		 * Координата Y прицельного шара
		 */
		public float collBallY0;
		/**
		 * Компонента X вектора отскока битка
		 */
		public float aimBallBounceVx;
		/**
		 * Компонента Y вектора отскока битка
		 */
		public float aimBallBounceVy;
		/**
		 * Компонента X вектора отскока прицельного шара
		 */
		public float collBallBounceVx;
		/**
		 * Компонента Y вектора отскока прицельного шара
		 */
		public float collBallBounceVy;
    }
}
