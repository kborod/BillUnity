using Kborod.BilliardCore;
using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class ShotResultDto : IRequest
    {
        #region IRequestMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public RequestType RequestType => RequestType.ShotResult;
        #endregion

        public SyncronizationInfo SynchronizationInfo { get; set; }

        public ShotResultDto(SyncronizationInfo synchronizationInfo)
        {
            SynchronizationInfo = synchronizationInfo;
        }
    }
}
