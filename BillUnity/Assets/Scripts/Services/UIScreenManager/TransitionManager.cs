using Cysharp.Threading.Tasks;
using Kborod.Services.UIScreenManager.Transitions;
using UnityEngine;

namespace Kborod.Services.UIScreenManager
{

    public class TransitionManager
    {
        public bool IsWorkingNow { get; private set; }

        public async UniTask SwitchScreen(ITransitionable prevOrNull, ITransitionable nextOrNull)
        {
            await SwitchScreen(prevOrNull?.Transition, nextOrNull?.Transition);
        }

        public async UniTask SwitchScreen(ITransition prevOrNull, ITransition nextOrNull)
        {
            IsWorkingNow = true;

            if (nextOrNull != null)
                await nextOrNull.Show(prevOrNull);
            else
                await prevOrNull.Hide();

            IsWorkingNow = false;
        }

        public async UniTask WaitCurrentWork()
        {
            if (IsWorkingNow)
                await UniTask.WaitUntil(() => !IsWorkingNow);
        }
    }
}