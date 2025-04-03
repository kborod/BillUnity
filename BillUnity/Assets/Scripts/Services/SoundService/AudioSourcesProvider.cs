using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Kborod.Services.Sound
{
    public class AudioSourcesProvider : MonoBehaviour
    {
        [SerializeField] private AudioSource soundSourcePrefab;

        private AudioSource _backgroundPlayer;
        private List<AudioSource> _pool = new List<AudioSource>();

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public AudioSource GetBackgroundPlayer()
        {
            if (_backgroundPlayer == null)
            {
                _backgroundPlayer = CreateNewAudioSource();
                _backgroundPlayer.spatialBlend = 0;
                _backgroundPlayer.loop = true;
            }

            return _backgroundPlayer;
        }

        /// <summary>
        /// From pool
        /// </summary>
        public AudioSource GetSoundPlayer()
        {
            var audioSource = GetPlayerFromPool();
            audioSource.spatialBlend = 0;
            audioSource.transform.position = Vector3.zero;
            audioSource.transform.parent = this.transform;
            return audioSource;
        }

        /// <summary>
        /// From pool in the case of looped == false. Looped players are not pooled, and managed by client
        /// </summary>
        public AudioSource GetSoundPlayer3D(Vector3 worldPosition, Transform parent = null, bool looped = false)
        {
            var audioSource = looped ? CreateNewAudioSource() : GetPlayerFromPool();
            audioSource.spatialBlend = 1;
            audioSource.loop = looped;
            audioSource.transform.position = worldPosition;
            audioSource.transform.parent = parent == null ? this.transform : parent;
            return audioSource;
        }

        private AudioSource GetPlayerFromPool()
        {
            var found = _pool.FirstOrDefault(p => !p.isPlaying);
            if (found != null)
                return found;

            var created = CreateNewAudioSource();
            _pool.Add(created);
            return created;
        }

        private AudioSource CreateNewAudioSource()
        {
            return Instantiate(soundSourcePrefab, transform);
        }
    }
}