using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.BilliardCore.Rules;
using Kborod.DomainModel;
using Kborod.Services.ServerCommunication.AsyncServerMessaging;
using Kborod.Services.ServerTime;
using Kborod.SharedDto.AsyncServerMessaging.Messages;
using Kborod.Utils;
using System;
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

        private readonly TimeSpan SendAimMinPeriodSec = TimeSpan.FromSeconds(1.5f);

        private MatchBase _match => _matchServices.Match;
        private MyInput _myInput => _matchServices.MyInput;

        private UniTimer _turnTimer = new UniTimer();
        private UniTimer _aimSendTimer = new UniTimer();

        private bool _isMyTurn => _match.TurningPlayer.Id == _accountModel.Id;
        private long _lastAimSentTime;
        private bool _aimSendTimerWorking;

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
            if (!_isMyTurn && _match.State == MatchState.PrepeareTurn)
                _match.ChangeAimInfo(dto.AimInfoData.AimInfo);
        }

        private void OppMakeShotReceived(MakeShotResponseDto dto)
        {
            if (!_isMyTurn && _match.State == MatchState.PrepeareTurn)
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

        public void MyMakeShotReceived(AimInfo aimInfo)
        {
            if (!_isMyTurn || _match.State != MatchState.PrepeareTurn)
                return;

            _turnTimer.Stop();
            _aimSendTimer.Stop();
            _aimSendTimerWorking = false;

            _messagingService.SendRequest(new MakeShotDto(new MakeShotData(_match.Id, aimInfo)));
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
            var nextSendTimeSeconds = TimeSpan.FromSeconds(_lastAimSentTime) + SendAimMinPeriodSec;
            if (nextSendTimeSeconds < _timeService.CurrTimestampTimeSpan)
            {
                SendAim();
            }
            else if(!_aimSendTimerWorking)
            {
                _aimSendTimer.Start((float)(nextSendTimeSeconds - _timeService.CurrTimestampTimeSpan).TotalSeconds, SendAim).Forget();
                _aimSendTimerWorking = true;
            }
        }

        private void SendAim()
        {
            _messagingService.SendRequest(new AimInfoDto(new AimInfoData(_match.Id, _myInput.CurrentAimInfo)));
            _lastAimSentTime = _timeService.CurrTimestamp;
            _aimSendTimerWorking = false;
        }


        //TODO BORODIN разобраться
        private float GetMyCuePower() => 300;
        private float GetMyCueId() => 1;
        private float GetOppCuePower() => 300;
    }
}
