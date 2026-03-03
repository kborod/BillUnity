using System;
using UnityEngine;

namespace Kborod.DomainModel
{
    public abstract class AccountModel
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
    }
}
