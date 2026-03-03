using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.BilliardCore.Rules;
using Kborod.Loader;
using Zenject;

namespace Kborod.MatchManagement.Control
{
    public class MatchControlLocal
    {
        [Inject] private MatchServices _matchServices;
        [Inject] private AppProcessor _appProcessor;

        private MatchBase _match => _matchServices.Match;

        public long EndTurnTime => long.MaxValue;

        public void Init()
        {
            _matchServices.MyInput.AimInfoChanged += AimChanged;
            _matchServices.MyInput.ShotMade += ShotMade;
            _match.StateChanged += MatchStateChangedHandler;
            _match.ShotCompleted += MatchShotCompletedHandler;
            //ShotMade(
            //    new AimInfo
            //    {
            //        CueBall = 0,
            //        CueBallX = -116,
            //        CueBallY = -127,
            //        DirectionX = 0.94f,
            //        DirectionY = 0.31f,
            //        Power = 0.83f
            //    });
        }

        public void Dispose()
        {
            _matchServices.MyInput.AimInfoChanged -= AimChanged;
            _matchServices.MyInput.ShotMade -= ShotMade;
            _match.StateChanged -= MatchStateChangedHandler;
            _match.ShotCompleted -= MatchShotCompletedHandler;

        }

        private void MatchShotCompletedHandler(ShotResultByRules result)
        {
            _matchServices.Match.StartTurn(new StartTurnData
            {
                TurnEndTimestamp = long.MaxValue,
                TurningPlayerId = _match.TurningPlayer.Id,
                MatchId = _match.Id
            });
        }

        private void MatchStateChangedHandler(MatchState state)
        {
            if (state == MatchState.Over)
            {
                Dispose();
                _appProcessor.MainMenu().Forget();
            }
        }

        private void AimChanged(AimInfo info)
        {
            if (_matchServices.Match.State != MatchState.PrepeareTurn)
                return;
            _match.ChangeAimInfo(info);
        }

        private void ShotMade(AimInfo info)
        {
            if (_matchServices.Match.State != MatchState.PrepeareTurn)
                return;
            _match.MakeShot(info);
        }
    }
}
