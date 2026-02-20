using Kborod.BilliardCore.Enums;
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

        public void SetBall(int ballNum, GameType gameType, float transparency = 1f)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = _ballsSO.GetBall(ballNum, gameType).Icon;
            SetTransparency0_1(transparency);
        }

        public void SetTransparency0_1(float value)
        {
            icon.color = new Color(1, 1, 1, value);
        }
    }
}
