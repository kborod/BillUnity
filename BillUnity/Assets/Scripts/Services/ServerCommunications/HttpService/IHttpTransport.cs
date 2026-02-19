using System;

namespace Kborod.Services.ServerCommunication
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