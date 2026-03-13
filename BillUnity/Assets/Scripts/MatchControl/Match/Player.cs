using Kborod.SharedDto;

namespace Kborod.MatchManagement
{
    public abstract class Player
    {
        public string Id => Profile.Id;
        public string Name => Profile.Name;
        public int Avatar => Profile.Avatar;
        public UserProfile Profile { get; private set; }

        protected Player(UserProfile profile)
        {
            Profile = profile;
        }
    }
}
