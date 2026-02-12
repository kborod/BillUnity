using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kborod.Services.ServerCommunication.Sockets.Messages
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

        public static ResponseEnvelope Create<T>(T payload) where T : IResponseMeta
        {
            var payloadJson = JObject.FromObject(payload);
            return new ResponseEnvelope
            {
                IsRequired = payload.IsRequired,
                SequenceNumber = 0,
                ResponseType = payload.ResponseType,
                Payload = payloadJson
            };
        }
    }
}

/* Copyright: Made by Appfox */