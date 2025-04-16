using Kborod.MatchManagement;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens.Table.TopPanel
{
    public class BallIconItem : MonoBehaviour
    {
        [SerializeField] private Image icon;

        [Inject] private BallsSO _ballsSO;

        private void Awake()
        {
            icon.gameObject.SetActive(false);
        }

        public void SetBall(int ballNum, GameType gameType)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = _ballsSO.GetBall(ballNum, gameType).Icon;
        }
    }
}
