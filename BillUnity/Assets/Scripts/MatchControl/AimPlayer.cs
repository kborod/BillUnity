using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using System;
using System.Threading;
using UnityEngine;

namespace Kborod.MatchManagement.Control
{
    public class AimPlayer
    {
        private CancellationTokenSource _aimPlayingCts;

        public async UniTask Play(AimInfo aimInfo, MatchBase match, float duration, bool beforeMakeShot)
        {
            _aimPlayingCts?.Cancel();
            _aimPlayingCts = new CancellationTokenSource();
            
            try
            {
                var origAim = aimInfo;

                var speed = beforeMakeShot ? 5f : 1 / duration;
                var progress = 0f;

                if (aimInfo.CueBall.HasValue && aimInfo.CueBallXraw.HasValue && aimInfo.CueBallYraw.HasValue
                    && (aimInfo.CueBallXraw != match.EngineForUI.Balls[aimInfo.CueBall.Value].X.Raw 
                    || aimInfo.CueBallYraw != match.EngineForUI.Balls[aimInfo.CueBall.Value].Y.Raw))
                {
                    var inx = new Fixed64(aimInfo.CueBallXraw.Value);
                    var iny = new Fixed64(aimInfo.CueBallYraw.Value);
                    var bx = match.EngineForUI.Balls[aimInfo.CueBall.Value].X;
                    var by = match.EngineForUI.Balls[aimInfo.CueBall.Value].Y;


                    var fromX = match.EngineForUI.Balls[aimInfo.CueBall.Value].X.ToFloat();
                    var fromY = match.EngineForUI.Balls[aimInfo.CueBall.Value].Y.ToFloat();
                    var toX = new Fixed64(aimInfo.CueBallXraw.Value).ToFloat();
                    var toY = new Fixed64(aimInfo.CueBallYraw.Value).ToFloat();
                    while (progress < 1)
                    {
                        progress += Time.deltaTime * speed;
                        var x = Mathf.Lerp(fromX, toX, progress);
                        var y = Mathf.Lerp(fromY, toY, progress);
                        aimInfo.CueBallXraw = Fixed64.FromFloat(x).Raw;
                        aimInfo.CueBallYraw = Fixed64.FromFloat(y).Raw;
                        aimInfo.IsBallMovingNow = true;
                        match.ChangeAimInfo(aimInfo);
                        await UniTask.NextFrame(_aimPlayingCts.Token);
                    }

                    origAim.IsBallMovingNow = false;
                    match.ChangeAimInfo(origAim);
                }
                else if (match.AimInfo.DirectionXraw != aimInfo.DirectionXraw 
                    || match.AimInfo.DirectionYraw !=  aimInfo.DirectionYraw
                    || match.AimInfo.PowerRaw != aimInfo.PowerRaw)
                {
                    var fromX = new Fixed64(match.AimInfo.DirectionXraw).ToFloat();
                    var fromY = new Fixed64(match.AimInfo.DirectionYraw).ToFloat();
                    var toX = new Fixed64(aimInfo.DirectionXraw).ToFloat();
                    var toY = new Fixed64(aimInfo.DirectionYraw).ToFloat();
                    var fromPower = new Fixed64(match.AimInfo.PowerRaw).ToFloat();
                    var toPower = new Fixed64(aimInfo.PowerRaw).ToFloat();
                    while (progress < 1)
                    {
                        progress += Time.deltaTime * speed;
                        var x = Mathf.Lerp(fromX, toX, progress);
                        var y = Mathf.Lerp(fromY, toY, progress);
                        aimInfo.DirectionXraw = Fixed64.FromFloat(x).Raw;
                        aimInfo.DirectionYraw = Fixed64.FromFloat(y).Raw;
                        var power = Mathf.Lerp(fromPower, toPower, progress);
                        aimInfo.PowerRaw = Fixed64.FromFloat(power).Raw;
                        match.ChangeAimInfo(aimInfo);
                        await UniTask.NextFrame(_aimPlayingCts.Token);
                    }
                    match.ChangeAimInfo(origAim);
                }

                if (beforeMakeShot)
                {
                    if (match.AimInfo.PowerRaw != origAim.PowerRaw)
                    {
                        var from = new Fixed64(match.AimInfo.PowerRaw).ToFloat();
                        var to = new Fixed64(origAim.PowerRaw).ToFloat();

                        speed = 5f;
                        progress = 0;
                        while (progress < 1)
                        {
                            progress += Time.deltaTime * speed;
                            var power = Mathf.Lerp(from, to, progress);
                            aimInfo.PowerRaw = Fixed64.FromFloat(power).Raw;
                            match.ChangeAimInfo(aimInfo);
                            await UniTask.NextFrame(_aimPlayingCts.Token);
                        }
                    }

                    speed = 10f;
                    progress = 0;

                    while (progress < 1)
                    {
                        progress += Time.deltaTime * speed;
                        var power = Mathf.Lerp(new Fixed64(origAim.PowerRaw).ToFloat(), 0, progress);
                        aimInfo.PowerRaw = Fixed64.FromFloat(power).Raw;
                        match.ChangeAimInfo(aimInfo);
                        await UniTask.NextFrame(_aimPlayingCts.Token);
                    }

                }
            }
            catch (OperationCanceledException) { }
        }

        public void Stop()
        {
            _aimPlayingCts?.Cancel();
            _aimPlayingCts = null;
        }
    }
}
