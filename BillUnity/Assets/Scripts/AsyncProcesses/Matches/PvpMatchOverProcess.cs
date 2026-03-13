using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.Loader;
using Kborod.Services.ServerCommunication.AsyncServerMessaging;
using Kborod.Services.UIScreenManager;
using Kborod.SharedDto;
using Kborod.SharedDto.AsyncServerMessaging.Messages;
using Kborod.UI.Screens;
using Zenject;

namespace Kborod.MatchManagement.Control
{
    public class PvpMatchOverProcess
    {
        [Inject] private IMessagingService _messagingService;
        [Inject] private ScreensHelper _screensHelper;
        [Inject] private AppProcessor _appProcessor;

        private string _matchId;

        private MatchResultScreen _matchResultScreen;

        private bool _isOppAfk = false;
        private bool _isOppReady = false;
        private bool _isIamReady = false;

        private bool _waitingToMenuConfirm = false;

        public async UniTaskVoid Run(MatchOverData data, UserProfile oppProfile)
        {
            Initialize();

            _matchId = data.MatchId;

            _matchResultScreen = await _screensHelper.OpenScreen<MatchResultScreen>();
            _matchResultScreen.Setup(data.GameType, data.BetType, data.WinPlayerIdOrNull, data.WinnerScore, 
                data.LoserScore, oppProfile, ToMenuHandler, RematchHandler);

            _isOppAfk = !data.RematchAvailable;
            RefreshResultScreen();
        }

        private void OppCancelRematchReceived(OppCancelRematchResponseDto dto)
        {
            if (_waitingToMenuConfirm)
            {
                ToMenu();
            }
            else
            {
                _isOppAfk = true;
                _isIamReady = false;
                _isOppReady = false;
                RefreshResultScreen();
            }
        }

        private void OppReadyRematchReceived(OppReadyRematchResponseDto dto)
        {
            _isOppReady = true;
            RefreshResultScreen();
        }

        private void CancelRematchConfirmReceived(CancelRematchConfirmResponseDto dto)
        {
            ToMenu();
        }

        private void MatchStartedReceived(MatchStartedResponseDto dto)
        {
            Dispose();
            _appProcessor.StartPvpMatch(dto.StartMatchData);
        }

        private void ToMenuHandler()
        {
            if (_isOppAfk)
            {
                ToMenu();
            }
            else
            {
                _messagingService.SendRequest(new CancelRematchDto(_matchId));
                _waitingToMenuConfirm = true;
            }
        }

        private void RematchHandler()
        {
            _messagingService.SendRequest(new ReadyRematchDto(_matchId));
            _isIamReady = true;
            RefreshResultScreen();
        }

        private void ToMenu()
        {
            Dispose();
            _appProcessor.MainMenu();
        }

        private void RefreshResultScreen()
        {
            _matchResultScreen.RefreshUI(_isOppAfk, _isOppReady, _isIamReady);
        }


        private void Initialize()
        {
            _messagingService.Subscribe<OppCancelRematchResponseDto>(OppCancelRematchReceived);
            _messagingService.Subscribe<OppReadyRematchResponseDto>(OppReadyRematchReceived);
            _messagingService.Subscribe<CancelRematchConfirmResponseDto>(CancelRematchConfirmReceived);
            _messagingService.Subscribe<MatchStartedResponseDto>(MatchStartedReceived);
        }

        private void Dispose()
        {
            _matchResultScreen?.Release();
            _messagingService.Unsubscribe<OppCancelRematchResponseDto>(OppCancelRematchReceived);
            _messagingService.Unsubscribe<OppReadyRematchResponseDto>(OppReadyRematchReceived);
            _messagingService.Unsubscribe<CancelRematchConfirmResponseDto>(CancelRematchConfirmReceived);
            _messagingService.Unsubscribe<MatchStartedResponseDto>(MatchStartedReceived);
        }
    }
}
