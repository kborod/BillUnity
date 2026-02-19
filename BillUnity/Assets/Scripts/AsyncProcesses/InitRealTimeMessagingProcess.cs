using Cysharp.Threading.Tasks;
using Kborod.Services.ServerCommunication.AsyncServerMessaging;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class InitRealTimeMessagingProcess
    {
        [Inject] private IMessagingService _asyncMessagingService;

        public async UniTask Run()
        {
            await _asyncMessagingService.Connect();
        }
    }
}
