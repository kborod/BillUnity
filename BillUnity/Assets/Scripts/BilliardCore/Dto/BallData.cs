namespace Kborod.BilliardCore
{
    public class BallData
	{
		public int Number;
		public bool IsRemoved;
		public float X;
		public float Y;

        public void SetPosition(float x, float y)
        {
            X = x; Y = y;
        }

        public override string ToString()
        {
            return $"Num:{Number}_IsRemoved:{IsRemoved}_X:{X}_Y:{Y}";
        }
    }
}
