using UnityEngine;
using UnityEngine.UI;

namespace Kborod.UI.Screens.SpinUI
{
    public class SpinPanel : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private RectTransform ball;
        [SerializeField] private RectTransform point;
        [SerializeField] private ChooseSpinPopup popup;

        public float SpinX { get; private set; } = 0;
        public float SpinY { get; private set; } = 0;

        private float ballRadius;
        private float maxSpin = 1f;

        private void Awake()
        {
            button.onClick.AddListener(() => popup.Show(SpinX, SpinY, maxSpin, SpinChangedHandler));

            ballRadius = ball.GetComponent<RectTransform>().sizeDelta.x * 0.33f;
            RefreshUI();
        }

        public void SetMaxSpin(float maxSpin)
        {
            this.maxSpin = maxSpin;
            Clear();
        }

        public void Clear()
        {
            SpinX = SpinY = 0;
            RefreshUI();
        }

        private void SpinChangedHandler(float spinX, float spinY)
        {
            SpinX = spinX;
            SpinY = spinY;
            RefreshUI();
        }

        private void RefreshUI()
        {
            point.localPosition = new Vector2(SpinX * ballRadius, SpinY * ballRadius);
        }
    }
}
