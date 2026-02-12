using Cysharp.Threading.Tasks;
using Kborod.Services.ServerCommunication.Token;
using Kborod.Services.UIScreenManager;
using Kborod.UI.Screens;
using Kborod.Utils;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class LoginWithEmailProcess
    {
        [Inject] private ScreensHelper _screensHelper;

        public async UniTask<Result<TokenData>> Run()
        {
            var screen = await _screensHelper.OpenScreen<LoginByEmailScreen>();
            return await screen.LoginAndGetToken();
        }
    }
}
