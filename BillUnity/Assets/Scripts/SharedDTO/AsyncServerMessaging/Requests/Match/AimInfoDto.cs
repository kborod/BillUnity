using Kborod.BilliardCore;
using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class AimInfoDto : IRequest
    {
        #region IRequestMeta
        [JsonIgnore]
        public bool IsRequired => false;
        [JsonIgnore]
        public RequestType RequestType => RequestType.AimInfo;
        #endregion

        public AimInfoData AimInfoData { get; set; }

        public AimInfoDto(AimInfoData aimInfoData)
        {
            AimInfoData = aimInfoData;
        }
    }
}
