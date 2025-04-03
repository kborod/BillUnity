using Cysharp.Threading.Tasks;
using Kborod.Services.UIScreenManager.Transitions;

namespace Kborod.Services.UIScreenManager
{

    public class TransitionManager
    {
        public bool TransitionNow { get; private set; }

        public async UniTask WaitCurrentTransition()
        {
            await UniTask.WaitUntil(() => !TransitionNow);
        }

        public async UniTask SwitchScreen(ITransitionable prevOrNull, ITransitionable nextOrNull)
        {
            await SwitchScreen(prevOrNull?.Transition, nextOrNull?.Transition);
        }

        public async UniTask SwitchScreen(ITransition prevOrNull, ITransition nextOrNull)
        {
            TransitionNow = true;
            if (nextOrNull != null)
                await nextOrNull.Show(prevOrNull);
            else
                await prevOrNull.Hide();
            TransitionNow = false;
        }
    }
}

/* Copyright: Made by Appfox */