using Kborod.MatchManagement;
using TMPro;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens.Table.TopPanel
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text userName;
        [SerializeField] private GameObject turnOverlay;
        [SerializeField] private bool isPlayer1;

        [Inject] private MatchServices _matchServices;
        private MatchBase _match => _matchServices.Match;

        private Player _player => isPlayer1 ? _match.Player1 : _match.Player2;


        private void Start()
        {
            _match.TurningPlayerChanged += RefreshTurn;

            RefreshName();
            RefreshTurn();
        }

        private void OnDestroy()
        {
            _match.TurningPlayerChanged -= RefreshTurn;
        }

        private void RefreshName()
        {
            userName.text = _player.Name;
        }

        private void RefreshTurn()
        {
            turnOverlay.SetActive(_match.TurningPlayer != _player);
        }
    }
}
