namespace Kborod.DomainModel
{
    public class CuesModelLocal : CuesModel
    {
        public override int CurrentCueId => 1;
        public override float GetCuePower(int cueId) => 300;
    }
}
