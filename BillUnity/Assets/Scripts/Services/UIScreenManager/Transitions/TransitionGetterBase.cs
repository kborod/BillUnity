using System;
using UnityEngine;

namespace Kborod.Services.UIScreenManager.Transitions
{
    public abstract class TransitionGetterBase : MonoBehaviour
    {
        public abstract ITransition Transition { get; }
    }
}

/* Copyright: Made by Appfox */