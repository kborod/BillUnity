using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Kborod.UI.Screens
{
    public class ShotPowerSlider : Slider, IPointerUpHandler
    {
        public Action<float> PowerSelected;

        private const float MIN_POWER = 0.02f;

        //тригерится из EventTrigger, который висит на handle сдайдера
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (value >= MIN_POWER)
            {
                PowerSelected?.Invoke(value);
            }

            value = 0;
        }
    }
}
