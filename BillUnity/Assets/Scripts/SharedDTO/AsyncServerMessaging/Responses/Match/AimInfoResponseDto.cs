using Kborod.BilliardCore;
using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class AimInfoResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => false;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.AimInfoResponse;
        #endregion

        public AimInfoData AimInfoData { get; set; }

        public AimInfoResponseDto(AimInfoData aimInfoData)
        {
            AimInfoData = aimInfoData;
        }
    }
}
