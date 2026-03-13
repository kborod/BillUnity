using Cysharp.Threading.Tasks;
using Kborod.SharedDto.AsyncServerMessaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
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
                switch (r.ResponseType)
                {
                    case ResponseType.ErrorResponse: RaiseResponseReceived(response.GetPayload<ErrorResponseDto>()); break;
                    case ResponseType.ConfirmResponse: RaiseResponseReceived(response.GetPayload<ConfirmResponseDto>()); break;
                    case ResponseType.TestResponse: RaiseResponseReceived(response.GetPayload<TestResponseDto>()); break;

                    //Протокольные сообщения до сюда не дойдут, так как обрабатываются в ProtocolResponsesProcessor
                    //и не передаются дальше, но на всякий случай оставлю их здесь закомментированными
                    //case ResponseType.MessageReceivedResponse: RaiseResponseReceived(response.GetPayload<MessageReceivedResponseDto>()); break;
                    //case ResponseType.ResendLastRequestsResponse: RaiseResponseReceived(response.GetPayload<ResendLastMessagesResponseDto>()); break;
                    //case ResponseType.AreYouAliveResponse: RaiseResponseReceived(response.GetPayload<AreYouAliveResponseDto>()); break;
                    //case ResponseType.LastResponsesListResponse: RaiseResponseReceived(response.GetPayload<LastMessagesResponseDto>()); break;
                    //case ResponseType.SessionErrorResponse: RaiseResponseReceived(response.GetPayload<SessionErrorResponseDto>()); break;

                    case ResponseType.AddedToQueueResponse: RaiseResponseReceived(response.GetPayload<AddedToQueueResponseDto>()); break;
                    case ResponseType.SearchCancelledResponse: RaiseResponseReceived(response.GetPayload<SearchCancelledResponseDto>()); break;
                    case ResponseType.MatchStartedResponse: RaiseResponseReceived(response.GetPayload<MatchStartedResponseDto>()); break;
                    case ResponseType.AimInfoResponse: RaiseResponseReceived(response.GetPayload<AimInfoResponseDto>()); break;
                    case ResponseType.MakeShotResponse: RaiseResponseReceived(response.GetPayload<MakeShotResponseDto>()); break;
                    case ResponseType.StartTurnResponse: RaiseResponseReceived(response.GetPayload<StartTurnResponseDto>()); break;
                    case ResponseType.MatchOverResponse: RaiseResponseReceived(response.GetPayload<MatchOverResponseDto>()); break;
                    case ResponseType.OppCancelRematchResponse: RaiseResponseReceived(response.GetPayload<OppCancelRematchResponseDto>()); break;
                    case ResponseType.OppReadyRematchResponse: RaiseResponseReceived(response.GetPayload<OppReadyRematchResponseDto>()); break;
                    case ResponseType.CancelRematchConfirmResponse: RaiseResponseReceived(response.GetPayload<CancelRematchConfirmResponseDto>()); break;

                    default: throw new NotImplementedException();

                }

                Debug.Log($"[MessageDeliveryService] Response {r.SequenceNumber} processed");
            }
            Debug.Log($"Session: {_sessionMessages}");

        }

        private void RaiseResponseReceived<T>(T response) where T:  IResponse
        {
            var t = response.GetType();

            var set = GetHandlers(response);
            if (set == null || set.Count == 0)
                return;

            var immutableSetCopy = set.ToArray();


            foreach (var handler in immutableSetCopy)
            {
                try { handler.Invoke(response); }
                catch (Exception ex)
                {
                    Debug.LogError($"Response {response.GetType().Name} handler invoking error : {ex}");
                }
            }
        }

        private HashSet<Action<T>> GetHandlers<T>(T response) where T : IResponse
        {
            if (_handlers.TryGetValue(response.GetType(), out var obj))
            {
                if (obj is HashSet<Action<T>> set)
                    return set;
            }
            return null;
        }
    }
}