using Kborod.BilliardCore;
using Kborod.Extensions;
using Kborod.MatchManagement;
using System;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens.Table.BallsMove
{
    public class BallReplacer : MonoBehaviour
    {
        public event Action<Ball> ballReplaced;
        public event Action PointerDownEvent;
        public event Action PointerUpEvent;

        [SerializeField] private MoveIcon icon;
        [SerializeField] private RectTransform panel;
        [SerializeField] private Camera tableCamera;
        [SerializeField] private Transform ballsRoot;

        [Inject] private IMatchServices _matchServices;
        private MatchBase _match => _matchServices.Match;
        private IEngineForUI _engine => _matchServices.EngineForUI;
        private MyInput _myShotInput => _matchServices.MyInput;

        private int _ballNum;
        private bool _onlyKitchen;

        private void Start()
        {
            icon.PointerDownEvent += PointerDownHandler;
            icon.PointerUpEvent += PointerUpHandler;
            icon.DragEvent += DragHandler;

            _match.StateChanged += StateChangedHandler;
            _match.AimInfoReceived += AimInfoReceivedHandler;

            RefreshByState(_match.State);
        }

        private void OnDestroy()
        {
            icon.PointerDownEvent -= PointerDownHandler;
            icon.PointerUpEvent -= PointerUpHandler;
            icon.DragEvent -= DragHandler;

            _match.StateChanged -= StateChangedHandler;
            _match.AimInfoReceived -= AimInfoReceivedHandler;
        }

        private void StateChangedHandler(MatchState state)
        {
            RefreshByState(state);
        }

        private void RefreshByState(MatchState state)
        {
            if (state == MatchState.PrepeareTurn && _myShotInput.CanIManageTurningPlayer && _match.TurnSettings.CanMoveBall.HasValue)
            {
                gameObject.SetActive(true);
                _ballNum = _match.TurnSettings.CanMoveBall.Value;
                _onlyKitchen = _match.TurnSettings.MoveOnlyInKitchen;
                icon.Show();
                RefreshIconPosition();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        private void AimInfoReceivedHandler(AimInfo info)
        {
            if (info.IsBallMovingNow)
                RefreshIconPosition();

            icon.Hide();
        }

        private void PointerDownHandler()
        {
            _myShotInput.MoveCueBall(null, null);
        }

        private void PointerUpHandler()
        {
            _myShotInput.MoveCueBallCompleted();
        }

        private void DragHandler(Vector2 pointerWorldPosition)
        {
            Vector2 worldPoint = tableCamera.ScreenToWorldPoint(pointerWorldPosition);
            Vector2 modelPoint = ballsRoot.InverseTransformPoint(worldPoint) / Config.MODEL_COORD_TO_WORLD_KOEF;

            _myShotInput.MoveCueBall(modelPoint.x, modelPoint.y);
        }

        private void RefreshIconPosition()
        {
            var worldBallPosition = ballsRoot.TransformPoint(_engine.Balls[_ballNum].v.p0.ToVector2() * Config.MODEL_COORD_TO_WORLD_KOEF);
            icon.transform.position = tableCamera.WorldToScreenPoint(worldBallPosition);
        }

        private void RefreshPanel()
        {
            panel.gameObject.SetActive(false);

            //var posLefBottomWorld = ballsRoot.TransformPoint(Config.leftBorderX * Config.MODEL_COORD_TO_WORLD_KOEF, Config.bottomBorderY * Config.MODEL_COORD_TO_WORLD_KOEF, 0);
            //var posRightTopWorld = ballsRoot.TransformPoint((onlyKitchen ? Config.headLineX : Config.rightBorderX) * Config.MODEL_COORD_TO_WORLD_KOEF, Config.topBorderY * Config.MODEL_COORD_TO_WORLD_KOEF, 0);

            //var posLefBottomScreenPoint = tableCamera.WorldToScreenPoint(posLefBottomWorld);
            //var posRightTopScreenPoint = tableCamera.WorldToScreenPoint(posRightTopWorld);

            //RectTransformUtility.ScreenPointToLocalPointInRectangle(panel.parent.transform as RectTransform, posLefBottomScreenPoint, null, out var posLefBottomLocalPoint);
            //RectTransformUtility.ScreenPointToLocalPointInRectangle(panel.parent.transform as RectTransform, posRightTopScreenPoint, null, out var posRightTopLocalPoint);

            //panel.sizeDelta = posRightTopLocalPoint - posLefBottomLocalPoint;
            ////panel.anchoredPosition = (posRightTopLocalPoint + posLefBottomLocalPoint) / 2;
        }
    }
}
