using Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages;
using System;
using Zenject;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.ReliableMessageDelivery
{
    public class SessionHeartbeatServices : IInitializable, IDisposable
    {
        [Inject] private IMessagingService _messagingService;

        public void Initialize()
        {
            _messagingService.Subscribe<AreYouAliveResponseDto>(AreYouAliveResponseReceived);
        }

        public void Dispose()
        {
            _messagingService?.Unsubscribe<AreYouAliveResponseDto>(AreYouAliveResponseReceived);
        }

        private void AreYouAliveResponseReceived(AreYouAliveResponseDto response)
        {
            _messagingService.SendRequest(new IamAliveDto());
        }
    }
}