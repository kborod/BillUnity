using Kborod.BilliardCore;
using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class MakeShotDto : IRequest
    {
        #region IRequestMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public RequestType RequestType => RequestType.MakeShot;
        #endregion

        public MakeShotData MakeShotData { get; set; }

        public MakeShotDto(MakeShotData makeShotData)
        {
            MakeShotData = makeShotData;
        }
    }
}
