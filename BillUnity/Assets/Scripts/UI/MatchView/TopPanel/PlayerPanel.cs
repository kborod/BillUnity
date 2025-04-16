using Kborod.MatchManagement;
using TMPro;
using UnityEngine;

namespace Kborod.UI.Screens.Table.TopPanel
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text userName;
        [SerializeField] private CanvasGroup canvasGroup;

        Match match;
        Player player;

        public void Setup(Match match, Player player)
        {
            this.match = match;
            this.player = player;

            userName.text = player.Name;

            match.TurningPlayerChanged += RefreshByTurn;

            RefreshByTurn();
        }

        private void RefreshByTurn()
        {
            canvasGroup.alpha = match.TurningPlayer != player ? 0.3f : 1f;
        }
    }
}
