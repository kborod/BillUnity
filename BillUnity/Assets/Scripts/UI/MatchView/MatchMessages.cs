using Kborod.BilliardCore.Enums;
using Kborod.BilliardCore.Rules;
using Kborod.MatchManagement;
using Kborod.Services.Localization;
using Kborod.Services.UIScreenManager;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens.Table
{
    public class MatchMessages : MonoBehaviour
    {
        [SerializeField] private MessagesOverlay matchMessages;
        
        [Inject] private MatchServices _matchServices;
        [Inject] private LocalizationService _localization;

        private MatchBase _match => _matchServices.Match;

        private MatchPoolEight _matchPoolEight;

        private void Start()
        {
            _matchPoolEight = _match as MatchPoolEight;

            _match.ShotCompleted += ShotCompletedHandler;

            if (_matchPoolEight != null)
                _matchPoolEight.BallTypesSelected += P8_BallTypeSelectedHandler;

        }

        private void OnDestroy()
        {
            _match.ShotCompleted -= ShotCompletedHandler;

            if (_matchPoolEight != null)
                _matchPoolEight.BallTypesSelected -= P8_BallTypeSelectedHandler;
        }



        private void ShotCompletedHandler(RulesShotResult data)
        {
            if (data.Foul != FoulType.None)
            {
                matchMessages.AddByLocalizeKey(_localization.GetIdOfEnum(data.Foul), OverlayMessageType.Error);
            }
        }

        private void P8_BallTypeSelectedHandler()
        {
            var ballType = (_match.TurningPlayer as PoolEightPlayer).BallType;
            matchMessages.AddByLocalizeKey(ballType == PoolBallType.Solid ? "MatchMsg.P8_SolidBallsSelected" : "MatchMsg.P8_StrippedBallsSelected", OverlayMessageType.Normal);
        }
    }
}
