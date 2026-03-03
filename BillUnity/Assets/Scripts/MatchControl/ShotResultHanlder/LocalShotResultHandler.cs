using Kborod.BilliardCore;
using System;
using Zenject;

namespace Kborod.MatchManagement
{
    public class LocalShotResultHandler : IShotResultHandler
    {
        public event Action<StartTurnData> TurnStarted;
        public event Action<MatchOverData> MatchIsOver;

        [Inject] private MatchServices _matchServices;

        public void IMadeShot(MakeShotData data)
        {
            //Для локала ничего не делаем
        }

        public void ShotAnimCompleted(SyncronizationInfo stateAfterShot)
        {
            if (stateAfterShot.WinPlayerId != null)
            {
                MatchIsOver?.Invoke(new MatchOverData
                {
                    MatchId = _matchServices.Match.Id,
                    WinPlayerId = stateAfterShot.WinPlayerId
                });
            }
            else
            {
                TurnStarted?.Invoke(new StartTurnData
                {
                    MatchId = _matchServices.Match.Id,
                    TurningPlayerId = stateAfterShot.TurningPlayerId,
                    TurnEndTimestamp = long.MaxValue
                });
            }
        }
    }
}
