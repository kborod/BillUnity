using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.BilliardCore.Enums;
using Kborod.DomainModel;
using Kborod.MatchManagement;
using Kborod.MatchManagement.Control;
using Kborod.Services.UIScreenManager;
using Kborod.Utils;
using System.Collections.Generic;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class InitPvpMatchProcess
    {
        [Inject] private ScreensHelper _screensHelper;
        [Inject] private DiContainer _diContainer;
        [Inject] private MatchServices _matchServices;
        [Inject] private AccountModel _accoundModel;


        public async UniTask<Result> Run(GameType gameType, BetType betType)
        {
            await _screensHelper.ClearAll();

            var searchOpponentResult = await _diContainer.Instantiate<SearchPvpOpponentProcess>().Run(gameType, betType);

            if (searchOpponentResult.IsSuccess == false)
                return Result.Fail(searchOpponentResult.Error);

            StartMatchData startData = searchOpponentResult.Value.StartMatchData;

            var match = new MatchPoolEight();
            match.Init(
                startData.MatchId, 
                startData.BallsPosition,
                new PoolEightPlayer(_accoundModel.Id, _accoundModel.Name),
                new PoolEightPlayer(startData.Opponent.Id, startData.Opponent.Username),
                startData.TurningPlayerId);

            _matchServices.Setup(match, new List<string>() { _accoundModel.Id });

            

            await new OpenSceneProcess("TableScene").Run(); 
            
            var matchControl = _diContainer.Instantiate<MatchControlNetwork>();
            matchControl.Setup(startData);

            return Result.Ok();
        }
    }
}
