using Cysharp.Threading.Tasks;
using Kborod.AsyncProcesses;
using UnityEngine;
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
            await MainMenu();
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

            //await _container.Instantiate<PreloaderProcess>().Run();
            //await _container.Instantiate<AgreementProcess>().Run();
        }
    }
}
