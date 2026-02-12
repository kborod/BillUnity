namespace Kborod.Services.ServerCommunication.Token
{
    public class TokenData
    {
        public string Token;
        public long TokenExpiredTimestamp;
        public string RefreshToken;
        public long RefreshTokenExpiredTimestamp;
    }
}
