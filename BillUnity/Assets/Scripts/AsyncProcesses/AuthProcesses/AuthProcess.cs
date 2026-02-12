using Cysharp.Threading.Tasks;
using Kborod.DomainModel.Auth;
using Kborod.Services.ServerCommunication.Token;
using Kborod.Utils;
using System;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class AuthProcess
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private ITokenService _tokenService;

        public async UniTask Run()
        {
            var isLoadedFromCache = await _tokenService.TryLoadFromCache();
            if (isLoadedFromCache == true)
                return;

            while (true)
            {
                var selectedProviderType = await ChooseProvider();

                Result<TokenData> loginResult;

                if (selectedProviderType == ProviderType.EmailPassword)
                    loginResult = await LoginWithEmail();
                else
                    throw new NotImplementedException();

                if (loginResult.IsSuccess)
                {
                    _tokenService.Set(loginResult.Value);
                    break;
                }
            }
        }

        private async UniTask<ProviderType> ChooseProvider()
        {
            return await _diContainer.Instantiate<ChooseProviderProcess>().Run();
        }

        private async UniTask<Result<TokenData>> LoginWithEmail()
        {
            return await _diContainer.Instantiate<LoginWithEmailProcess>().Run();
        }
    }
}
