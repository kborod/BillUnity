using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using System;
using UnityEngine;

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

        public async UniTaskVoid MakeShot(AimInfo aimInfo, int cuePower)
        {
            _engine.MakeShot(
                new Fixed64(aimInfo.DirectionXraw) * new Fixed64(aimInfo.PowerRaw) * Fixed64.FromInt(cuePower),
                new Fixed64(aimInfo.DirectionYraw) * new Fixed64(aimInfo.PowerRaw) * Fixed64.FromInt(cuePower),
                aimInfo.CueBall.Value,
                new Fixed64(aimInfo.SpinXraw),
                new Fixed64(aimInfo.SpinYraw));

            var sumMs = 0f;

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

                sumMs += deltaMS;
                if (sumMs > 1000 * 60)
                    throw new Exception("Infinity cycle detected");

                await UniTask.NextFrame();
            }
        }
    }
}
