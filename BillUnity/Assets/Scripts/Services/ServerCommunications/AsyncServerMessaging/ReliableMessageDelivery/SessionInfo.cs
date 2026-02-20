using Kborod.SharedDto.AsyncServerMessaging.Messages;
using System.Collections.Generic;
using System.Linq;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.ReliableMessageDelivery
{
    public class SessionInfo
    {
        public int LastReceivedResponseNumber { get; private set; } = 0;

        private List<RequestEnvelope> _requestsCache = new();

        private int _lastRequestNumber = 0;
        
        public int GetNextRequestNumber() => ++_lastRequestNumber;

        public void SetLastReceivedResponseNumber(int number)
        {
            LastReceivedResponseNumber = number;
        }

        public void AddRequest(RequestEnvelope request)
        {
            _requestsCache.Add(request);
        }

        public List<RequestEnvelope> GetRequestsFromNumber(int numberInclusive)
        {
            return _requestsCache
                .Where(r => r.SequenceNumber >= numberInclusive)
                .OrderBy(r => r.SequenceNumber)
                .ToList();
        }

        public void RemoveRequestsBeforeNumber(int numberInclusive)
        {
            _requestsCache.RemoveAll(r => r.SequenceNumber <= numberInclusive);
        }

        public override string ToString()
        {
            return $"LastResponse:{LastReceivedResponseNumber}; LastRequest:{_lastRequestNumber}; ResponsesCache: {_requestsCache.Count}";
        }
    }
}