using Newtonsoft.Json.Linq;

namespace Kborod.SharedDto.AsyncServerMessaging.Messages
{
    public class RequestEnvelope
    {
        public bool IsRequired { get; set; }

        public int SequenceNumber { get; set; }

        public RequestType RequestType { get; set; }

        public JObject Payload { get; set; }

        public static RequestEnvelope Create<T>(T payload, int sequenceNumber = -1) where T : IRequest
        {
            var payloadJson = JObject.FromObject(payload);
            return new RequestEnvelope
            {
                IsRequired = payload.IsRequired,
                SequenceNumber = sequenceNumber,
                RequestType = payload.RequestType,
                Payload = payloadJson
            };
        }
    }
}