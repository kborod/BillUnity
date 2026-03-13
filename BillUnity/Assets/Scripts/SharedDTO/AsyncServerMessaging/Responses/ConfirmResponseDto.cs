using Newtonsoft.Json;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class ConfirmResponseDto : IResponse
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => false;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.ConfirmResponse;
        #endregion

        public string Message { get; set; }

        public ConfirmResponseDto(string message)
        {
            Message = message;
        }
    }
}
