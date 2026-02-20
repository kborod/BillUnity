using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class MessageReceivedResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => false;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.MessageReceivedResponse;
        #endregion

        public int LastReceivedRequest { get; set; }

        public MessageReceivedResponseDto(int lastReceivedRequest)
        {
            LastReceivedRequest = lastReceivedRequest;
        }
    }
}
