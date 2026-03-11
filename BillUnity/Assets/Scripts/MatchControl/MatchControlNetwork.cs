using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.BilliardCore.Rules;
using Kborod.DomainModel;
using Kborod.Services.ServerCommunication.AsyncServerMessaging;
using Kborod.Services.ServerTime;
using Kborod.SharedDto.AsyncServerMessaging.Messages;
using Kborod.Utils;
using UnityEngine;
using Zenject;

namespace Kborod.MatchManagement.Control
{
    public class MatchControlNetwork
    {
        [Inject] private MatchServices _matchServices;
        [Inject] private AccountModel _accountModel;
        [Inject] private IMessagingService _messagingService;
        [Inject] private TimeService _timeService;

        private readonly float SendAimMinPeriodSec = 1f;

        private MatchBase _match => _matchServices.Match;
        private MyInput _myInput => _matchServices.MyInput;

        private UniTimer _turnTimer = new UniTimer();
        private UniTimer _aimSendTimer = new UniTimer();

        private bool _isMyTurn => _match.TurningPlayer.Id == _accountModel.Id;

        private AimPlayer _aimPlayer = new AimPlayer();

        public void Setup(StartMatchData startMatchData)
        {
            _match.ShotCompleted += ShotCompletedHandler;
            _messagingService.Subscribe<StartTurnResponseDto>(StartTurnReceived);
            _messagingService.Subscribe<AimInfoResponseDto>(OppAimInfoReceived);
            _messagingService.Subscribe<MakeShotResponseDto>(OppMakeShotReceived);

            _myInput.AimInfoChanged += MyAimInfoReceived;
            _myInput.ShotMade += MyMakeShotReceived;

            _messagingService.SendRequest(new MatchInitedDto(_match.Id));
        }

        public void Dispose()
        {
            _match.ShotCompleted -= ShotCompletedHandler;
        }

        private void StartTurnReceived(StartTurnResponseDto dto)
        {
            if (_isMyTurn)
            {
                var turnSecondsLeft = dto.StartTurnData.TurnEndTimestamp - _timeService.CurrTimestamp;
                _turnTimer.Start(turnSecondsLeft, TurnTimeIsOver).Forget();
            }

            _match.StartTurn(dto.StartTurnData);
        }

        private void OppAimInfoReceived(AimInfoResponseDto dto)
        {
            if (_isMyTurn || _match.State != MatchState.PrepeareTurn)
                return;

            _ = _aimPlayer.Play(dto.AimInfoData.AimInfo, _match, SendAimMinPeriodSec, false);
        }

        private async void OppMakeShotReceived(MakeShotResponseDto dto)
        {
            if (_isMyTurn || _match.State != MatchState.PrepeareTurn)
                return;
            await _aimPlayer.Play(dto.MakeShotData.AimInfo, _match, SendAimMinPeriodSec, true);
            _match.MakeShot(dto.MakeShotData.AimInfo, GetOppCuePower());
        }

        private void ShotCompletedHandler(RulesShotResult data)
        {
            var syncInfo = new SynchronizationInfo
            {
                MatchId = _match.Id,
                RulesShotResult = data,
            };
            _messagingService.SendRequest(new ShotResultDto(syncInfo));
        }

        public void MyAimInfoReceived(AimInfo aimInfo)
        {
            if (!_isMyTurn || _match.State != MatchState.PrepeareTurn)
                return;

            TrySendAim();
            _match.ChangeAimInfo(aimInfo);
        }

        public async void MyMakeShotReceived(AimInfo aimInfo)
        {
            if (!_isMyTurn || _match.State != MatchState.PrepeareTurn)
                return;

            _turnTimer.Stop();

            _messagingService.SendRequest(new MakeShotDto(new MakeShotData(_match.Id, aimInfo)));

            await _aimPlayer.Play(aimInfo, _match, SendAimMinPeriodSec, true);

            _match.MakeShot(aimInfo, GetMyCuePower());
        }

        private void TurnTimeIsOver()
        {
            if (_isMyTurn)
            {
                //Отправляем переход хода сопернику, Ожидаем ответ начала нового хода.
                Debug.Log("Время моего хода истекло.");
                //_messagingService.SendRequest(new SkipShotDto());
                MyMakeShotReceived(_myInput.CurrentAimInfo);
            }
            else
            {
                //Ждем ответ от сервера
                Debug.Log("Время хода соперника истекло.");
            }
        }

        private void TrySendAim()
        {
            if (!_aimSendTimer.IsWorking)
            {
                _aimSendTimer.Start(SendAimMinPeriodSec, SendAim).Forget();
            }
        }

        private void SendAim()
        {
            _messagingService.SendRequest(new AimInfoDto(new AimInfoData(_match.Id, _myInput.CurrentAimInfo)));
        }


        //TODO BORODIN разобраться
        private int GetMyCuePower() => 300;
        private int GetOppCuePower() => 300;
    }
}
