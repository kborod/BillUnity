using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.DomainModel;
using Kborod.MatchManagement;
using Kborod.MatchManagement.Control;
using Kborod.Services.UIScreenManager;
using Kborod.Utils;
using System.Collections.Generic;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class StartPvpMatchProcess
    {
        [Inject] private ScreensHelper _screensHelper;
        [Inject] private DiContainer _diContainer;
        [Inject] private MatchServices _matchServices;
        [Inject] private AccountModel _accoundModel;
        [Inject] private CuesModel _cuesModel;


        public async UniTaskVoid Run(StartMatchData startData)
        {

            var match = new MatchPoolEight();
            match.Init(
                startData.MatchId, 
                startData.BallsPosition,
                new PoolEightPlayer(_accoundModel.GetProfile()),
                new PoolEightPlayer(startData.Opponent),
                startData.TurningPlayerId);

            _matchServices.Setup(match, _cuesModel, new List<string>() { _accoundModel.Id });

            await new OpenSceneProcess("TableScene").Run();

            await _screensHelper.ClearAll();

            var matchControl = _diContainer.Instantiate<MatchControlNetwork>();
            matchControl.Setup(startData);
        }
    }
}
