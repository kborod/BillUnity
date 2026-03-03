
using Kborod.BilliardCore;
using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class MatchOverResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.MatchOverResponse;
        #endregion

        public MatchOverData MatchOverData { get; set; }

        public MatchOverResponseDto(MatchOverData matchOverData)
        {
            MatchOverData = matchOverData;
        }
    }
}
