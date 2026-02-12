using Newtonsoft.Json;

namespace Kborod.Services.ServerCommunication.Sockets.Messages
{
    public class ResendLastMessagesResponseDto : IResponseMeta
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.ResendLastRequestsResponse;
        #endregion

        public int FromNumberInclusive { get; set; }

        public ResendLastMessagesResponseDto(int fromNumberInclusive)
        {
            FromNumberInclusive = fromNumberInclusive;
        }
    }
}
