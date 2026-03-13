using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class CancelRematchConfirmResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.CancelRematchConfirmResponse;
        #endregion

        public string MatchId { get; set; }

        public CancelRematchConfirmResponseDto(string matchId)
        {
            MatchId = matchId;
        }
    }
}
