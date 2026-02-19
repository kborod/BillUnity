using Newtonsoft.Json;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages
{
    public class TestResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.TestResponse;
        #endregion

        public string Data { get; set; }

        public TestResponseDto(string data)
        {
            Data = data;
        }
    }
}
