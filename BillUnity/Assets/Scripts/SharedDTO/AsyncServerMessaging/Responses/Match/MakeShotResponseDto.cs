using Kborod.BilliardCore;
using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class MakeShotResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.MakeShotResponse;
        #endregion

        public MakeShotData MakeShotData { get; set; }

        public MakeShotResponseDto(MakeShotData makeShotData)
        {
            MakeShotData = makeShotData;
        }
    }
}
