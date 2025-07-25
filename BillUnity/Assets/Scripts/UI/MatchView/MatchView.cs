using Kborod.MatchManagement;
using Kborod.Services.UIScreenManager;
using System;
using Zenject;

namespace Kborod.UI.Screens
{
    public class MatchView : UIScreenBase
    {
        [Inject] private MatchBase _match;

        private void Awake()
        {
            var p1 = new PoolEightPlayer("1", "Player1");
            var p2 = new PoolEightPlayer("2", "Player2");

            if (_match is not MatchPoolEight)
            {
                throw new Exception("NotImplemented");
            }
            (_match as MatchPoolEight).StartNew(p1, p2);
        }
    }
}
