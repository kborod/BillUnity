using System;

namespace Kborod.Services.ServerHTTPCommunication
{
    public interface IHttpTransport
    {
        public void SendMessage(
            HttpMessageBase msg,
            string address,
            string tokenOrNull,
            Action<HttpMessageBase, string> successCallback,
            Action<HttpMessageBase, string> errorCallback,
            Action<bool> connectionErrorCallback);
    }
}

/* Copyright: Made by Appfox */