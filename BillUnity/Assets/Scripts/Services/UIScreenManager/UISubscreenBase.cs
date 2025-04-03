using Kborod.Services.UIScreenManager.Transitions;
using System;
using UnityEngine;

namespace Kborod.Services.UIScreenManager
{
    public abstract class UISubscreenBase : MonoBehaviour, ITransitionable
    {
        public virtual ITransition Transition => GetTransition();

        private UIScreenBase _rootScreen;
        private ITransition _transition;

        private void Awake()
        {
            _rootScreen = GetComponentInParent<UIScreenBase>();
            if (_rootScreen == null)
                throw new Exception($"{typeof(UIScreenBase)} not found in parents");
        }

        private ITransition GetTransition()
        {
            if (_transition == null)
            {
                var control = GetComponent<TransitionGetterBase>();
                if (control == null)
                    throw new Exception($"{typeof(TransitionGetterBase)} not found on screen {name}");
                _transition = control.Transition;
            }
            return _transition;
        }

        public virtual void Close()
        {
            _rootScreen.Close();
        }

        public virtual void Release()
        {
            _rootScreen.Release();
        }
    }
}

/* Copyright: Made by Appfox */