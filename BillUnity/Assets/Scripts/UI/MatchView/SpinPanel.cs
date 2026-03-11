using Kborod.BilliardCore;
using Kborod.MatchManagement;
using Kborod.UI.Screens.Table;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens.SpinUI
{
    public class SpinPanel : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private RectTransform ball;
        [SerializeField] private RectTransform point;
        [SerializeField] private ChooseSpinPopup popup;


        [Inject] private MatchServices _matchServices;
        private MatchBase _match => _matchServices.Match;
        private MyInput _myShotInput => _matchServices.MyInput;

        private float _spinX => new Fixed64(_match.AimInfo.SpinXraw).ToFloat();
        private float _spinY => new Fixed64(_match.AimInfo.SpinYraw).ToFloat();

        private float ballRadius;
        private float maxSpin = 1f;

        private void Awake()
        {
            _match.AimInfoReceived += AimInfoReceivedHandler;

            button.onClick.AddListener(ClickHandler);

            ballRadius = ball.GetComponent<RectTransform>().sizeDelta.x * 0.33f;
            RefreshUI();
        }

        private void OnDestroy()
        {
            _match.AimInfoReceived -= AimInfoReceivedHandler;
        }

        public void SetMaxSpin(float maxSpin)
        {
            this.maxSpin = maxSpin;
        }

        private void AimInfoReceivedHandler(AimInfo info)
        {
            RefreshUI();
        }

        private void ClickHandler()
        {
            if (_match.State == MatchState.PrepeareTurn && _myShotInput.CanIManageTurningPlayer)
                popup.Show(_spinX, _spinY, maxSpin, SpinChangedHandler);
        }

        private void SpinChangedHandler(float spinX, float spinY)
        {
            _myShotInput.ChangeSpin(spinX, spinY);
        }

        private void RefreshUI()
        {
            point.localPosition = new Vector2(_spinX * ballRadius, _spinY * ballRadius);
        }
    }
}
