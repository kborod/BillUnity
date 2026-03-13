using Cysharp.Threading.Tasks;
using Kborod.MatchManagement;
using Kborod.Services.ServerTime;
using Kborod.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens.Table.TopPanel
{
    public class TimePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _seconds;
        [SerializeField] private Image _scale;
        [SerializeField] private bool isPlayer1;

        [Inject] private MatchServices _matchServices;
        [Inject] private TimeService _timeService;


        private readonly UniTimer _timer = new UniTimer();
        private MatchBase _match => _matchServices.Match;

        private const float ScaleSeconds = 30f;

        private Player _player => isPlayer1 ? _match.Player1 : _match.Player2;


        private void Start()
        {
            _match.StateChanged += StateChanged;

            StateChanged(_match.State);
        }

        private void OnDestroy()
        {
            _match.StateChanged -= StateChanged;
            _timer.Dispose();
        }

        private void StateChanged(MatchState state)
        {
            if (state == MatchState.PrepeareTurn && _match.TurningPlayer == _player)
                _timer.StartPeriodic(0.2f, null, Refresh).Forget();
            else
                _timer.Stop();

            Refresh();
        }

        private void Refresh()
        {
            if (_match.State == MatchState.PrepeareTurn && _player == _match.TurningPlayer && _match.TurnEndTime < int.MaxValue)
            {
                var msecLeft = Mathf.Clamp(_match.TurnEndTime * 1000 - _timeService.CurrTimestampMs, 0, int.MaxValue);
                _scale.fillAmount = (ScaleSeconds * 1000 - msecLeft) / (ScaleSeconds * 1000) ;
                _seconds.text = msecLeft <= 0 ? "0" : ((int) (msecLeft / 1000f) + 1).ToString();
            }
            else
            {
                _scale.fillAmount = 0;
                _seconds.text = "";
            }
        }
    }
}
