using Kborod.BilliardCore;
using System;

namespace Kborod.MatchManagement
{
    public class NetworkShotResultHandler : IShotResultHandler
    {
        public event Action<StartTurnData> TurnStarted;
        public event Action<MatchOverData> MatchIsOver;

        public void IMadeShot(MakeShotData data)
        {

        }

        public void ShotAnimCompleted(SyncronizationInfo stateAfterShot)
        {

        }
    }
}
