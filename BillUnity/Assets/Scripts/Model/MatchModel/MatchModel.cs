using Kborod.MatchManagement;
using Zenject;

namespace Kborod.Model
{
    public class MatchModel
    {
        public MatchBase Match { get; private set; }

        [Inject] DiContainer _diCcontainer;

        public void InitTrainingMatch()
        {

        }

        public void InitTwoPlayersMatch()
        {
            Match = _diCcontainer.Instantiate<MatchPoolEight>();
        }

        public void InitWithBot()
        {

        }
    }
}
