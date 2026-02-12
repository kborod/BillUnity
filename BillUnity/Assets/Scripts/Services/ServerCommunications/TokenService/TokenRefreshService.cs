using Cysharp.Threading.Tasks;
using Kborod.Services.ServerTime;
using Zenject;

namespace Kborod.Services.ServerCommunication.Token
{
    public class TokenRefreshService
    {
        [Inject] private TimeControl _timeService;

        public async UniTask<TokenData> TryRefresh(TokenData data)
        {
            if (data.RefreshTokenExpiredTimestamp < _timeService.CurrTimestamp)
                return null;

            //TODO BORODIN добавить реализацию рефреша токена
            await UniTask.NextFrame();
            return null;
        }
    }
}
