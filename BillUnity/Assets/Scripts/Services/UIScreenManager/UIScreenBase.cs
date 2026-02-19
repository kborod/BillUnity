using Kborod.Services.UIScreenManager.Transitions;
using System;
using UnityEngine;

namespace Kborod.Services.UIScreenManager
{
    public abstract class UIScreenBase : MonoBehaviour, ITransitionable
    {
        public event Action<UIScreenBase> OnCloseCalled;
        public event Action<UIScreenBase> OnReleaseCalled;

        public virtual ITransition Transition => GetTransition();

        private ITransition _transition;

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

        [ContextMenu("==CloseScreen==")]
        public virtual void Close()
        {
            OnCloseCalled?.Invoke(this);
        }

        [ContextMenu("==ReleaseScreen==")]
        public virtual void Release()
        {
            OnReleaseCalled?.Invoke(this);
        }
    }
}