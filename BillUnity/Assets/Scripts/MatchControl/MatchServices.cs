using Kborod.BilliardCore;
using System.Collections.Generic;

namespace Kborod.MatchManagement
{
    public class MatchServices : IMatchServices
    {
        public MatchBase Match { get; private set; }
        public MyInput MyInput { get; private set; }
        public IEngineForUI EngineForUI => Match.EngineForUI;

        public void Setup(MatchBase match, List<string> canManagePlayerIds)
        {
            Dispose();

            Match = match;
            MyInput = new MyInput(match, canManagePlayerIds);
        }

        private void Dispose()
        {
            Match?.Dispose();
            MyInput?.Dispose();
        }
    }
}
