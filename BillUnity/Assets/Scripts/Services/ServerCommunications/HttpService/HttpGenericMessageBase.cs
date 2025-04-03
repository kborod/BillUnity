using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Kborod.Services.ServerHTTPCommunication
{
    public abstract class HttpGenericMessageBase<TRequest, TResponse> : HttpMessageBase
    {
        private readonly MessageListener<TResponse> _messageListener = new MessageListener<TResponse>();
        public override string ResponseText
        {
            get => _responseText;
            set => SetResponse(value);
        }

        public override string ResponseErrorText
        {
            get => _errorText;
            set => SetError(value);
        }

        private TRequest _request;
        private string _responseText;
        private string _errorText;

        public HttpGenericMessageBase(TRequest request)
        {
            _request = request;
        }

        public HttpGenericMessageBase<TRequest, TResponse> SetResponseHandler(Action<TResponse> responseDelegate)
        {
            _messageListener.SetResponseHandler(responseDelegate);
            return this;
        }

        public HttpGenericMessageBase<TRequest, TResponse> SetErrorHandler(Action<string> errorDelegate)
        {
            _messageListener.SetErrorHandler(errorDelegate);
            return this;
        }

        public override string JsonRequest => JsonConvert.SerializeObject(_request, Formatting.None, JsonSerializerSettings);

        private void SetResponse(string text)
        {
            _responseText = text;
            if (string.IsNullOrEmpty(text)) text = "{}";
            TResponse response;
            try
            {
                response = JsonConvert.DeserializeObject<TResponse>(text, JsonSerializerSettings);
            }
            catch (Exception e)
            {
                Debug.LogError($"Parsing json fail: {typeof(TResponse)}: {text} error : {e}");
                SetError("Response parsing error");
                return;
            }

            try
            {
                InitializeResponse(response);
                _messageListener.CallResponse(response);
            }
            catch (Exception e)
            {
                Debug.LogError($"{GetType().Name} response processing fail ({e})");
            }
        }

        private void SetError(string text)
        {
            _errorText = text;
            try
            {
                _messageListener.CallError(text);
            }
            catch (Exception e)
            {
                Debug.LogError($"{GetType().Name} error processing fail ({e})");
            }
        }

        protected virtual void InitializeResponse(TResponse response) { }
    }

    public class MessageListener<T>
    {
        private event Action<string> errorhandler;
        private event Action<T> responseHandler;

        public void CallError(string text)
        {
            errorhandler?.Invoke(text);
        }

        public void CallResponse(T response)
        {
            responseHandler?.Invoke(response);
        }

        public MessageListener<T> SetResponseHandler(Action<T> responseDelegate)
        {
            responseHandler += responseDelegate;
            return this;
        }

        public MessageListener<T> SetErrorHandler(Action<string> errorDelegate)
        {
            errorhandler += errorDelegate;
            return this;
        }
    }
}

/* Copyright: Made by Appfox */