using Cysharp.Threading.Tasks;
using Kborod.AsyncProcesses;
using Kborod.BilliardCore;
using Kborod.BilliardCore.Enums;
using Kborod.MatchManagement.Control;
using Kborod.SharedDto;
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

            //TestWindow();
            //return;
            
            await TestLoginByTags();
            //await Login();
            
            await InitRealTimeMsgs();
            
            MainMenu();
        }

        public void MainMenu()
        {
            _container.Instantiate<MainMenuProcess>().Run().Forget();
        }

        public void StartTrainingMatch()
        {
            _container.Instantiate<StartTrainingMatchProcess>().Run().Forget();
        }

        public async UniTaskVoid SearchAndStartPvpMatch(GameType gameType, BetType betType)
        {
            var searchOpponentResult = await _container.Instantiate<SearchPvpOpponentProcess>().Run(gameType, betType);

            if (searchOpponentResult.IsSuccess == false)
                MainMenu();
            else
                StartPvpMatch(searchOpponentResult.Value.StartMatchData);
        }

        public void StartPvpMatch(StartMatchData startData)
        {
            _container.Instantiate<StartPvpMatchProcess>().Run(startData).Forget();
        }

        public void PvpMatchOver(MatchOverData matchOverData, UserProfile oppProfile)
        {
            _container.Instantiate<PvpMatchOverProcess>().Run(matchOverData, oppProfile).Forget();
        }

        private void TestWindow()
        {
            _container.Instantiate<TestWindowProcess>().Run().Forget();
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
    }
}
