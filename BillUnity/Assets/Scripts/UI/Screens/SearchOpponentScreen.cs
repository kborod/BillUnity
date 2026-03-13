using Cysharp.Threading.Tasks;
using Kborod.BilliardCore.Enums;
using Kborod.Services.UIScreenManager;
using Kborod.SharedDto;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kborod.UI.Screens
{
    [UIScreen("UI/Screens/SearchOpponentScreen.prefab", true)]
    public class SearchOpponentScreen : UIScreenBase
    {
        [SerializeField] private Button _cancelButton;
        [SerializeField] private TMP_Text _gameTypeText;
        [SerializeField] private TMP_Text _betTypeText;
        [SerializeField] private TMP_Text _foundOppText;

        private Action _cancelClickCallback;

        private void Start()
        {
            _cancelButton.onClick.AddListener(CancelClickHandler);
        }

        public void Setup(GameType gameType, BetType betType, Action cancelClickCallback)
        {
            _gameTypeText.text = gameType.ToString();
            _betTypeText.text = betType.ToString();
            _cancelClickCallback = cancelClickCallback;
        }

        public async UniTask ShowFoundOpponent(UserProfile oppProfile)
        {
            _foundOppText.text = $"Found opponent: {oppProfile.Name}";
            await UniTask.Delay(TimeSpan.FromSeconds(3));
        }

        private void CancelClickHandler()
        {
            _cancelClickCallback?.Invoke();
        }
    }
}
