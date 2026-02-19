using Newtonsoft.Json;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages
{
    public class StartSessionDto : IRequest
    {
        #region IRequestMeta
        [JsonIgnore]
        public bool IsRequired => false;
        [JsonIgnore]
        public RequestType RequestType => RequestType.StartSession;
        #endregion
    }
}
