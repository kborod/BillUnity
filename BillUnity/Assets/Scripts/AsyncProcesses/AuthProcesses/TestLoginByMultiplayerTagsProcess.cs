using Cysharp.Threading.Tasks;
using Kborod.DomainModel.Auth;
using Kborod.Services.ServerCommunication.Token;
using Kborod.Utils;
using System;
using System.Linq;
using Unity.Multiplayer.Playmode;
using Zenject;

namespace Kborod.AsyncProcesses
{
    public class TestLoginByMultiplayerTagsProcess
    {
        [Inject] private EmailPasswordAuthProvider _provider;
        [Inject] private ITokenService _tokenService;

        private UniTaskCompletionSource<Result> _tcs;

        public UniTask<Result> LoginAndGetToken()
        {
            _tcs = new UniTaskCompletionSource<Result>();
            Login();
            return _tcs.Task;
        }

        private void Login()
        {
            if (CurrentPlayer.ReadOnlyTags().Contains("Player1"))
            {
                _provider.Login("test@test", "123123", LoginSuccessHandler, LoginErrorHandler);
            }
            else if (CurrentPlayer.ReadOnlyTags().Contains("Player2"))
            {
                _provider.Login("test2@test", "123123", LoginSuccessHandler, LoginErrorHandler);
            }
        }

        private void LoginSuccessHandler(TokenData data)
        {
            _tokenService.Set(data);
            _tcs.TrySetResult(Result.Ok());
        }

        private void LoginErrorHandler(string error)
        {

            _tcs.TrySetResult(Result.Fail(error));
        }
    }
}
