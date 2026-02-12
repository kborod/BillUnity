using Cysharp.Threading.Tasks;
using Kborod.Services.UIScreenManager;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class MatchProcess
    {
        [Inject] private ScreensHelper _screensHelper;
        public async UniTask Run()
        {
            await _screensHelper.ClearAll();
            await new OpenSceneProcess("TableScene").Run();
        }
    }
}
