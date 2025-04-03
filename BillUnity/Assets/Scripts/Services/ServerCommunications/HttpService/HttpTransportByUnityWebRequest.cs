using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Kborod.Services.ServerHTTPCommunication
{
    public class HttpTransportByUnityWebRequest : MonoBehaviour, IHttpTransport
    {
        [SerializeField] private bool _verbose;
        private Action<HttpMessageBase, string> _successCallback;
        private Action<HttpMessageBase, string> _errorCallback;
        private Action<bool> _connectionErrorCallback;

        public void SendMessage(
            HttpMessageBase msg,
            string address,
            string tokenOrNull,
            Action<HttpMessageBase, string> successCallback,
            Action<HttpMessageBase, string> errorCallback,
            Action<bool> connectionErrorCallback)
        {
            _successCallback = successCallback;
            _errorCallback = errorCallback;
            _connectionErrorCallback = connectionErrorCallback;

            StartCoroutine(SendMessageCor(msg, address, tokenOrNull));
        }

        private IEnumerator SendMessageCor(HttpMessageBase msg, string address, string tokenOrNull)
        {
            long responseCode = -1;
            UnityWebRequest.Result result = UnityWebRequest.Result.InProgress;
            string responseText = "";
            bool connectionErrorCatched = false;

            while (true)
            {
                var data = msg.JsonRequest;

                using (var request = new UnityWebRequest(address, msg.MethodType))
                {
                    request.method = msg.MethodType;
                    request.downloadHandler = new DownloadHandlerBuffer();
                    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
                    request.SetRequestHeader("Content-Type", "application/json");
                    request.SetRequestHeader("Accept", "application/json");

                    if (!string.IsNullOrEmpty(tokenOrNull))
                        request.SetRequestHeader("Authorization", $"Bearer {tokenOrNull}");

                    if (_verbose)
                    {
                        string logMessageTitle = ApplyColorTag("[Sended Request]", "blue");
                        string logMessageName = ApplyColorTag(msg.GetType().Name, "yellow");
                        string logMessage = $"{logMessageTitle} ({logMessageName})  address: {address}\n message: {data}";
                        Debug.Log(logMessage);
                    }

                    yield return request.SendWebRequest();

                    result = request.result;
                    responseCode = request.responseCode;
                    responseText = request.downloadHandler.text;

                    if (_verbose)
                    {
                        string logMessageTitle;
                        if (responseCode == 0 || result == UnityWebRequest.Result.ProtocolError)
                            logMessageTitle = ApplyColorTag($"[Fail Response responseCode={responseCode}]", "red");
                        else if (result == UnityWebRequest.Result.Success)
                            logMessageTitle = ApplyColorTag($"[Success Response {responseCode}]", "green");
                        else
                            logMessageTitle = ApplyColorTag($"[Fail Response result={request.result} code={responseCode}]", "orange");

                        string logMessageName = ApplyColorTag(msg.GetType().Name, "yellow");

                        string logMessage = $"{logMessageTitle} ({logMessageName})  address: {address} \n response: {responseText}";
                        Debug.Log(logMessage);
                    }
                }

                if (responseCode != 0 && (result == UnityWebRequest.Result.Success || result == UnityWebRequest.Result.ProtocolError))
                {
                    break;
                }

                if (connectionErrorCatched == false)
                {
                    _connectionErrorCallback?.Invoke(true);
                    connectionErrorCatched = true;
                }

                var delay = 1;
                Debug.Log(ApplyColorTag($"There will be an attempt in {delay} seconds", "orange"));
                yield return new WaitForSeconds(delay);
            }

            if (connectionErrorCatched)
            {
                _connectionErrorCallback?.Invoke(false);
            }

            if (result == UnityWebRequest.Result.ProtocolError)
            {
                _errorCallback?.Invoke(msg, responseText);
            }
            else
            {
                _successCallback?.Invoke(msg, responseText);
            }
        }

        private string ApplyColorTag(string str, string color)
        {
            return $"<color={color}>{str}</color>";
        }
    }
}

/* Copyright: Made by Appfox */