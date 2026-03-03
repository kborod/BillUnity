using Kborod.BilliardCore;
using System;

namespace Kborod.MatchManagement
{
    public interface IShotResultHandler
    {
        public event Action<StartTurnData> TurnStarted;
        public event Action<MatchOverData> MatchIsOver;

        /// <summary> Я произвел удар </summary>
        public void IMadeShot(MakeShotData data);

        /// <summary> Воспроизведение удара завершено </summary>
        public void ShotAnimCompleted(SyncronizationInfo stateAfterShot);
    }
}
