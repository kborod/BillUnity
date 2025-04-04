using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;

namespace Kborod.Services.Sound
{
    public class SoundService : MonoBehaviour
    {
        [Inject] private AudioAssetsSO _audioAssets;
        [Inject] private AudioSourcesProvider _audioSourcesProvider;
        [Inject] private ISoundVolumeSettings _volumeSettings;

        private Dictionary<AudioSource, ClipData> _playerToClipVolume = new Dictionary<AudioSource, ClipData>();

        private void Start()
        {
            _volumeSettings.SoundSettingsChanged += RefreshVolume;
            //PlayBackgroundSound(SoundType.LoadingBG);
        }

        private void OnDestroy()
        {
            _volumeSettings.SoundSettingsChanged -= RefreshVolume;
        }

        public void PlaySound(SoundType soundType)
        {
            PlaySound(soundType, 1);
        }

        public void PlaySound(SoundType soundType, float volumeKoef)
        {
            var player = _audioSourcesProvider.GetSoundPlayer();
            PlaySoundWithPlayer(soundType, player, volumeKoef);
        }

        public void PlaySound3d(SoundType soundType, Vector3 worldPosition, Transform parent = null)
        {
            var player = _audioSourcesProvider.GetSoundPlayer3D(worldPosition, parent);
            PlaySoundWithPlayer(soundType, player);
        }

        public AudioSource PlaySound3dLooped(SoundType soundType, Vector3 worldPosition, Transform parent = null)
        {
            var player = _audioSourcesProvider.GetSoundPlayer3D(worldPosition, parent, true);
            PlaySoundWithPlayer(soundType, player);
            return player;
        }

        private void PlaySoundWithPlayer(SoundType soundsType, AudioSource player, float volumeKoef0_1 = 1)
        {
            var clip = _audioAssets.GetClip(soundsType);
            if (clip.Clip == null)
                return;
            if (GetVolumeByTag(clip.Tag) == 0)
                return;

            player.clip = clip.Clip;
            SetPlayerVolume(player, clip, volumeKoef0_1);
            player.Play();

            _playerToClipVolume[player] = clip;
        }

        public void PlayBackgroundSound(SoundType soundType)
        {
            var player = _audioSourcesProvider.GetBackgroundPlayer();
            var clip = _audioAssets.GetClip(soundType);
            if (clip.Clip == null)
            {
                player.Stop();
                return;
            }

            if (clip.Tag != SoundTag.background)
            {
                Debug.LogError("clip has no background tag");
                player.Stop();
                return;
            }

            player.clip = clip.Clip;
            _playerToClipVolume[player] = clip;
            SetPlayerVolume(player, clip);
        }

        private void RefreshVolume()
        {
            var nullPlayers = _playerToClipVolume.Select(pair => pair.Key).Where(player => player == null).ToArray();

            foreach (var player in nullPlayers)
                _playerToClipVolume.Remove(player);

            foreach (var pair in _playerToClipVolume)
                SetPlayerVolume(pair.Key, pair.Value);
        }

        private void SetPlayerVolume(AudioSource player, ClipData clipData, float volumeKoef0_1 = 1)
        {
            player.volume = clipData.Volume * GetVolumeByTag(clipData.Tag) * volumeKoef0_1;

            if (clipData.Tag == SoundTag.background)
            {
                if (player.volume == 0 && player.isPlaying)
                    player.Stop();
                else if (player.volume > 0 && !player.isPlaying)
                    player.Play();
            }
        }

        private float GetVolumeByTag(SoundTag tag)
        {
            return _volumeSettings.GetSoundVolume0_1(tag);
        }
    }
}