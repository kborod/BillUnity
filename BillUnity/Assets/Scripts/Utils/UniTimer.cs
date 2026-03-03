using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Kborod.Utils
{
    public class UniTimer : IDisposable
    {
        private CancellationTokenSource cts;

        public async UniTask Start(float delaySeconds, Action completeCallback)
        {
            await StartPeriodic(delaySeconds, completeCallback, null, 1);
        }

        public async UniTask StartPeriodic(float intervalSeconds, Action CompleteCallback, Action TickCallback, int repeatCount = -1, bool ignoreTimeScale = true)
        {
            cts?.Cancel();
            cts = new CancellationTokenSource();

            try
            {
                int count = 0;
                while (repeatCount < 0 || count < repeatCount)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(intervalSeconds),
                        ignoreTimeScale: ignoreTimeScale,
                        cancellationToken: cts.Token);

                    TickCallback?.Invoke();
                    count++;
                }

                CompleteCallback?.Invoke();
            }
            catch (OperationCanceledException) { }
        }

        public void Stop() => cts?.Cancel();

        public void Dispose()
        {
            Stop();
            cts?.Dispose();
        }
    }
}