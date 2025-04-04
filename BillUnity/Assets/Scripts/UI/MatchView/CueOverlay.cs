using Kborod.BilliardCore;
using System;
using UnityEngine;

namespace Kborod.UI.Screens
{
    public class CueOverlay : MonoBehaviour
    {
        public Action<Vector2, float> HitReady;

        [SerializeField] private Transform cueHolder;
        [SerializeField] private Transform cue;
        [SerializeField] private MouseInput mouseInput;
        [SerializeField] private ShotPowerSlider powersSlider;
        [SerializeField] private AimLines aimLines;
        [SerializeField] private Camera tableCamera;
        [SerializeField] private Transform ballsRoot;

        private const float CUE_MIN_X = -0.35f;
        private const float CUE_MAX_X = -1.2f;

        private Engine engine;
        private int ballNumber;

        private Ball bTmp;

        private Vector2 ballPosition => ballsRoot.TransformPoint(engine.balls[ballNumber].v.p0 * Config.MODEL_COORD_TO_WORLD_KOEF);
        private Vector2 currDirection => cueHolder.right.normalized;
        private Vector2 lastCursorScreenPosition;

        private void Start()
        {
            mouseInput.MouseDown += MouseDownHandler;
            mouseInput.MouseMove += MouseMoveHandler;
            mouseInput.MouseUp += MouseUpHandler;

            powersSlider.onValueChanged.AddListener(PowerChangedHandler);
            powersSlider.PowerSelected += PowerSelectedHandler;
        }

        private void OnDestroy()
        {
            mouseInput.MouseDown -= MouseDownHandler;
            mouseInput.MouseMove -= MouseMoveHandler;
            mouseInput.MouseUp -= MouseUpHandler;

            powersSlider.onValueChanged.RemoveAllListeners();
            powersSlider.PowerSelected -= PowerSelectedHandler;
        }

        public void Show(Engine engine, int ballNumber = 0, bool resetDirection = true)
        {
            this.engine = engine;
            this.ballNumber = ballNumber;

            var ballPosition = this.ballPosition;

            bTmp = new Ball(ballNumber);
            bTmp.v.p0.x = engine.balls[ballNumber].v.p0.x;
            bTmp.v.p0.y = engine.balls[ballNumber].v.p0.y;

            cueHolder.transform.localPosition = ballPosition;

            if (resetDirection)
                cueHolder.rotation = Quaternion.Euler(0, 0, 0);

            cue.transform.localPosition = new Vector2(CUE_MIN_X, 0);

            UpdateAimLines();

            gameObject.SetActive(true);
            powersSlider.gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            powersSlider.gameObject.SetActive(false);
        }

        private void MouseDownHandler(Vector2 position)
        {
            lastCursorScreenPosition = position;
        }

        private void MouseMoveHandler(Vector2 delta)
        {
            Vector2 newScreenPosition = lastCursorScreenPosition + delta;
            Vector2 lastWorldPosition = tableCamera.ScreenToWorldPoint(lastCursorScreenPosition);
            Vector2 newWorldPosition = tableCamera.ScreenToWorldPoint(newScreenPosition);

            Vector2 a1 = lastWorldPosition - ballPosition;
            Vector2 a2 = newWorldPosition - ballPosition;

            var angle = Vector3.Angle(a1, a2);
            if (Vector3.Cross(a1, a2).z < 0)
                angle *= -1;

            //Debug.Log($"{delta} {lastCursorScreenPosition} {newScreenPosition} {angle}");

            cueHolder.Rotate(0, 0, angle);
            lastCursorScreenPosition = newScreenPosition;

            UpdateAimLines();
        }

        private void MouseUpHandler(Vector2 position)
        {
            //Debug.Log($"MouseUpHandler {position}");
        }



        private void PowerChangedHandler(float power)
        {
            cue.localPosition = new Vector2(Mathf.Lerp(CUE_MIN_X, CUE_MAX_X, power), 0);
        }

        private void PowerSelectedHandler(float power)
        {
            HitReady?.Invoke(currDirection * 300, power);
        }

        private void UpdateAimLines()
        {
            bTmp.v.vx = currDirection.x;
            bTmp.v.vy = currDirection.y;
            bTmp.v.updatePointsFromComponents();
            bTmp.v.makeVector();
            var aimData = engine.getAimObject(bTmp);
            aimLines.Show(aimData, ballsRoot);
        }
    }
}
