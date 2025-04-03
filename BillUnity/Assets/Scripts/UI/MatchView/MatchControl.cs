using Kborod.BilliardCore;
using Kborod.MatchManagement;
using Kborod.Services.UIScreenManager;
using Kborod.UI.Screens.SpinUI;
using Kborod.UI.Screens.Table;
using Kborod.UI.Screens.Table.BallsMove;
using Kborod.UI.Screens.Table.BallsRemove;
using Kborod.UI.Screens.Table.TopPanel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kborod.UI.Screens
{
    public class MatchControl : UIScreenBase
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


        private Match match;
        private Engine engine => match.Engine;

        private void Start()
        {
            cueOverlay.HitReady += CueHitReadyHandler;
            ballReplacer.PointerDownEvent += MoverPointerDownHandler;
            ballReplacer.PointerUpEvent += MoverPointerUpHandler;

            Init();
        }

        private void MoverPointerDownHandler()
        {
            cueOverlay.Hide();
        }

        private void MoverPointerUpHandler()
        {
            cueOverlay.Show(engine, resetDirection: false);
        }

        private void Init()
        {
            var p1 = new PoolEightPlayer("1", "Player1");
            var p2 = new PoolEightPlayer("2", "Player2");

            match = new Match(p1, p2);
            match.StateChanged += StateChangedHandler;

            tableView.Setup(TableType.Pool, engine);

            userPanelLeft.Setup(match, p1);
            userPanelRight.Setup(match, p2);

            if (match.State == MatchState.PrepeareTurn)
                StateChangedHandler(match.State);
        }

        private void StateChangedHandler(MatchState state)
        {
            if (state == MatchState.PrepeareTurn && match.CanIManageTurningPlayer)
            {
                var turnSettings = match.CurrTurnSettings;

                if (turnSettings.CanMoveBall.HasValue)
                    ballReplacer.Show(engine, turnSettings.MoveOnlyInKitchen, turnSettings.CanMoveBall.Value);

                cueOverlay.Show(engine);
            }
            else if (state == MatchState.Animation)
            {

            }
        }

        private void CueHitReadyHandler(Vector2 direction)
        {
            match.MakeShot(0, direction, spinPanel.SpinX, spinPanel.SpinY, AnimationTickHandler, AnimationCompleteHandler);

            cueOverlay.Hide();
            ballReplacer.Hide();
            spinPanel.Clear();
        }

        private void AnimationTickHandler(float deltaMS)
        {
            MovePocketedBallsToRemover();
            tableView.UpdateBallsPositions(deltaMS);

            void MovePocketedBallsToRemover()
            {
                foreach (var b in engine.balls.Where(b => b.needMoveToBallRemover))
                {
                    ballsRemover.AddBall(b, ballsRoot, false);
                    b.needMoveToBallRemover = false;
                }
            }
        }

        private void AnimationCompleteHandler(ShotResultData result)
        {
            if (result.ReturnedPocketedBalls.Count > 0)
            {
                result.ReturnedPocketedBalls.ForEach(b => ballsRemover.RemoveBall(b));
                tableView.UpdateBallsPositions();
            }
        }
    }
}
