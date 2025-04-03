using Zenject;

namespace Kborod.Services.UIScreenManager
{
    public abstract class UIScreenBaseWithSubscreens : UIScreenBase
    {
        [Inject] private TransitionManager _transitionManager;
        private UISubscreenBase _currSubscreen;

        protected async void SwitchSubscreen(UISubscreenBase to, bool instant = false)
        {
            if (_currSubscreen == to)
                return;
            var fromScreen = _currSubscreen;
            if (instant)
            {
                fromScreen?.gameObject.SetActive(false);
                to?.gameObject.SetActive(true);
            }
            else
            {
                await _transitionManager.WaitCurrentTransition();
                _ = _transitionManager.SwitchScreen(fromScreen, to);
            }
            _currSubscreen = to;
        }
    }
}

/* Copyright: Made by Appfox */