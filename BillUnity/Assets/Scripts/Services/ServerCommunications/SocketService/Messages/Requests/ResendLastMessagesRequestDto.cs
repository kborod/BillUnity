using Newtonsoft.Json;

namespace Kborod.Services.ServerCommunication.Sockets.Messages
{
    public class ResendLastMessagesRequestDto : IRequestMeta
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public RequestType RequestType => RequestType.ResendLastResponses;
        #endregion

        public int FromNumberInclusive { get; set; }

        public ResendLastMessagesRequestDto(int fromNumberInclusive)
        {
            FromNumberInclusive = fromNumberInclusive;
        }
    }
}
