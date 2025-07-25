using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using System;
using UnityEngine;
using Zenject;

namespace Kborod.MatchManagement
{
    public class EngineShotMaker
    {
        public event Action<ShotTickResult> ShotTickCompleted;
        public event Action<ShotResult> ShotCompleted;

        [Inject] private Engine Engine;

        public async void MakeShot(int ballNumber, float directionX, float directionY, float spinX, float spinY)
        {
            Engine.MakeShot(directionX, directionY, ballNumber, spinX, spinY);

            while (true)
            {
                var deltaMS = (int)(Time.deltaTime * 1000);
                if (deltaMS > 0)
                {
                    Engine.UpdateModel(deltaMS, out var tickResult, out var shotResultOrNull);
                    ShotTickCompleted?.Invoke(tickResult);
                    if (shotResultOrNull != null)
                    {
                        ShotCompleted?.Invoke(shotResultOrNull);
                        break;
                    }
                }
                await UniTask.NextFrame();
            }
        }
    }
}
