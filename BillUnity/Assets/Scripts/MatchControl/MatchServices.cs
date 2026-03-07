using Kborod.BilliardCore;
using Kborod.DomainModel;
using System.Collections.Generic;

namespace Kborod.MatchManagement
{
    public class MatchServices : IMatchServices
    {
        public MatchBase Match { get; private set; }
        public MyInput MyInput { get; private set; }
        public IEngineForUI EngineForUI => Match.EngineForUI;

        public void Setup(MatchBase match, CuesModel cuesModel, List<string> canManagePlayerIds)
        {
            Dispose();

            Match = match;
            MyInput = new MyInput(match, cuesModel, canManagePlayerIds);
        }

        private void Dispose()
        {
            Match?.Dispose();
            MyInput?.Dispose();
        }
    }
}
