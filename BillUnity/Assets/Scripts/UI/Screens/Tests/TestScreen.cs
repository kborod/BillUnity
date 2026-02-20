using Cysharp.Threading.Tasks;
using Kborod.Services.ServerCommunication.AsyncServerMessaging;
using Kborod.Services.ServerCommunication.Token;
using Kborod.Services.UIScreenManager;
using Kborod.SharedDto.AsyncServerMessaging.Messages;
using Kborod.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens
{
    [UIScreen("UI/Screens/TestScreen.prefab", true)]
    public class TestScreen : UIScreenBase
    {
        [SerializeField] private Button _btnStartSession;
        [SerializeField] private Button _btnHubDisconnect;
        [SerializeField] private Button _btnSend;

        [Inject] private IMessagingService _messagingService;

        private UniTaskCompletionSource<Result<TokenData>> _tcs;

        private async void Start()
        {
            _btnStartSession.onClick.AddListener(StartSessionClickHandler);
            _btnHubDisconnect.onClick.AddListener(HubDisconnectClickHandler);
            _btnSend.onClick.AddListener(SendClickHandler);
        }

        private void StartSessionClickHandler()
        {
            _messagingService.Connect();
        }

        private void HubDisconnectClickHandler()
        {
            _messagingService.Disconnect();
        }

        public void SendClickHandler()
        {
            _messagingService.SendRequest(new TestRequestDto("???Test???"));
        }
    }
}
