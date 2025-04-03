using System;

namespace Kborod.Services.Sound
{
    public interface ISoundVolumeSettings
    {
        public event Action SoundSettingsChanged;

        public float GetSoundVolume0_1(SoundTag tag);
        
        
        public void SetSoundVolume0_1(SoundTag tag, float volume);
    }
}