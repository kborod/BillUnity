using Cysharp.Threading.Tasks;
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

            var match = new MatchPoolEight();
            match.Init(
                "1", 
                1,
                new PoolEightPlayer("1", "Player1"),
                new PoolEightPlayer("2", "Player2"), 
                "1", 
                long.MaxValue);

            _matchServices.Setup(match, new List<string>() { "1", "2" });

            await UniTask.Delay(TimeSpan.FromSeconds(1.5f));

            var matchControl = _diContainer.Instantiate<MatchControlLocal>();
            matchControl.Init();

            await new OpenSceneProcess("TableScene").Run();
        }
    }
}
