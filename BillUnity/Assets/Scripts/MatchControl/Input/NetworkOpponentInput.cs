using Kborod.BilliardCore;
using Kborod.Services.ServerCommunication.AsyncServerMessaging;
using System;
using Zenject;

namespace Kborod.MatchManagement
{
    public class NetworkOpponentInput : IInputProvider
    {
        public event Action<AimInfo> AimInfoChanged;
        public event Action<AimInfo> ShotMade;

        [Inject] private IMessagingService _messagingService;

        public void Initialize()
        {
            //_messagingService.Subscribe<AimIn>(OnOpponentAimInfoReceived);
            //_messagingService.Subscribe<OpponentMakeShotMessage>(OnOpponentMakeShotReceived);
        }
    }
}
