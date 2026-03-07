using Cysharp.Threading.Tasks;
using Kborod.AsyncProcesses;
using Kborod.BilliardCore.Enums;
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

            //await TestWindow();
            //return;
            //await TestLoginByTags();
            //await Login();
            //await InitRealTimeMsgs();
            await MainMenu();
        }

        public async UniTask MainMenu()
        {
            await _container.Instantiate<MainMenuProcess>().Run();
        }

        public async UniTask TrainingMatch()
        {
            await _container.Instantiate<InitTrainingMatchProcess>().Run();
        }

        public async UniTask PvpMatch(GameType gameType, BetType betType)
        {
            var processResult = await _container.Instantiate<InitPvpMatchProcess>().Run(gameType, betType);
            if (processResult.IsSuccess == false)
                MainMenu().Forget();
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

        private async UniTask TestLoginByTags()
        {
            var result = await _container.Instantiate<TestLoginByMultiplayerTagsProcess>().LoginAndGetToken();
            if (result.IsSuccess == false)
                throw new System.Exception("Login error");
        }

        private async UniTask TestWindow()
        {
            await _container.Instantiate<TestWindowProcess>().Run();
        }
    }
}
