using Best.SignalR;
using Best.SignalR.Encoders;
using Best.SignalR.Messages;
using Cysharp.Threading.Tasks;
using Kborod.Services.ServerCommunication.Token;
using Newtonsoft.Json;
using System;
using UnityEngine;
using Zenject;

namespace Kborod.Services.ServerCommunication.Sockets
{
    public class SocketService
    {
        public event Action HubCreated;

        //private const string _serverAddr = "http://localhost:5237/";
        private const string _serverAddr = "https://localhost:7155/";
        private const string _hubAddr = "gameHub";

        [Inject] private ITokenService _tokenProvider;
        private string token;

        private HubConnection hub;

        public async UniTask ConnectHubAsync(string token)
        {
            this.token = token;

            await TryDisconnectAsync();
            try
            {
                if (hub == null || hub.State != ConnectionStates.Connected)
                {
                    Debug.Log("1");
                    CreateHubConnection();
                    Debug.Log("2");
                    SubscribeHubEvents();
                    Debug.Log("3");
                    await hub.ConnectAsync();
                    Debug.Log($"<color=green>Hub connection state: {hub.State}</color>");
                    if (hub.State == ConnectionStates.Connected)
                        HubCreated?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public async UniTask Disconnect()
        {
            await TryDisconnectAsync();
        }

        //public void Send<T>(SocketMethod method, T data)
        //{
        //    Debug.Log($"<color=blue>[Hub message sent]</color> <color=yellow>{method}</color>:{JsonConvert.SerializeObject(data)}");
        //    hub.Send(method.ToString(), data)
        //        .OnSuccess(response => Debug.Log($"<color=green>[Hub message sent success]</color> <color=yellow>{method}</color>"))
        //        .OnError(error => Debug.Log($"<color=red>[Hub message sent error]</color> <color=yellow>{method}</color> error:{JsonConvert.SerializeObject(error)}"));
        //}

        //public void Send(SocketMethod method)
        //{
        //    hub.Send(method.ToString())
        //        .OnSuccess(response => Debug.Log($"<color=green>[Hub message sent success]</color> <color=yellow>{method}</color>"))
        //        .OnError(error => Debug.Log($"<color=red>[Hub message sent error]</color> <color=yellow>{method}</color> error:{JsonConvert.SerializeObject(error)}"));
        //}

        public void Invoke<TReq, TResp>(string method, TReq data, Action<TResp> callback, Action<Exception> errorCallback)
        {
            Debug.Log($"[Hub request sent] <color=yellow>{method}</color>:{JsonConvert.SerializeObject(data)}");

            hub.Invoke<TResp>(method, data)
                .OnSuccess(response =>
                {
                    Debug.Log($"[Hub response] <color=yellow>{method}</color>:{JsonConvert.SerializeObject(response)}");
                    callback?.Invoke(response);
                })
                .OnError(error =>
                {
                    Debug.Log($"[Hub response] <color=red>{method}</color> error:{JsonConvert.SerializeObject(error)}");
                    errorCallback?.Invoke(error);
                });
        }

        //public void HandleMethod<T>(SocketMethod method, Action<T> callback) => hub.On(method.ToString(), callback);
        //public void HandleMethod<T1, T2>(SocketMethod method, Action<T1, T2> callback) => hub.On(method.ToString(), callback);

        //public void HandleMethod(SocketMethod method, Action callback) => hub.On(method.ToString(), callback);

        private async UniTask TryDisconnectAsync()
        {
            if (hub != null && hub.State != ConnectionStates.Closed && hub.State != ConnectionStates.CloseInitiated)
            {
                await hub.CloseAsync();
                hub = null;
            }
        }

        private void CreateHubConnection()
        {
            var uriBuilder = new UriBuilder($"{_serverAddr}{_hubAddr}");
            uriBuilder.Query = "access_token=" + token;
            HubOptions options = new HubOptions();
            hub = new HubConnection(uriBuilder.Uri, new JsonProtocol(new JsonDotNetEncoder()), options);
            hub.ReconnectPolicy = new RetryPolicy();
        }

        private void SubscribeHubEvents()
        {
            hub.OnConnected += (hub) => Debug.Log($"<color=green>Hub {GetUriForLog()}:</color> connect");
            hub.OnError += (hub, arg2) => Debug.LogError($"<color=red>hub {GetUriForLog()}:</color> error!" + arg2);
            hub.OnClosed += (hub) => Debug.Log($"<color=red>Hub {GetUriForLog()}:</color> closed!");
            hub.OnReconnecting += (hub, arg2) => Debug.Log($"<color=#FF7575>Hub {GetUriForLog()}:</color> reconnecting");
            hub.OnReconnected += (hub) => Debug.Log($"<color=green>Hub {GetUriForLog()}:</color> reconnected");
            hub.OnTransportEvent += (hub, transp, events) => Debug.Log($"<color=blue>Hub {GetUriForLog()} {transp.TransportType}</color> transportEvent: {events}");
            hub.OnMessage += OnHubMessageReceived;

            string GetUriForLog() => $"{hub.Uri.ToString().Substring(0, 50)}...";
        }

        private bool OnHubMessageReceived(HubConnection connection, Message message)
        {
            if (message.type == MessageTypes.Invocation)
                Debug.Log($"<color=orange>Hub message</color> <color=yellow>{message.target}</color> received:{JsonConvert.SerializeObject(message)}");
            return true;
        }
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

/* Copyright: Made by Appfox */