using Kborod.SharedDto;

namespace Kborod.DomainModel
{
    public abstract class AccountModel
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public UserProfile GetProfile() => new UserProfile { Id = Id, Name = Name };
    }
}
