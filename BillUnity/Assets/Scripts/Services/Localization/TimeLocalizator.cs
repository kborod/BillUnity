using System;
using UnityEngine;

namespace Kborod.Services.Localization
{
    public class TimeLocalizator : LocalizatorBase
    {
        [SerializeField] private TimeFieldFormat format = default;
        [SerializeField] private long seconds = default;

        public void SetTimeSpan(TimeSpan timeSpan)
        {
            this.seconds = (long)timeSpan.TotalSeconds;
            UpdateLocalization();
        }

        public void SetSeconds(long seconds)
        {
            this.seconds = seconds;
            UpdateLocalization();
        }

        protected override void UpdateLocalization()
        {
            if (inited == false) return;

            TextComponent.text = localization.GetTranslationOfTimePeriod(seconds, format);
        }
    }
}
/* Copyright: Made by Appfox */