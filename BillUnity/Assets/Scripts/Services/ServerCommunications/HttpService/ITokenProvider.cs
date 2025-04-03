namespace Kborod.Services.ServerHTTPCommunication
{
    public interface ITokenProvider
    {
        public string TokenOrNull { get; }

        public void RefreshTokenIfNeed();
    }
}

/* Copyright: Made by Appfox */