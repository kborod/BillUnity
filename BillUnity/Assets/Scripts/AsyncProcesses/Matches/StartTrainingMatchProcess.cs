using Cysharp.Threading.Tasks;
using Kborod.DomainModel;
using Kborod.MatchManagement;
using Kborod.MatchManagement.Control;
using Kborod.Services.UIScreenManager;
using Kborod.SharedDto;
using System.Collections.Generic;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class StartTrainingMatchProcess
    {
        [Inject] private CuesModel _cuesModel;
        [Inject] private ScreensHelper _screensHelper;
        [Inject] private MatchServices _matchServices;
        [Inject] private DiContainer _diContainer;
        [Inject] private AccountModel _accountModel;
        
        public async UniTaskVoid Run()
        {
            //await _screensHelper.ClearAll();

            var matchId = "TestMatch";
            var myProfile = _accountModel.GetProfile();
            var opponentProfile = new UserProfile { Id = "player2Id", Name = "Player2", Avatar = 1 };
            var ballsPosition = 1;


            var match = new MatchPoolEight();
            match.Init(
                matchId,
                ballsPosition,
                new PoolEightPlayer(myProfile),
                new PoolEightPlayer(opponentProfile),
                myProfile.Id);

            _matchServices.Setup(match, _cuesModel, new List<string>() { myProfile.Id, opponentProfile.Id });

            var matchControl = _diContainer.Instantiate<MatchControlLocal>();
            matchControl.Init();

            await new OpenSceneProcess("TableScene").Run();
        }
    }
}
