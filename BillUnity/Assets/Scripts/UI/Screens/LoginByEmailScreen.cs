using Cysharp.Threading.Tasks;
using Kborod.DomainModel.Auth;
using Kborod.Services.ServerCommunication.Token;
using Kborod.Services.UIScreenManager;
using Kborod.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens
{
    [UIScreen("UI/Screens/LoginByEmailScreen.prefab", true)]
    public class LoginByEmailScreen : UIScreenBase
    {
        [SerializeField] private TMP_InputField _email;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private Button _loginBtn;
        [SerializeField] private Button _backBtn;

        [Inject] private EmailPasswordAuthProvider _provider;

        private UniTaskCompletionSource<Result<TokenData>> _tcs;

        private void Start()
        {
            _loginBtn.onClick.AddListener(LoginClickHandler);
            _backBtn.onClick.AddListener(BackClickHandler);
        }

        public UniTask<Result<TokenData>> LoginAndGetToken()
        {
            _tcs = new UniTaskCompletionSource<Result<TokenData>>();
            return _tcs.Task;
        }

        private void LoginClickHandler()
        {
            _provider.Login(_email.text, _password.text, LoginSuccessHandler, LoginErrorHandler);
        }

        private void BackClickHandler()
        {
            _tcs.TrySetResult(Result<TokenData>.Fail("cancel clicked"));
        }

        private void LoginSuccessHandler(TokenData token)
        {
            _tcs.TrySetResult(Result<TokenData>.Ok(token)); ;
        }

        private void LoginErrorHandler(string error)
        {
            Debug.Log($"Errors:{error}");
        }
    }
}
