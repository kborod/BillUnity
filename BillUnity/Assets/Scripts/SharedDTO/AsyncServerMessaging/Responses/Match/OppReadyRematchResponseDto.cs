using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class OppReadyRematchResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.OppReadyRematchResponse;
        #endregion

        public string MatchId { get; set; }

        public OppReadyRematchResponseDto(string matchId)
        {
            MatchId = matchId;
        }
    }
}
