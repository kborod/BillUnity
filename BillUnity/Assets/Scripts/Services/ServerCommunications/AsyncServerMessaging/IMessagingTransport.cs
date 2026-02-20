using Cysharp.Threading.Tasks;
using Kborod.SharedDto.AsyncServerMessaging.Messages;
using System;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging
{
    public interface IMessagingTransport
    {
        public event Action Connected;
        public event Action Disconnected;
        public event Action<ResponseEnvelope> ResponseReceived;

        UniTask Connect();
        UniTask Disconnect();
        void SendRequest(RequestEnvelope request);
    }
}