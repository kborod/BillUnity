using Kborod.MatchManagement;
using Kborod.Services.UIScreenManager;
using System;
using System.Text.RegularExpressions;
using Zenject;

namespace Kborod.UI.Screens
{
    public class MatchView : UIScreenBase
    {
        [Inject] private MatchServices _matchServices;
        private MatchBase _match => _matchServices.Match;

        private void Start()
        {
            if (_match is not MatchPoolEight)
            {
                throw new Exception("NotImplemented");
            }
        }
    }
}
