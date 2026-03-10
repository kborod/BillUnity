using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class LeaveMatchDto : IRequest
    {
        #region IRequestMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public RequestType RequestType => RequestType.LeaveMatch;
        #endregion

        public string MatchId { get; set; }

        public LeaveMatchDto(string matchId)
        {
            MatchId = matchId;
        }
    }
}
