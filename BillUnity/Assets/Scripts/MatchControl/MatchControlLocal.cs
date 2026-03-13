using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.BilliardCore.Enums;
using Kborod.BilliardCore.Rules;
using Kborod.Loader;
using Kborod.Services.UIScreenManager;
using Kborod.UI.Screens;
using Zenject;

namespace Kborod.MatchManagement.Control
{
    public class MatchControlLocal
    {
        [Inject] private MatchServices _matchServices;
        [Inject] private AppProcessor _appProcessor;
        [Inject] private ScreensHelper _screensHelper;

        private MatchBase _match => _matchServices.Match; 
        private AimPlayer _aimPlayer = new AimPlayer();

        public long EndTurnTime => long.MaxValue;

        public void Init()
        {
            _matchServices.MyInput.AimInfoChanged += AimChanged;
            _matchServices.MyInput.ShotMade += ShotMade;
            _matchServices.MyInput.WantLeave += Leave;
            _match.ShotCompleted += MatchShotCompletedHandler;

            StartTurn();


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
            _matchServices.MyInput.WantLeave -= Leave;
            _match.ShotCompleted -= MatchShotCompletedHandler;
        }

        private void StartTurn()
        {
            _matchServices.Match.StartTurn(new StartTurnData
            {
                TurnEndTimestamp = long.MaxValue,
                TurningPlayerId = _match.TurningPlayer.Id,
                MatchId = _match.Id
            });
        }

        
        private async void MatchShotCompletedHandler(RulesShotResult result)
        {
            if (string.IsNullOrEmpty(result.WinUserIdOrNull) == false)
            {
                MatchOver(result.WinUserIdOrNull);
            }
            else
            {
                StartTurn();
            }
        }

        private void AimChanged(AimInfo info)
        {
            if (_matchServices.Match.State != MatchState.PrepeareTurn)
                return;
            _match.ChangeAimInfo(info);
        }

        private async void ShotMade(AimInfo info)
        {
            if (_matchServices.Match.State != MatchState.PrepeareTurn)
                return;

            await _aimPlayer.Play(info, _match, 1f, true);
            _match.MakeShot(info, GetCuePower());
        }

        private void Leave()
        {
            Dispose();
            _appProcessor.MainMenu();
        }

        private async void MatchOver(string winnerId)
        {
            await UniTask.Delay(1000);

            _appProcessor.MainMenu();

            //var matchResultScreen = await _screensHelper.OpenScreen<MatchResultScreen>();

            //var loserId = winnerId == _match.Player1.Id ? _match.Player2.Id : _match.Player1.Id;
            //var winnerScore = _match.GetScore(winnerId);
            //var loserScore = _match.GetScore(loserId);

            //matchResultScreen.Setup(_match.GameType, BetType.None, winnerId, winnerScore, loserScore,
            //    new SharedDto.UserProfile { Id = _match.Player2.Id, Name = _match.Player2.Name },
            //    () => _appProcessor.MainMenu(), () => _appProcessor.StartTrainingMatch());

            Dispose();
        }

        private int GetCuePower() => 300;
    }
}
