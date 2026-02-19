using Newtonsoft.Json;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages
{
    public class SessionErrorResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => false;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.SessionErrorResponse;
        #endregion

        public string Error { get; set; }

        public SessionErrorResponseDto(string error)
        {
            Error = error;
        }
    }
}
