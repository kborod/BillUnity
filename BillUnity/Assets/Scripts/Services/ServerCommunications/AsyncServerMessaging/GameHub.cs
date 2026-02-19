using Best.SignalR;
using Best.SignalR.Encoders;
using Best.SignalR.Messages;
using Cysharp.Threading.Tasks;
using Kborod.Services.ServerCommunication.AsyncServerMessaging.Messages;
using Kborod.Services.ServerCommunication.Token;
using Newtonsoft.Json;
using System;
using UnityEngine;
using Zenject;

namespace Kborod.Services.ServerCommunication.AsyncServerMessaging
{
    public class GameHub : IMessagingTransport
    {
        public event Action Connected;
        public event Action Disconnected;
        public event Action<ResponseEnvelope> ResponseReceived;

        //private const string _serverAddr = "http://localhost:5237/";
        private const string _serverAddr = "https://localhost:7155/";
        private const string _hubAddr = "gameHub";

        [Inject] private ITokenService _tokenProvider;

        private HubConnection _hub;

        public async UniTask Connect()
        {
            //HTTPManager.Logger.Level = Loglevels.All;

            await TryDisconnectAsync();
            await CreateHubConnection();
            SubscribeHubEvents();
            await _hub.ConnectAsync();
            if (_hub.State != ConnectionStates.Connected)
                throw new Exception($"State = {_hub.State}");
        }

        public async UniTask Disconnect()
        {
            await TryDisconnectAsync();
            Debug.Log("<color=red>[Hub] disconnected</color>");
        }

        public void SendRequest(RequestEnvelope request)
        {
            Debug.Log($"<color=green>[Hub]->Sent->:</color><color=yellow>[{request.RequestType}]</color> SeqNum:{request.SequenceNumber} {JsonConvert.SerializeObject(request)}");

            _hub.Invoke<RequestEnvelope>("ProcessRequest", request)
                .OnError(error => Debug.Log($"<color=red>[Hub]SendingError</color><color=yellow>[{request.RequestType}]</color>  SeqNum:{request.SequenceNumber} {JsonConvert.SerializeObject(error)}"));
        }

        private async UniTask CreateHubConnection()
        {
            var uriBuilder = new UriBuilder($"{_serverAddr}{_hubAddr}");
            var token = await _tokenProvider.GetTokenOrNull();
            uriBuilder.Query = "access_token=" + token;
            HubOptions options = new HubOptions();
            _hub = new HubConnection(uriBuilder.Uri, new JsonProtocol(new JsonDotNetEncoder()), options);
            _hub.ReconnectPolicy = new RetryPolicy();
        }

        private void SubscribeHubEvents()
        {
            _hub.OnConnected += ConnectedHandler;
            _hub.OnClosed += ClosedHandler;
            _hub.OnError += ErrorHandler;
            _hub.OnReconnecting += (hub, arg2) => Debug.Log($"<color=#FF7575>[Hub] {GetUriForLog(hub)}:</color> reconnecting");
            _hub.OnReconnected += (hub) => Debug.Log($"<color=green>[Hub] {GetUriForLog(hub)}:</color> reconnected");
            _hub.OnTransportEvent += (hub, transp, events) => Debug.Log($"<color=blue>[Hub] {GetUriForLog(hub)} {transp.TransportType}</color> transportEvent: {events}");
            _hub.OnMessage += OnHubMessageReceived;
            _hub.On<ResponseEnvelope>("ProcessResponse", ResponseReceivedHandler);
        }

        private void ErrorHandler(HubConnection hub, string arg2)
        {
            Debug.LogError($"<color=red>[Hub] {GetUriForLog(hub)}:</color> error!" + arg2);
            _ = Disconnect();
        }

        private void ResponseReceivedHandler(ResponseEnvelope response)
        {
            Debug.Log($"<color=green>[Hub]<-Received<-:</color><color=yellow>[{response.ResponseType}]</color> SeqNum:{response.SequenceNumber} {JsonConvert.SerializeObject(response)}");
            ResponseReceived?.Invoke(response);
        }

        private void ConnectedHandler(HubConnection hub)
        {
            Debug.Log($"<color=green>[Hub] {GetUriForLog(hub)}:</color> connected");
            Connected?.Invoke();
        }

        private void ClosedHandler(HubConnection hub)
        {
            Debug.Log($"<color=red>[Hub] {GetUriForLog(hub)}:</color> closed!");
        }

        private bool OnHubMessageReceived(HubConnection connection, Message message)
        {
            if (message.type != MessageTypes.Invocation)
                return true;
            if (message.target == "ProcessResponse")
                return true;
            Debug.Log($"<color=orange>[Hub] message</color> <color=yellow>{message.target}</color> received:{JsonConvert.SerializeObject(message)}");
            return true;
        }

        private async UniTask TryDisconnectAsync()
        {
            if (_hub != null && _hub.State != ConnectionStates.Closed && _hub.State != ConnectionStates.CloseInitiated)
            {
                await _hub.CloseAsync();
                _hub = null;
            }
        }

        private string GetUriForLog(HubConnection hub) => $"{_hub.Uri.ToString().Substring(0, 50)}...";
    }

    public class RetryPolicy : IRetryPolicy
    {
        public TimeSpan? GetNextRetryDelay(RetryContext context)
        {
            if (context.ElapsedTime > TimeSpan.FromSeconds(10) && context.RetryReason.Contains("Connection Timed Out"))
                return null;

            return context.PreviousRetryCount > 4 ? null : TimeSpan.FromSeconds(context.PreviousRetryCount);
        }
    }
}