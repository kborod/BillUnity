using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Kborod.Services.ServerHTTPCommunication
{
    public class HttpService : MonoBehaviour
    {
        [SerializeField] private string _authServerAddr;
        [SerializeField] private string _gameAPIServerAddr;

        public event Action<HttpMessageBase> OnCurrentMessageChanged;
        public event Action<bool> OnConnectionError;

        [Inject] private ITokenProvider _tokenProvider;
        [Inject] private IHttpTransport _transport;

        private Queue<HttpMessageBase> _messagesQueue = new Queue<HttpMessageBase>();
        private bool isWaitServerResponse;

        public void SendMessage(HttpMessageBase msg)
        {
            _tokenProvider.RefreshTokenIfNeed();

            if (isWaitServerResponse)
                _messagesQueue.Enqueue(msg);
            else
                ProcessMessage(msg);
        }

        public void SendMessageWithoutQueue(HttpMessageBase msg)
        {
            ProcessMessage(msg);
        }

        private void RequestSuccessCallback(HttpMessageBase msg, string responseText)
        {
            msg.ResponseText = responseText;
            TryProcessNextMessageFromQueue();
        }

        private void RequestErrorCallback(HttpMessageBase msg, string errorText)
        {
            msg.ResponseErrorText = errorText;
            TryProcessNextMessageFromQueue();
        }

        private void ConnectionErrorCallback(bool isError)
        {
            OnConnectionError?.Invoke(isError);
        }

        private void TryProcessNextMessageFromQueue()
        {
            if (_messagesQueue.Count > 0)
            {
                ProcessMessage(_messagesQueue.Dequeue());
            }
            else
            {
                isWaitServerResponse = false;
                SetCurrentMessage(null);
            }
        }

        private void ProcessMessage(HttpMessageBase msg)
        {
            SetCurrentMessage(msg);
            isWaitServerResponse = true;
            var serverAddress = msg.TargetApi == TargetApi.Authorization ? _authServerAddr : _gameAPIServerAddr;
            var address = $"{serverAddress}{msg.ApiAddress}";

            _transport.SendMessage(
                    msg,
                    address,
                    _tokenProvider?.TokenOrNull,
                    RequestSuccessCallback,
                    RequestErrorCallback,
                    ConnectionErrorCallback
                );
        }

        private void SetCurrentMessage(HttpMessageBase message)
        {
            OnCurrentMessageChanged?.Invoke(message);
        }
    }
}

/* Copyright: Made by Appfox */