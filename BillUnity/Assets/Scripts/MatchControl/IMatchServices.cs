using Kborod.BilliardCore;
using Kborod.UI.Screens.Table;

namespace Kborod.MatchManagement
{
    public interface IMatchServices
    {
        MatchBase Match { get; }
        MyInput MyInput { get; }
        IEngineForUI EngineForUI { get; }
    }
}