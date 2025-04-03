using Newtonsoft.Json;

namespace Kborod.Services.ServerHTTPCommunication
{
    public abstract class HttpMessageBase
    {
        public abstract string MethodType { get; }
        public abstract string ApiAddress { get; }
        public abstract TargetApi TargetApi { get; }
        public abstract string JsonRequest { get; }
        public abstract string ResponseText { get; set; }
        public abstract string ResponseErrorText { get; set; }

        public virtual string TextWhileSending => $"Loading : {GetType().Name}";

        protected virtual JsonSerializerSettings JsonSerializerSettings => new JsonSerializerSettings();
    }
}

/* Copyright: Made by Appfox */