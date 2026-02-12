using System.Collections.Generic;

namespace Kborod.DomainModel.Auth
{
    public class AuthService
    {
        public IReadOnlyList<ProviderType> AvailableProviders => new List<ProviderType>() { ProviderType.EmailPassword };
    }
}
