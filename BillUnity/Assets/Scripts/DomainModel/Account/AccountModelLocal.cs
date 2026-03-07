using System.Linq;
using Unity.Multiplayer.Playmode;

namespace Kborod.DomainModel.Implementations
{
    public class AccountModelLocal : AccountModel
    {
        public override string Id => CurrentPlayer.ReadOnlyTags().Contains("Player2") ? "2" : "1";
        public override string Name => CurrentPlayer.ReadOnlyTags().Contains("Player2") ? "User_2" : "User_1";
    }
}
