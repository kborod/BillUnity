using Kborod.Services.ServerCommunication;
using Kborod.Services.ServerCommunication.Sockets;
using Kborod.Services.ServerCommunication.Sockets.ReliableMessageDelivery;
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
            
            Container.Bind<SocketService>().FromNew().AsSingle().NonLazy();
            Container.Bind<ReliableMessageDeliveryService>().FromNew().AsSingle().NonLazy();
            
            Container.Bind<TimeControl>().FromNew().AsSingle().NonLazy();
        }
    }
}
