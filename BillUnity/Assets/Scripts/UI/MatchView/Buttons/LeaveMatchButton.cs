using Kborod.MatchManagement;
using Kborod.Services.UIScreenManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens
{
    public class LeaveMatchButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [Inject] private ScreensHelper _screensHelper;
        [Inject] private MatchServices _matchServices;

        private string _leaveModalTextId = "Match.LeaveMatchModalText";

        private void Start()
        {
            _button.onClick.AddListener(ClickHandler);
        }

        private async void ClickHandler()
        {
            var m = await _screensHelper.OpenModal<DialogModal>();
            m.Setup(_leaveModalTextId, LeaveModalHandler);
        }

        private void LeaveModalHandler(bool result)
        {
            if (result)
                _matchServices.MyInput.Leave();

        }
    }
}
