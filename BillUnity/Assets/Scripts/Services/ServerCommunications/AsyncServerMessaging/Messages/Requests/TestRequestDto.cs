using Newtonsoft.Json;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages
{
    public class TestRequestDto : IRequest
    {
        #region IRequestMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public RequestType RequestType => RequestType.Test;
        #endregion

        public string Data { get; set; }

        public TestRequestDto(string data)
        {
            Data = data;
        }
    }
}
