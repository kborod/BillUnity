using Kborod.Services.ServerHTTPCommunication;
using System;

namespace kborod.Services.ServerHTTPCommunication
{
    public class AuthorizationMessage : HttpGenericMessageBase<LoginRequest, LoginResponse>
    {
        public override string MethodType => UnityEngine.Networking.UnityWebRequest.kHttpVerbPOST;
        public override string ApiAddress => $"auth/login";
        public override TargetApi TargetApi => TargetApi.Authorization;

        public AuthorizationMessage(string nick)
            : base(new LoginRequest() { Nick = nick })
        {
        }
    }

    public class LoginRequest
    {
        public string Nick;
    }

    public class LoginResponse
    {
        public LoginData Content;
    }

    public class LoginData
    {
        public string refresh;
        public DateTime refreshExpiredAt;
        public long refreshExpiredAtUnix;
        public string jwt;
        public DateTime jwtExpiredAt;
        public long jwtExpiredAtUnix;
    }
}

/* Copyright: Made by Appfox */