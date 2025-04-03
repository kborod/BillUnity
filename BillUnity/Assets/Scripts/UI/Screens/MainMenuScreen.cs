using Kborod.Loader;
using Kborod.Services.UIScreenManager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens
{
    [UIScreen("UI/Screens/MainMenuScreen.prefab", true)]
    public class MainMenuScreen : UIScreenBase
    {
        [SerializeField] private Button trainingButton;

        [Inject] private AppProcessor _appProcessor;

        private void Start()
        {
            trainingButton.onClick.AddListener(OpenTrainingMatch);
        }

        private async void OpenTrainingMatch()
        {
            await _appProcessor.Match();
        }
    }
}
