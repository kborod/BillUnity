using Cysharp.Threading.Tasks;
using Kborod.Services.UIScreenManager;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class InitServices
    {
        private UIScreensLoader _screensLoader;

        [Inject]
        public void Construct(UIScreensLoader screensLoader)
        {
            _screensLoader = screensLoader;
        }

        public async UniTask Run()
        {
            await _screensLoader.Initialize();
        }
    }
}
