using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class CancelRematchDto : IRequest
    {
        #region IRequestMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public RequestType RequestType => RequestType.CancelRematch;
        #endregion

        public string MatchId { get; set; }

        public CancelRematchDto(string matchId)
        {
            MatchId = matchId;
        }
    }
}
