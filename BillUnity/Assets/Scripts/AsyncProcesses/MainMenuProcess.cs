using Cysharp.Threading.Tasks;
using Kborod.Services.UIScreenManager;
using Kborod.UI.Screens;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class MainMenuProcess
    {
        [Inject] private ScreensHelper _screensHelper;

        public async UniTask Run()
        {
            await new OpenSceneProcess("MainMenu").Run();
            await _screensHelper.ScreensManager.Open<MainMenuScreen>();
        }
    }
}
