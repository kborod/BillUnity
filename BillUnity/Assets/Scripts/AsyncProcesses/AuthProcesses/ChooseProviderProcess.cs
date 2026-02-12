using Cysharp.Threading.Tasks;
using Kborod.DomainModel.Auth;
using Kborod.Services.UIScreenManager;
using Kborod.UI.Screens;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class ChooseProviderProcess
    {
        private ScreensHelper _screensHelper;

        [Inject]
        public void Construct(ScreensHelper screensHelper)
        {
            _screensHelper = screensHelper;
        }

        public async UniTask<ProviderType> Run()
        {
            var screen = await _screensHelper.OpenScreen<ChooseAuthProviderScreen>();
            return await screen.GetSelectedProviderAsync();
        }
    }
}
