using Cysharp.Threading.Tasks;
using Kborod.Services.LocalCache;
using Kborod.Services.ServerTime;
using Zenject;

namespace Kborod.Services.ServerCommunication.Token
{
    public class TokenService : ITokenService
    {
        [Inject] private TokenRefreshService _tokenRefreshService;
        [Inject] private PlayerPrefsService _playerPrefsService;
        [Inject] private TimeControl _timeService;

        private TokenData _tokenData;

        public async UniTask<string> GetTokenOrNull()
        {
            await TryRefreshTokenData();
            return _tokenData?.Token;
        }


        public async UniTask<bool> TryLoadFromCache()
        {
            if (_playerPrefsService.LoadOrDefault<bool>(PlayerPrefKey.CacheTokenEnabled) == false)
            {
                ClearCache();
                return false;
            }

            _tokenData = _playerPrefsService.LoadOrDefault<TokenData>(PlayerPrefKey.TokenData);

            await TryRefreshTokenData();
            return _tokenData != null;
        }

        public void Set(TokenData data)
        {
            _tokenData = data;
            _playerPrefsService.Save(data, PlayerPrefKey.TokenData);
        }

        private void ClearCache()
        {
            _playerPrefsService.Clear(PlayerPrefKey.TokenData);
        }

        private async UniTask TryRefreshTokenData()
        {
            if (_tokenData == null)
                return;

            if (_tokenData.TokenExpiredTimestamp < _timeService.CurrTimestamp)
            {
                _tokenData = await _tokenRefreshService.TryRefresh(_tokenData);
                Set(_tokenData);
            }
        }
    }
}
