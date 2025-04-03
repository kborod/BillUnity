using System;
using Kborod.Services.LocalCache;
using UnityEngine;
using Zenject;

namespace Kborod.Services.Sound
{
    public class SoundSettingsLocal: ISoundVolumeSettings
    {
        public event Action SoundSettingsChanged;

        [Inject] private PlayerPrefsService _playerPrefsService;

        public float GetSoundVolume0_1(SoundTag tag)
        {
            return _playerPrefsService.LoadOrDefault(GetTagPrefKey(tag), 0.5f);
        }

        public void SetSoundVolume0_1(SoundTag tag, float volume)
        {
            var clampedVolume = Mathf.Clamp01(volume);
            _playerPrefsService.Save(clampedVolume, GetTagPrefKey(tag));
            SoundSettingsChanged?.Invoke();
        }

        private PlayerPrefKey GetTagPrefKey(SoundTag tag) 
        {
            return tag switch
            {
                SoundTag.background => PlayerPrefKey.VolumeBackground,
                SoundTag.ui => PlayerPrefKey.VolumeInterface,
                _ => throw new NotImplementedException(),
            };
        }
    }
}