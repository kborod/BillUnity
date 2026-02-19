using Newtonsoft.Json;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages
{
    public class IamAliveDto : IRequest
    {
        #region IRequestMeta
        [JsonIgnore]
        public bool IsRequired => false;
        [JsonIgnore]
        public RequestType RequestType => RequestType.IamAlive;
        #endregion
    }
}
