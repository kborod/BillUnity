using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class MatchInitedDto : IRequest
    {
        #region IRequestMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public RequestType RequestType => RequestType.MatchInited;
        #endregion

        public string MatchId { get; set; }

        public MatchInitedDto(string matchId)
        {
            MatchId = matchId;
        }
    }
}
