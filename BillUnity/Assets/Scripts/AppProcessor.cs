using Cysharp.Threading.Tasks;
using Kborod.AsyncProcesses;
using Zenject;

namespace Kborod.Loader
{
    public class AppProcessor
    {
        [Inject]
        private DiContainer _container;


        public async UniTask StartApplication()
        {
            await InitApp();
            // Login();
            //await InitRealTimeMsgs();
            await MainMenu();
            //await TestWindow();
        }

        public async UniTask MainMenu()
        {
            await _container.Instantiate<MainMenuProcess>().Run();
        }

        public async UniTask Match()
        {
            await _container.Instantiate<MatchProcess>().Run();
        }

        private async UniTask InitApp()
        {
            UnityEngine.Application.runInBackground = true;

            await _container.Instantiate<InitServices>().Run();
        }

        private async UniTask Login()
        {
            await _container.Instantiate<AuthProcess>().Run();
        }

        private async UniTask InitRealTimeMsgs()
        {
            await _container.Instantiate<InitRealTimeMessagingProcess>().Run();
        }

        private async UniTask TestWindow()
        {
            await _container.Instantiate<TestWindowProcess>().Run();
        }
    }
}
