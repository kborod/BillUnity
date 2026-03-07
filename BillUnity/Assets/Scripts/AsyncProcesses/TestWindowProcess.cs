using Cysharp.Threading.Tasks;
using Kborod.Services.UIScreenManager;
using Kborod.UI.Screens;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class TestWindowProcess
    {
        [Inject] private ScreensHelper _screensHelper;

        public async UniTask Run()
        {
            //await _screensHelper.ScreensManager.Open<TestScreen>();
            await _screensHelper.ScreensManager.Open<TestShotScreen>();
        }
    }
}
