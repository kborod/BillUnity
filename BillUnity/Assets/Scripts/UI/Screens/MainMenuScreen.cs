using Cysharp.Threading.Tasks;
using Kborod.BilliardCore.Enums;
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
        [SerializeField] private Button pvpButton;
        [SerializeField] private Button withBotButton;

        [Inject] private AppProcessor _appProcessor;

        private void Start()
        {
            trainingButton.onClick.AddListener(OpenTrainingMatch);
            pvpButton.onClick.AddListener(OpenPvpMatch);
        }

        private void OpenTrainingMatch()
        {
             _appProcessor.TrainingMatch().Forget();
        }

        private void OpenPvpMatch()
        {
            _appProcessor.PvpMatch(GameType.PoolEight, BetType.Table_1).Forget();
        }
    }
}
