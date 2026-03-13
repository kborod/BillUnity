using Kborod.Services.LocalCache;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Kborod.Services.Sound
{
    public class SoundSettingsLocal: ISoundVolumeSettings
    {
        public event Action SoundSettingsChanged;

        [Inject] private PlayerPrefsService _playerPrefsService;

        private Dictionary<SoundTag, float> _volumes;
        private bool _mute;

        private void TryInit()
        {
            if (_volumes != null)
                return;
            _volumes = new Dictionary<SoundTag, float>();
            foreach (SoundTag tag in Enum.GetValues(typeof(SoundTag)))
            {
                _volumes.Add(tag, LoadSoundVolume0_1(tag));
            }
            _mute = LoadMuteState();
        }

        public float GetSoundVolume0_1(SoundTag tag)
        {
            TryInit();
            return _mute ? 0 : _volumes[tag];
        }

        public bool GetMuteState()
        {
            TryInit();
            return _mute;
        }

        public void SetSoundVolume0_1(SoundTag tag, float volume)
        {
            var clampedVolume = Mathf.Clamp01(volume);
            _playerPrefsService.Save(clampedVolume, GetTagPrefKey(tag));
            SoundSettingsChanged?.Invoke();
        }

        public void SetMuteState(bool mute)
        {
            _mute = mute;
            _playerPrefsService.Save(mute, PlayerPrefKey.VolumeMute);
            SoundSettingsChanged?.Invoke();
        }

        private bool LoadMuteState() => _playerPrefsService.LoadOrDefault(PlayerPrefKey.VolumeMute, false);

        private float LoadSoundVolume0_1(SoundTag tag)
        {
            return _playerPrefsService.LoadOrDefault(GetTagPrefKey(tag), 0.5f);
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