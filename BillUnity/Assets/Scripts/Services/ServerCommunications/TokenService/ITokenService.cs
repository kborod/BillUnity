using Cysharp.Threading.Tasks;

namespace Kborod.Services.ServerCommunication.Token
{
    public interface ITokenService
    {
        public UniTask<string> GetTokenOrNull();
        public void Set(TokenData tokenData);
        public UniTask<bool> TryLoadFromCache();
    }
}

/* Copyright: Made by Appfox */