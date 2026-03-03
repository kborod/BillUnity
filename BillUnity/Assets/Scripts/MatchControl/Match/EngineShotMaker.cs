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

        private Engine _engine;

        public EngineShotMaker(Engine engine)
        {
            _engine = engine;
        }

        public async UniTaskVoid MakeShot(int ballNumber, float directionX, float directionY, float spinX, float spinY)
        {
            _engine.MakeShot(directionX, directionY, ballNumber, spinX, spinY);

            while (true)
            {
                var deltaMS = (int)(Time.deltaTime * 1000);
                if (deltaMS > 0)
                {
                    _engine.UpdateModel(deltaMS, out var tickResult, out var shotResultOrNull);
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
