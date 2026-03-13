using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class OppCancelRematchResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.OppCancelRematchResponse;
        #endregion

        public string MatchId { get; set; }

        public OppCancelRematchResponseDto(string matchId)
        {
            MatchId = matchId;
        }
    }
}
