using Newtonsoft.Json;

namespace Kborod.BilliardCore
{
    public struct AimInfo
    {
        public int? CueBall;
        public float DirectionX;
        public float DirectionY;
        public int? Pocket;

        public float? CueBallX;
        public float? CueBallY;

        public float SpinX;
        public float SpinY;

        public float Power;

        /// <summary> только для фронта </summary>
        [JsonIgnore]
        public bool IsBallMovingNow;
    }
}