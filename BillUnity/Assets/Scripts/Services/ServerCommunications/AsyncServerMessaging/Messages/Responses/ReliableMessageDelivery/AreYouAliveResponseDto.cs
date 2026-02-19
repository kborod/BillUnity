using Newtonsoft.Json;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages
{
    public class AreYouAliveResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => false;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.AreYouAliveResponse;
        #endregion
    }
}
