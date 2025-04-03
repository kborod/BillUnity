using Kborod.BilliardCore;
using System;
using UnityEngine;
using UnityEngine.UI;

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
        

        private Engine engine;
        private int ballNum;
        private bool onlyKitchen;

        private void Start()
        {
            icon.PointerDownEvent += PointerDownHandler;
            icon.PointerUpEvent += PointerUpHandler;
            icon.DragEvent += DragHandler;
        }

        private void OnDestroy()
        {
            icon.PointerDownEvent -= PointerDownHandler;
            icon.PointerUpEvent -= PointerUpHandler;
            icon.DragEvent -= DragHandler;
        }

        private void PointerDownHandler()
        {
            PointerDownEvent?.Invoke();
        }

        private void PointerUpHandler()
        {
            PointerUpEvent?.Invoke();
        }

        private void DragHandler(Vector2 pointerWorldPosition)
        {
            Vector2 worldPoint = tableCamera.ScreenToWorldPoint(pointerWorldPosition);
            Vector2 modelPoint = ballsRoot.InverseTransformPoint(worldPoint) / Config.MODEL_COORD_TO_WORLD_KOEF;

            var moved = engine.ReplaceBall(ballNum, modelPoint.x, modelPoint.y, onlyKitchen);
            if (moved)
            {
                ballReplaced?.Invoke(engine.balls[ballNum]);
                RefreshIcon();
            }
        }

        public void Show(Engine engine, bool onlyKitchen, int ballNum = 0)
        {
            gameObject.SetActive(true);
            icon.Show();

            this.engine = engine;
            this.ballNum = ballNum;
            this.onlyKitchen = onlyKitchen;

            RefreshIcon();
            RefreshPanel();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void RefreshIcon()
        {
            var worldBallPosition = ballsRoot.TransformPoint(engine.balls[ballNum].v.p0 * Config.MODEL_COORD_TO_WORLD_KOEF);
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
