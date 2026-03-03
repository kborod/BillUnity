using Kborod.BilliardCore;
using System;

namespace Kborod.MatchManagement
{
    public interface IInputProvider
    {
        public event Action<AimInfo> AimInfoChanged;
        public event Action<AimInfo> ShotMade;
    }
}
