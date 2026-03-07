using System;

namespace Kborod.DomainModel
{
    public abstract class CuesModel
    {
        public event Action CueChanged;
        public abstract int CurrentCueId { get; }
        public abstract float GetCuePower(int cueId);
    }
}
