using Kborod.Services.ServerCommunication;
using Kborod.Services.ServerCommunication.Token;
using System;
using Zenject;

namespace Kborod.DomainModel.Auth
{
    public class EmailPasswordAuthProvider
    {
        [Inject] private HttpService _httpService;

        public void Login(string email, string password, Action<TokenData> successCallback, Action<string> errorCallback)
        {
            var message = new AuthorizationMessage(email, password)
                .SetResponseHandler(response => successCallback?.Invoke(response.TokenData))
                .SetErrorHandler(errorCallback);

            _httpService.SendMessage(message).Forget();
        }
    }
}