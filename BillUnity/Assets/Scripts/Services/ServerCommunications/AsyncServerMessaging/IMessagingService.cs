using Cysharp.Threading.Tasks;
using Kborod.SharedDto.AsyncServerMessaging.Messages;
using System;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging
{
    public interface IMessagingService
    {
        public event Action Connected;
        public event Action Disconnected;

        UniTask Connect();
        UniTask Disconnect();
        void SendRequest<T>(T request) where T : IRequest;

        void Subscribe<T>(Action<T> handler) where T : IResponse;
        void Unsubscribe<T>(Action<T> handler) where T : IResponse;
    }
}