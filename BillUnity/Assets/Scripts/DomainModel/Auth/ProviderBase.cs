using System;

namespace Kborod.DomainModel.Auth
{
    public abstract class ProviderBase
    {
        public abstract ProviderType ProviderType { get; }
    }
}
