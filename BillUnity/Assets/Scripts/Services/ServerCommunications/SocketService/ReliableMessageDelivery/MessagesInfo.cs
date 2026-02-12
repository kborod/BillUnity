using Best.SignalR;
using Best.SignalR.Encoders;
using Best.SignalR.Messages;
using Cysharp.Threading.Tasks;
using Kborod.Services.ServerCommunication;
using Newtonsoft.Json;
using System;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using Zenject;

namespace Kborod.Services.ServerCommunication.Sockets.ReliableMessageDelivery
{
    public class MessagesInfo
    {
        public int LastReceivedResponseNumber { get; private set; }
        public int NextRequestNumber => _nextRequestNumber++;

        private int _nextRequestNumber = 1;

        public void SetLastReceivedResponseNumber(int number)
        {
            LastReceivedResponseNumber = number;
        }


    }
}

/* Copyright: Made by Appfox */