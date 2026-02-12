using Newtonsoft.Json;
using System.Collections.Generic;

namespace Kborod.Services.ServerCommunication.Sockets.Messages
{
    public class LastMessagesResponseDto : IResponseMeta
    {
        #region IResponseMeta
        [JsonIgnore]
        public bool IsRequired => true;
        [JsonIgnore]
        public ResponseType ResponseType => ResponseType.LastResponsesListResponse;
        #endregion

        public List<ResponseEnvelope> LastResponses { get; set; }

        public LastMessagesResponseDto(List<ResponseEnvelope> lastResponses)
        {
            LastResponses = lastResponses;
        }
    }
}
