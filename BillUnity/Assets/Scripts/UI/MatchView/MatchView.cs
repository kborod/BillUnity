using Kborod.BilliardCore;
using Kborod.MatchManagement;
using Kborod.Services.Localization;
using Kborod.Services.Sound;
using Kborod.Services.UIScreenManager;
using Kborod.UI.Screens.SpinUI;
using Kborod.UI.Screens.Table;
using Kborod.UI.Screens.Table.BallsMove;
using Kborod.UI.Screens.Table.BallsRemove;
using Kborod.UI.Screens.Table.TopPanel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens
{
    public class MatchView : UIScreenBase
    {
        [SerializeField] private Transform ballPrefab;
        [SerializeField] private Transform ballShadowPrefab;
        [SerializeField] private Transform tablePrefab;
        [SerializeField] private Transform ballsRoot;
        [SerializeField] private List<Material> materials;
        [SerializeField] private CueOverlay cueOverlay;
        [SerializeField] private TableView tableView;
        [Space(10)]
        [SerializeField] private BallReplacer ballReplacer;
        [SerializeField] private BallsRemover ballsRemover;
        [SerializeField] private SpinPanel spinPanel;
        [Space(10)]
        [SerializeField] private PlayerPanel userPanelLeft;
        [SerializeField] private PlayerPanel userPanelRight;
        [Space(10)]
        [SerializeField] private PoolEightPocketedBallsPanel pocketedPanelLeft;
        [SerializeField] private PoolEightPocketedBallsPanel pocketedPanelRight;
        [Space(10)]
        [SerializeField] private MessagesOverlay matchMessages;

        [Inject] private SoundService _soundService;
        [Inject] private LocalizationService _localizationService;
        //[Inject] ScreensHelper _screensHelper;


        [Inject] private Match _match;
        [Inject] private EnginePlayer _enginePlayer;
        [Inject] private IEngineForUI _engineForUI;

        private MatchSound _matchSound;
        private void Awake()
        {
            var p1 = new PoolEightPlayer("1", "Player1");
            var p2 = new PoolEightPlayer("2", "Player2");

            _match.StartNew(p1, p2);
            _match.StateChanged += StateChangedHandler;
            _match.ShotCompleted += AnimationCompleteHandler;
            _enginePlayer.ShotTickCompleted += AnimationTickHandler;

            userPanelLeft?.Setup(_match, p1);
            userPanelRight?.Setup(_match, p2);

            pocketedPanelLeft?.Setup(_match, p1);
            pocketedPanelRight?.Setup(_match, p2);
        }

        private void Start()
        {
            cueOverlay.HitReady += CueHitReadyHandler;
            ballReplacer.PointerDownEvent += MoverPointerDownHandler;
            ballReplacer.PointerUpEvent += MoverPointerUpHandler;

            _matchSound = new MatchSound(_soundService);

            if (_match.State == MatchState.PrepeareTurn)
                StateChangedHandler(_match.State);
        }

        private void MoverPointerDownHandler()
        {
            cueOverlay.Hide();
        }

        private void MoverPointerUpHandler()
        {
            cueOverlay.Show(resetDirection: false);
        }

        private void StateChangedHandler(MatchState state)
        {
            if (state == MatchState.PrepeareTurn && _match.CanIManageTurningPlayer)
            {
                var turnSettings = _match.CurrTurnSettings;

                if (turnSettings.CanMoveBall.HasValue)
                    ballReplacer.Show(turnSettings.MoveOnlyInKitchen, turnSettings.CanMoveBall.Value);

                cueOverlay.Show();
            }
            else if (state == MatchState.Animation)
            {

            }
        }

        private void CueHitReadyHandler(Vector2 direction, float power)
        {
            cueOverlay.Hide();
            ballReplacer.Hide();
            spinPanel.Clear();

            _matchSound.PlayShot(power);

            _match.MakeShot(0, direction * power, spinPanel.SpinX, spinPanel.SpinY);
        }

        private void AnimationTickHandler(ShotTickResult tickResult)
        {
            MovePocketedBallsToRemover();

            void MovePocketedBallsToRemover()
            {
                foreach (var b in _engineForUI.Balls.Where(b => b.needMoveToBallRemover))
                {
                    ballsRemover.AddBall(b, ballsRoot, false);
                    b.needMoveToBallRemover = false;
                }
            }
            _matchSound.PlayPartyTickSounds(tickResult);
        }

        private void AnimationCompleteHandler(ShotResultData result)
        {
            if (result.ReturnedPocketedBalls.Count > 0)
            {
                result.ReturnedPocketedBalls.ForEach(b => ballsRemover.RemoveBall(b));
            }

            if (result.Foul != FoulType.None)
            {
                matchMessages.AddByLocalizeKey(_localizationService.GetIdOfEnum(result.Foul), OverlayMessageType.Error);
            }
        }
    }
}
