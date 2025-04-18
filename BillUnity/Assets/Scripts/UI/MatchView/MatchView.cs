using Cysharp.Threading.Tasks;
using Kborod.BilliardCore;
using Kborod.MatchManagement;
using Kborod.Services.UIScreenManager;
using Kborod.UI.Screens.SpinUI;
using Kborod.UI.Screens.Table.BallsMove;
using Kborod.UI.Screens.Table.BallsRemove;
using System;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens
{
    public class MatchView : UIScreenBase
    {
        [SerializeField] private Transform ballsRoot;
        [SerializeField] private CueOverlay cueOverlay;
        [Space(10)]
        [SerializeField] private BallReplacer ballReplacer;
        [SerializeField] private BallsRemover ballsRemover;
        [SerializeField] private SpinPanel spinPanel;

        [Inject] private MatchBase _match;

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

            
            _match.StateChanged += StateChangedHandler;
            _match.ShotCompleted += AnimationCompleteHandler;
            _match.ShotTickCompleted += AnimationTickHandler;

            cueOverlay.HitReady += CueHitReadyHandler;
            ballReplacer.PointerDownEvent += MoverPointerDownHandler;
            ballReplacer.PointerUpEvent += MoverPointerUpHandler;
        }

        private void Start()
        {
            if (_match.State == MatchState.PrepeareTurn)
                StateChangedHandler(_match.State);
        }

        private void OnDestroy()
        {
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
            cueOverlay.Hide();
            ballReplacer.Hide();

            _match.MakeShot(_selectedCueBallNum, direction, power, spinPanel.SpinX, spinPanel.SpinY);

            spinPanel.Clear();
        }

        private void AnimationTickHandler(ShotTickResult tickResult)
        {
            tickResult.PocketedBallsOrNull?.ForEach(b => ballsRemover.AddBall(b, ballsRoot, false));
        }

        private void AnimationCompleteHandler(ShotResultData result)
        {
            if (result.ReturnedPocketedBalls.Count > 0)
            {
                result.ReturnedPocketedBalls.ForEach(b => ballsRemover.RemoveBall(b));
            }
        }
    }
}
