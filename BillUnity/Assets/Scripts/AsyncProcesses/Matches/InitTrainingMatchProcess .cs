using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.MatchManagement;
using Kborod.MatchManagement.Control;
using Kborod.Services.UIScreenManager;
using System;
using System.Collections.Generic;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class InitTrainingMatchProcess
    {
        [Inject] private ScreensHelper _screensHelper;
        [Inject] private MatchServices _matchServices;
        [Inject] private DiContainer _diContainer;
        
        public async UniTask Run()
        {
            await _screensHelper.ClearAll();

            var matchId = "TestMatch";
            var player1Id = "Player1Id";
            var player2Id = "Player2Id";
            var ballsPosition = 1;


            var match = new MatchPoolEight();
            match.Init(
                matchId,
                ballsPosition,
                new PoolEightPlayer(player1Id, "Player1"),
                new PoolEightPlayer(player2Id, "Player2"),
                player1Id);

            _matchServices.Setup(match, new List<string>() { player1Id, player2Id });

            var matchControl = _diContainer.Instantiate<MatchControlLocal>();
            matchControl.Init();

            await new OpenSceneProcess("TableScene").Run();
        }
    }
}
