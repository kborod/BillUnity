using Kborod.BilliardCore.Enums;
using Kborod.DomainModel;
using Kborod.Services.Localization;
using Kborod.Services.UIScreenManager;
using Kborod.SharedDto;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens
{
    [UIScreen("UI/Screens/MatchResultScreen.prefab", true)]
    public class MatchResultScreen : UIScreenBase
    {
        [SerializeField] private Button _toMenuButton;
        [SerializeField] private Button _replayButton;
        [SerializeField] private TMP_Text _gameTypeText;
        [SerializeField] private TMP_Text _betTypeText;
        [SerializeField] private TextLocalizator _resultText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _myNameText;
        [SerializeField] private TMP_Text _oppNameText;
        [SerializeField] private GameObject _iAmReadyPanel;
        [SerializeField] private GameObject _oppReadyPanel;
        [SerializeField] private GameObject _oppAfkPanel;

        [Inject] private AccountModel _accountModel;

        private Action _toMenuCallback;
        private Action _rematchCallback;

        private void Start()
        {
            _toMenuButton.onClick.AddListener(ToMenuClickHandler);
            _replayButton.onClick.AddListener(ReplayClickHandler);
        }

        public void Setup(GameType gameType, BetType betType, string winnerId, 
            int winnerScore, int loserScore, UserProfile oppProfile,
            Action toMenuCallback, Action rematchCallback)
        {
            _gameTypeText.text = gameType.ToString();
            _betTypeText.text = betType.ToString();
            
            var amIwinner = winnerId == _accountModel.Id;
            var myscore = amIwinner ? winnerScore : loserScore;
            var oppScore = amIwinner ? loserScore : winnerScore;

            _resultText.SetId(amIwinner ? "Match.WinMessage" : "Match.LoseMessage");
            _scoreText.text = $"{myscore} : {oppScore}";
            _myNameText.text = _accountModel.Name;
            _oppNameText.text = oppProfile.Name;

            _toMenuCallback = toMenuCallback;
            _rematchCallback = rematchCallback;
        }

        private void ReplayClickHandler()
        {
            _rematchCallback?.Invoke();
        }

        private void ToMenuClickHandler()
        {
            _toMenuCallback?.Invoke();
        }

        public void RefreshUI(bool isOppAfk, bool isOppReady, bool isIamReady)
        {
            _oppReadyPanel.SetActive(isOppAfk == false && isOppReady == true);
            _iAmReadyPanel.SetActive(isOppAfk == false && isIamReady == true);
            _replayButton.interactable = isOppAfk == false && isIamReady == false;
            _oppAfkPanel.SetActive(isOppAfk);
        }
    }
}
