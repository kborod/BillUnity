using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class AddedToQueueResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.AddedToQueueResponse;
        #endregion
    }
}
