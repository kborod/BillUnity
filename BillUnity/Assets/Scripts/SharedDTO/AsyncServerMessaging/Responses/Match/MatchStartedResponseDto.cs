using Kborod.BilliardCore;
using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class MatchStartedResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.MatchStartedResponse;
        #endregion

        public StartMatchData StartMatchData { get; set; }

        public MatchStartedResponseDto(StartMatchData startMatchData)
        {
            StartMatchData = startMatchData;
        }
    }
}
