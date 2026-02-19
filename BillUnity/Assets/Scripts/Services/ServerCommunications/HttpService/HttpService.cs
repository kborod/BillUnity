using Cysharp.Threading.Tasks;
using Kborod.Services.ServerCommunication.Token;
using System;
using System.Collections.Generic;
using Zenject;

namespace Kborod.Services.ServerCommunication
{
    public class HttpService
    {

        public event Action<HttpMessageBase> OnCurrentMessageChanged;
        public event Action<bool> OnConnectionError;

        [Inject] private ITokenService _tokenProvider;
        [Inject] private IHttpTransport _transport;

        private string _mainApiServerAddr = "https://localhost:7155/";

        private Queue<HttpMessageBase> _messagesQueue = new Queue<HttpMessageBase>();
        private bool _isWaitServerResponse;

        private string _currentToken;

        public async UniTaskVoid SendMessage(HttpMessageBase msg)
        {
            //TODO BORODIN подумать над этим
            _currentToken = await _tokenProvider.GetTokenOrNull();

            if (_isWaitServerResponse)
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
                _isWaitServerResponse = false;
                SetCurrentMessage(null);
            }
        }

        private async void ProcessMessage(HttpMessageBase msg)
        {
            SetCurrentMessage(msg);
            _isWaitServerResponse = true;
            var serverAddress = msg.TargetApi == TargetApi.MainApi ? _mainApiServerAddr : throw new NotImplementedException();
            var address = $"{serverAddress}{msg.ApiAddress}";

            _transport.SendMessage(
                    msg,
                    address,
                    _currentToken,
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