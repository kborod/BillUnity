namespace Kborod.MatchManagement
{
    public class Player
    {
        public string Id { get; private set; }
        public string Name { get; private set; }

        public Player(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
