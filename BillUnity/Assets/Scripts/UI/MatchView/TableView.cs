using Kborod.BilliardCore;
using Kborod.Extensions;
using Kborod.MatchManagement;
using Kborod.UI.Screens.Table.BallsMove;
using Kborod.UI.Screens.Table.BallsRemove;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens.Table
{
    public class TableView: MonoBehaviour
    {
        private const bool SHOW_DEBUG_POINTS = false;

        [SerializeField] private Transform floor;
        [SerializeField] private Transform shadow;
        [SerializeField] private Transform table;
        [SerializeField] private Transform ballsRoot;
        [Space(10)]
        [SerializeField] private Transform ballPrefab;
        [SerializeField] private Transform ballShadowPrefab;
        [SerializeField] private Transform debugPointPrefab;
        [Space(10)]
        [SerializeField] private BallsRemover ballsRemover;
        [SerializeField] private BallReplacer ballReplacer;

        [Inject] private BallsSO _ballsSO;
        [Inject] private MatchBase _match;
        [Inject] private IEngineForUI _engineForUI;

        //private TableParams tableParams = new TableParams();

        private Dictionary<int, Transform> balls;
        private Dictionary<int, Transform> ballShadows;

        private void Start()
        {
            ballsRemover.BallMoved += UpdateBallPosition;
            ballReplacer.ballReplaced += UpdateBallPosition;
            _match.ShotTickCompleted += ShotTickHandler;
            _match.ShotCompleted += ShotCompleteHandler;
            _match.AimInfoReceived += AimInfoReceivedHandler;

            CreateTableAndBalls();
            TryShowDebugPoints();
        }

        private void OnDestroy()
        {
            ballsRemover.BallMoved -= UpdateBallPosition;
            ballReplacer.ballReplaced -= UpdateBallPosition;
            _match.ShotTickCompleted -= ShotTickHandler;
            _match.ShotCompleted -= ShotCompleteHandler;
            _match.AimInfoReceived -= AimInfoReceivedHandler;
        }

        private void CreateTableAndBalls()
        {
            foreach (Transform child in ballsRoot)
            {
                GameObject.Destroy(child.gameObject);
            }

            balls = new Dictionary<int, Transform>();
            ballShadows = new Dictionary<int, Transform>();
            foreach (var b in _engineForUI.Balls)
            {
                var ball = Instantiate(ballPrefab, ballsRoot);
                ball.GetComponent<MeshRenderer>().material.mainTexture = _ballsSO.GetPoolBall(b.Number).Texture;
                balls.Add(b.Number, ball);

                var shadow = Instantiate(ballShadowPrefab, ballsRoot);
                ballShadows.Add(b.Number, shadow);
            }
            UpdateBallsPositions();
        }

        private void ShotTickHandler(ShotTickResult result)
        {
            UpdateBallsPositions(result.DeltaTimeMS);
        }

        private void ShotCompleteHandler(ShotResultData result)
        {
            UpdateBallsPositions();
        }

        private void AimInfoReceivedHandler(AimInfo info)
        {
            if (info.IsBallMovingNow)
            {
                var ball = _engineForUI.Balls[info.CueBall.Value];
                UpdateBallPosition(ball);
            }
        }

        private void UpdateBallsPositions(float deltaTime = 0)
        {
            foreach (var b in _engineForUI.Balls.Where(b => !b.IsRemoved))
            {
                UpdateBallPosition(b, deltaTime);
            }
        }

        private void UpdateBallPosition(Ball b)
        {
            UpdateBallPosition(b, 0);
        }

        private void UpdateBallPosition(Ball b, float deltaTime)
        {
            balls[b.Number].localPosition = new Vector3(
                    b.v.p0.x * Config.MODEL_COORD_TO_WORLD_KOEF,
                    b.v.p0.y * Config.MODEL_COORD_TO_WORLD_KOEF,
                    b.Zcoordinate);
            balls[b.Number].Rotate(new Vector3(b.vVertSpin.vy, -b.vVertSpin.vx), b.vVertSpin.len * deltaTime * 0.06f, Space.World);
            balls[b.Number].Rotate(new Vector3(0, 0, 1), -b.SideSpin * deltaTime * 0.12f, Space.World);

            //if (b.bNumber == 0 /*&& b.isRemoved*/)
            //    Debug.Log($"{b.v.vx} {b.v.vy} --- {b.vVertSpin.vx} {b.vVertSpin.vy}");


            ballShadows[b.Number].gameObject.SetActive(!b.IsRemoved);
            ballShadows[b.Number].localPosition = new Vector3(
                    b.v.p0.x * Config.MODEL_COORD_TO_WORLD_KOEF,
                    b.v.p0.y * Config.MODEL_COORD_TO_WORLD_KOEF,
                    Config.BALL_RAD_PX * Config.MODEL_COORD_TO_WORLD_KOEF);
        }

        private void TryShowDebugPoints()
        {
            if (SHOW_DEBUG_POINTS)
            {
                var counter = 0;
                foreach (var wall in _engineForUI.RealWalls)
                {
                    var tablePoint = Instantiate(debugPointPrefab, ballsRoot);
                    tablePoint.localPosition = wall.p0.ToVector2() * Config.MODEL_COORD_TO_WORLD_KOEF;
                    tablePoint.name = $"Point {counter}";
                }
            }
        }
    }

    public enum TableType
    {
        Pool
    }
}
