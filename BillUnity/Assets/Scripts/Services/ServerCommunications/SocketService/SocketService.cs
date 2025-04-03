//using BestHTTP.SignalRCore;
//using BestHTTP.SignalRCore.Authentication;
//using BestHTTP.SignalRCore.Encoders;
//using BestHTTP.SignalRCore.Messages;
//using Cysharp.Threading.Tasks;
//using Newtonsoft.Json;
//using System;
//using UnityEngine;
//using Zenject;

namespace Dexnet.Services.ServerHTTPCommunication
{
    public class SocketService
    {
        //public event Action HubCreated;

        //private const string _serverAddr = "http://51.250.97.19:7400/";
        //private const string _hubAddr = "hub/match";

        //[Inject] private ITokenProvider _tokenProvider;

        //private HubConnection hub;

        //public async UniTask ConnectHubAsync()
        //{
        //    await TryDisconnectAsync();
        //    if (hub == null || hub.State != ConnectionStates.Connected)
        //    {
        //        CreateHubConnection();
        //        SubscribeHubEvents();
        //        await hub.ConnectAsync();
        //        Debug.Log($"<color=green>Hub connection state: {hub.State}</color>");
        //        if (hub.State == ConnectionStates.Connected)
        //            HubCreated?.Invoke();
        //    }
        //}

        //public async UniTask Disconnect()
        //{
        //    await TryDisconnectAsync();
        //}

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

        //public void Invoke<TReq, TResp>(string method, TReq data, Action<TResp> callback, Action<Exception> errorCallback)
        //{
        //    Debug.Log($"[Hub request sent] <color=yellow>{method}</color>:{JsonConvert.SerializeObject(data)}");

        //    hub.Invoke<TResp>(method, data)
        //        .OnSuccess(response =>
        //        {
        //            Debug.Log($"[Hub response] <color=yellow>{method}</color>:{JsonConvert.SerializeObject(response)}");
        //            callback?.Invoke(response);
        //        })
        //        .OnError(error =>
        //        {
        //            Debug.Log($"[Hub response] <color=red>{method}</color> error:{JsonConvert.SerializeObject(error)}");
        //            errorCallback?.Invoke(error);
        //        });
        //}

        //public void HandleMethod<T>(SocketMethod method, Action<T> callback) => hub.On(method.ToString(), callback);
        //public void HandleMethod<T1, T2>(SocketMethod method, Action<T1, T2> callback) => hub.On(method.ToString(), callback);

        //public void HandleMethod(SocketMethod method, Action callback) => hub.On(method.ToString(), callback);

        //private async UniTask TryDisconnectAsync()
        //{
        //    if (hub != null && hub.State != ConnectionStates.Closed && hub.State != ConnectionStates.CloseInitiated)
        //    {
        //        await hub.CloseAsync();
        //        hub = null;
        //    }
        //}

        //private void CreateHubConnection()
        //{
        //    HubOptions options = new HubOptions();
        //    hub = new HubConnection(new Uri($"{_serverAddr}{_hubAddr}"), new JsonProtocol(new JsonDotNetEncoder()), options)
        //    {
        //        ReconnectPolicy = new DefaultRetryPolicy(),
        //        AuthenticationProvider = new HeaderAuthenticator(_tokenProvider.TokenOrNull)
        //    };
        //}

        //private void SubscribeHubEvents()
        //{
        //    hub.OnConnected += (hub) => Debug.Log($"<color=green>Hub {hub.Uri} connect</color>");
        //    hub.OnError += (hub, arg2) => Debug.LogError($"<color=red>hub {hub.Uri} error!</color>" + arg2);
        //    hub.OnClosed += (hub) => Debug.Log($"<color=red>Hub {hub.Uri} closed!</color>");
        //    hub.OnReconnecting += (hub, arg2) => Debug.Log($"<color=green>Hub {hub.Uri} reconnecting</color>");
        //    hub.OnReconnected += (hub) => Debug.Log($"<color=green>Hub {hub.Uri} reconnected</color>");
        //    hub.OnTransportEvent += (hub, transp, events) => Debug.Log($"<color=green>Hub {hub.Uri} {transp.TransportType}</color> event: <color=green>{events}</color>");
        //    hub.OnMessage += OnHubMessageReceived;
        //}

        //private bool OnHubMessageReceived(HubConnection connection, Message message)
        //{
        //    if (message.type == MessageTypes.Invocation)
        //        Debug.Log($"<color=orange>Hub message</color> <color=yellow>{message.target}</color> received:{JsonConvert.SerializeObject(message)}");
        //    return true;
        //}
    }
}

/* Copyright: Made by Appfox */