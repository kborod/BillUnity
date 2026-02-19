using Kborod.Services.ServerCommunication;
using Kborod.Services.ServerCommunication.AsyncServerMessaging;
using Kborod.Services.ServerCommunication.AsyncServerMessaging.ReliableMessageDelivery;
using Kborod.Services.ServerCommunication.Token;
using Kborod.Services.ServerTime;
using UnityEngine;
using Zenject;

namespace Kborod.Services.DI
{
    public class ServerComminicationsInstaller : MonoInstaller
    {
        [SerializeField] private HttpTransportByUnityWebRequest _httpTransportByUnityWebRequestPrefab;

        public override void InstallBindings()
        {
            Container.Bind<HttpService>().FromNew().AsSingle().NonLazy();
            Container.Bind<IHttpTransport>().FromComponentInNewPrefab(_httpTransportByUnityWebRequestPrefab).AsSingle().NonLazy();
            
            Container.Bind<ITokenService>().To<TokenService>().FromNew().AsSingle().NonLazy();
            Container.Bind<TokenRefreshService>().FromNew().AsSingle().NonLazy();
            
            Container.Bind<IMessagingTransport>().To<GameHub>().FromNew().AsSingle().NonLazy();
            Container.Bind<IMessagingService>().To<ReliableMessageDeliveryService>().FromNew().AsSingle().NonLazy();
            Container.Bind<SessionHeartbeatServices>().FromNew().AsSingle().NonLazy();

            Container.Bind<TimeControl>().FromNew().AsSingle().NonLazy();
        }
    }
}
