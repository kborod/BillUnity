using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class ResponseEnvelope
    {
        [JsonProperty(Required = Required.Always)]
        public bool IsRequired { get; set; }

        [JsonProperty(Required = Required.Always)]
        public int SequenceNumber { get; set; }

        [JsonProperty(Required = Required.Always)]
        public ResponseType ResponseType { get; set; }

        [JsonProperty(Required = Required.Always)]
        public JObject Payload { get; set; }

        public static ResponseEnvelope Create<T>(T payload) where T : IResponse
        {
            var payloadJson = JObject.FromObject(payload);
            return new ResponseEnvelope
            {
                IsRequired = payload.IsRequired,
                SequenceNumber = -1,
                ResponseType = payload.ResponseType,
                Payload = payloadJson
            };
        }
    }
}