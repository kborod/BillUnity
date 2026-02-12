using Newtonsoft.Json.Linq;

namespace Kborod.Services.ServerCommunication.Sockets.Messages
{
    public class RequestEnvelope
    {
        public bool IsRequired { get; set; }

        public int SequenceNumber { get; set; }

        public RequestType RequestType { get; set; }

        public JObject Payload { get; set; }

        public static RequestEnvelope Create<T>(T payload) where T : IRequestMeta
        {
            var payloadJson = JObject.FromObject(payload);
            return new RequestEnvelope
            {
                IsRequired = payload.IsRequired,
                SequenceNumber = 0,
                RequestType = payload.RequestType,
                Payload = payloadJson
            };
        }
    }
}

/* Copyright: Made by Appfox */