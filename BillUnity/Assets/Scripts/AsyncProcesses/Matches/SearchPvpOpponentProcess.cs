using Cysharp.Threading.Tasks;
using Kborod.BilliardCore.Enums;
using Kborod.Services.ServerCommunication.AsyncServerMessaging;
using Kborod.Services.UIScreenManager;
using Kborod.SharedDto.AsyncServerMessaging.Messages;
using Kborod.UI.Screens;
using Kborod.Utils;
using UnityEngine;
using Zenject;

namespace Kborod.MatchManagement.Control
{
    public class SearchPvpOpponentProcess
    {
        [Inject] private IMessagingService _messagingService;
        [Inject] private ScreensHelper _screensHelper;

        private UniTaskCompletionSource<Result<MatchStartedResponseDto>> _matchStartedTcs;

        private SearchOpponentScreen _searchScreen;

        public async UniTask<Result<MatchStartedResponseDto>> Run(GameType gameType, BetType betType)
        {
            Initialize();

            _searchScreen = await _screensHelper.OpenScreen<SearchOpponentScreen>();
            _searchScreen.Setup(gameType, betType, CancelSearch);

            _messagingService.SendRequest(new SearchMatchDto(gameType, betType));
            _matchStartedTcs = new UniTaskCompletionSource<Result<MatchStartedResponseDto>>();
            return await _matchStartedTcs.Task;
        }

        private void CancelSearch()
        {
            _messagingService.SendRequest(new CancelSearchMatchDto());
        }

        private void MatchStartReceivedHandler(MatchStartedResponseDto response)
        {
            Dispose();
            _matchStartedTcs.TrySetResult(Result<MatchStartedResponseDto>.Ok(response));
        }

        private void AddedToQueueReceivedHandler(AddedToQueueResponseDto response)
        {
            Debug.Log("Waiting...");
        }

        private void SearchCancelledReceivedHandler(SearchCancelledResponseDto response)
        {
            Dispose();
            _matchStartedTcs.TrySetResult(Result<MatchStartedResponseDto>.Fail("Search cancelled"));
        }


        private void Initialize()
        {
            _messagingService.Subscribe<MatchStartedResponseDto>(MatchStartReceivedHandler);
            _messagingService.Subscribe<AddedToQueueResponseDto>(AddedToQueueReceivedHandler);
            _messagingService.Subscribe<SearchCancelledResponseDto>(SearchCancelledReceivedHandler);
        }

        private void Dispose()
        {
            _messagingService.Unsubscribe<MatchStartedResponseDto>(MatchStartReceivedHandler);
            _messagingService.Unsubscribe<AddedToQueueResponseDto>(AddedToQueueReceivedHandler);
            _messagingService.Unsubscribe<SearchCancelledResponseDto>(SearchCancelledReceivedHandler);

            //_searchScreen?.Release();
        }
    }
}
