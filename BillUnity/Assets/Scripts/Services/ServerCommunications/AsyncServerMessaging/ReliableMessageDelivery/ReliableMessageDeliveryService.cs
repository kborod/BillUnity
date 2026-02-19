using Cysharp.Threading.Tasks;
using Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging.ReliableMessageDelivery
{
    public class ReliableMessageDeliveryService : IMessagingService
    {
        public event Action Connected;
        public event Action Disconnected;
        public event Action<ResponseEnvelope> ResponseReceived;

        private readonly ProtocolResponsesProcessor _responsesProcessor = new();

        private readonly Dictionary<Type, object> _handlers = new();

        private IMessagingTransport _messagingTransport;
        private SessionInfo _sessionMessages = null;

        [Inject]
        public void Construct(IMessagingTransport transport)
        {
            _messagingTransport = transport;
            _messagingTransport.Connected += ConnectedHandler;
            _messagingTransport.Disconnected += DisconnectedHandler;
            _messagingTransport.ResponseReceived += ResponseReceivedHandler;
        }

        public async UniTask Connect() => await _messagingTransport.Connect();

        public async UniTask Disconnect() => await _messagingTransport.Disconnect();

        public void SendRequest<T>(T requestDto) where T : IRequest
        {
            if (_sessionMessages == null)
            {
                Debug.LogError("You need to connect and create session first! Request cancelled");
                return;
            }
            var request = requestDto.IsRequired 
                ? RequestEnvelope.Create(requestDto, _sessionMessages.GetNextRequestNumber())
                : RequestEnvelope.Create(requestDto);

            if (request.IsRequired)
                _sessionMessages.AddRequest(request);

            _messagingTransport.SendRequest(request);
        }

        public void Subscribe<T>(Action<T> handler) where T : IResponse
        {
            var type = typeof(T);
            if (_handlers.TryGetValue(type, out var set) == false)
            {
                _handlers.Add(type, set = new HashSet<Action<T>>());
            }
            (set as HashSet<Action<T>>).Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler) where T : IResponse
        {
            if (_handlers.TryGetValue(typeof(T), out var obj) &&
                obj is HashSet<Action<T>> set)
            {
                set.Remove(handler);
                if (set.Count == 0) _handlers.Remove(typeof(T));
            }
        }

        private void ConnectedHandler()
        {
            InitSession();
            Connected?.Invoke();
        }

        private void DisconnectedHandler()
        {
            _sessionMessages = null;
            Disconnected?.Invoke();
        }

        private void InitSession()
        {
            _messagingTransport.SendRequest(RequestEnvelope.Create(new StartSessionDto()));
            _sessionMessages = new SessionInfo();
        }

        private void ResponseReceivedHandler(ResponseEnvelope response)
        {
            var validateResult = _responsesProcessor.GetValidatedResponses(response, _sessionMessages, _messagingTransport);
            if (!validateResult.IsSuccess)
            {
                Debug.Log($"[MessageDeliveryService] Validate response error: {validateResult.Error}");
                _ = Disconnect();
                return;
            }

            var responsesForProcess = validateResult.Value;

            if (responsesForProcess == null)
                return;

            foreach (var r in responsesForProcess)
            {
                RaiseResponseReceived(response.GetPayload());
                Debug.Log($"[MessageDeliveryService] Response {r.SequenceNumber} processed");
            }
            Debug.Log($"Session: {_sessionMessages}");

        }

        private void RaiseResponseReceived(IResponse response)
        {
            var set = Gethandlers(response);
            if (set == null)
                return;
            foreach (var handler in set)
            {
                try { handler.Invoke(response); }
                catch (Exception ex)
                {
                    Debug.LogError($"Response {response.GetType().Name} handler invoking error : {ex}");
                }
            }

            HashSet<Action<T>> Gethandlers<T>(T response) where T : IResponse
            {
                if (_handlers.TryGetValue(typeof(T), out var obj) && obj is HashSet<Action<T>> set)
                    return set;
                return null;
            }
        }
    }
}