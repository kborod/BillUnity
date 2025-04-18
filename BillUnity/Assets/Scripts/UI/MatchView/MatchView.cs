using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.MatchManagement;
using Kborod.Services.Localization;
using Kborod.Services.Sound;
using Kborod.Services.UIScreenManager;
using Kborod.UI.Screens.SpinUI;
using Kborod.UI.Screens.Table;
using Kborod.UI.Screens.Table.BallsMove;
using Kborod.UI.Screens.Table.BallsRemove;
using System;
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
        [SerializeField] private MessagesOverlay matchMessages;

        [Inject] private SoundService _soundService;
        [Inject] private LocalizationService _localizationService;
        //[Inject] ScreensHelper _screensHelper;


        [Inject] private MatchBase _match;
        [Inject] private IEngineForUI _engineForUI;

        private MatchSound _matchSound;

        private int _selectedCueBallNum;

        private void Awake()
        {
            var p1 = new PoolEightPlayer("1", "Player1");
            var p2 = new PoolEightPlayer("2", "Player2");

            if (_match is not MatchPoolEight)
            {
                throw new Exception("NotImplemented");
            }
            (_match as MatchPoolEight).StartNew(p1, p2);

            (_match as MatchPoolEight).BallTypesSelected += P8_BallTypeSelectedHandler;
            _match.StateChanged += StateChangedHandler;
            _match.ShotCompleted += AnimationCompleteHandler;
            _match.ShotTickCompleted += AnimationTickHandler;

            cueOverlay.HitReady += CueHitReadyHandler;
            ballReplacer.PointerDownEvent += MoverPointerDownHandler;
            ballReplacer.PointerUpEvent += MoverPointerUpHandler;
        }

        private void Start()
        {
            _matchSound = new MatchSound(_soundService);

            if (_match.State == MatchState.PrepeareTurn)
                StateChangedHandler(_match.State);
        }

        private void OnDestroy()
        {
            (_match as MatchPoolEight).BallTypesSelected -= P8_BallTypeSelectedHandler;
            _match.StateChanged -= StateChangedHandler;
            _match.ShotCompleted -= AnimationCompleteHandler;
            _match.ShotTickCompleted -= AnimationTickHandler;

            cueOverlay.HitReady -= CueHitReadyHandler;
            ballReplacer.PointerDownEvent -= MoverPointerDownHandler;
            ballReplacer.PointerUpEvent -= MoverPointerUpHandler;
        }

        private void MoverPointerDownHandler()
        {
            cueOverlay.Hide();
        }

        private void MoverPointerUpHandler()
        {
            cueOverlay.Show(_selectedCueBallNum, _match.CurrTurnSettings.BallsAvailableToAim, resetDirection: false);
        }

        private async void StateChangedHandler(MatchState state)
        {
            if (state == MatchState.PrepeareTurn && _match.CanIManageTurningPlayer)
            {
                var turnSettings = _match.CurrTurnSettings;

                await SelectCueBall();

                if (turnSettings.CanMoveBall.HasValue)
                    ballReplacer.Show(turnSettings.MoveOnlyInKitchen, turnSettings.CanMoveBall.Value);

                cueOverlay.Show(_selectedCueBallNum, _match.CurrTurnSettings.BallsAvailableToAim);
            }
            else if (state == MatchState.Animation)
            {

            }
        }

        private async UniTask SelectCueBall()
        {
            if (_match.CurrTurnSettings.BallsAvailableToSelectAsCueball.Count == 1)
            {
                _selectedCueBallNum = _match.CurrTurnSettings.BallsAvailableToSelectAsCueball[0];
            }
            else if (_match.CurrTurnSettings.BallsAvailableToSelectAsCueball.Count > 1)
            {
                await UniTask.NextFrame();
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception("No balls for select as cueball");
            }
        }

        private void CueHitReadyHandler(Vector2 direction, float power)
        {
            _matchSound.PlayShot(power);

            cueOverlay.Hide();
            ballReplacer.Hide();

            _match.MakeShot(_selectedCueBallNum, direction * power, spinPanel.SpinX, spinPanel.SpinY);

            spinPanel.Clear();
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

        private void P8_BallTypeSelectedHandler()
        {
            var ballType = (_match.TurningPlayer as PoolEightPlayer).BallType;
            matchMessages.AddByLocalizeKey(ballType == PoolBallType.Solid ? "MatchMsg.P8_SolidBallsSelected" : "MatchMsg.P8_StrippedBallsSelected", OverlayMessageType.Normal);
        }
    }
}
