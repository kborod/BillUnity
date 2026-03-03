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

        private MatchBase _match => _matchServices.Match;
        private MyInput _myInput => _matchServices.MyInput;

        private UniTimer _turnTimer = new UniTimer();

        private bool IsMyTurn => _match.TurningPlayer.Id == _accountModel.Id;

        public void Setup(StartMatchData startMatchData)
        {
            _match.ShotCompleted += ShotCompletedHandler;
            _messagingService.Subscribe<StartTurnResponseDto>(StartTurnReceived);
            _messagingService.Subscribe<AimInfoResponseDto>(OppAimInfoReceived);
            _messagingService.Subscribe<MakeShotResponseDto>(OppMakeShotReceived);

            _myInput.AimInfoChanged += MyAimInfoReceived;
            _myInput.ShotMade += MyMakeShotReceived;
        }

        public void Dispose()
        {
            _match.ShotCompleted -= ShotCompletedHandler;
        }

        private void StartTurnReceived(StartTurnResponseDto dto)
        {
            if (IsMyTurn)
            {
                var turnSecondsLeft = dto.StartTurnData.TurnEndTimestamp - _timeService.CurrTimestamp;
                _turnTimer.Start(turnSecondsLeft, TurnTimeIsOver).Forget();
            }

            _match.StartTurn(dto.StartTurnData);
        }

        private void OppAimInfoReceived(AimInfoResponseDto dto)
        {
            if (!IsMyTurn && _match.State == MatchState.PrepeareTurn)
                _match.ChangeAimInfo(dto.AimInfoData.AimInfo);
        }

        private void OppMakeShotReceived(MakeShotResponseDto dto)
        {
            if (!IsMyTurn && _match.State == MatchState.PrepeareTurn)
                _match.MakeShot(dto.MakeShotData.AimInfo);
        }

        private void ShotCompletedHandler(ShotResultByRules data)
        {
            var syncInfo = new SyncronizationInfo
            {
                MatchId = _match.Id,
                Balls = _match.EngineForUI.GetBallDatas(),
                TurningPlayerId = data.NextTurnPlayerId,
                WinPlayerId = data.WinUserIdOrNull
            };
            _messagingService.SendRequest(new ShotResultDto(syncInfo));
        }

        public void MyAimInfoReceived(AimInfo aimInfo)
        {
            if (!IsMyTurn || _match.State != MatchState.PrepeareTurn)
                return;

            _messagingService.SendRequest(new AimInfoDto(new AimInfoData(_match.Id, aimInfo)));
            _match.ChangeAimInfo(aimInfo);
        }

        public void MyMakeShotReceived(AimInfo aimInfo)
        {
            if (!IsMyTurn || _match.State != MatchState.PrepeareTurn)
                return;

            _turnTimer.Stop();
            _messagingService.SendRequest(new MakeShotDto(new MakeShotData(_match.Id, aimInfo)));
            _match.MakeShot(aimInfo);
        }

        private void TurnTimeIsOver()
        {
            if (IsMyTurn)
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
    }
}
