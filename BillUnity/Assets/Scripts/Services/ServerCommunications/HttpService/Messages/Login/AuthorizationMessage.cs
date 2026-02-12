using Kborod.SharedDto.Auth;

namespace Kborod.Services.ServerCommunication
{
    public class AuthorizationMessage : HttpGenericMessageBase<LoginDto, LoginResponseDto>
    {
        public override string MethodType => UnityEngine.Networking.UnityWebRequest.kHttpVerbPOST;
        public override string ApiAddress => $"Auth/login";
        public override TargetApi TargetApi => TargetApi.MainApi;

        public AuthorizationMessage(string email, string password)
            : base(new() { Email = email, Password = password })
        {
        }
    }
}

/* Copyright: Made by Appfox */