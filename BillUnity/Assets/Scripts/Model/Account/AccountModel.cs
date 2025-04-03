using System;
using UnityEngine;

namespace Kborod.Model
{
    public abstract class AccountModel
    {
        public event Action LoggedIn;

        public string Name { get; protected set; }

        public abstract void Login(string name);

        protected void InvokeLoggedIn() =>
            LoggedIn?.Invoke();
    }
}
