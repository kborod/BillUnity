using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Kborod.Services.Sound
{
    [CreateAssetMenu(menuName = "ScriptableObjects/AudioAssetsSO", fileName = "AudioAssetsSO", order = 0)]
    public class AudioAssetsSO : ScriptableObject
    {
        [SerializeField]
        private List<SoundClip> _soundClips;

        /// <summary>
        /// Возвращает клип для типа звука. Если клипов несколько, то возвращает случайный
        /// </summary>
        public ClipData GetClip(SoundType type)
        {
            var resultClips = _soundClips.FirstOrDefault(c => c.Type == type)?.Clips;

            if (resultClips == null || resultClips.Length == 0)
                Debug.LogError($"SoundsClip not found {type}");

            return resultClips[UnityEngine.Random.Range(0, resultClips.Length)];
        }
    }

    [Serializable]
    public class SoundClip
    {
        [field: SerializeField] public SoundType Type { get; private set; }
        [field: SerializeField, Tooltip("Если больше одного элемента, то при воспроизведении выбирается случайный")] public ClipData[] Clips { get; private set; }
    }

    [Serializable]
    public class ClipData
    {
        [field: SerializeField] public AudioClip Clip { get; private set; }
        [field: SerializeField] public float Volume { get; private set; }
        [field: SerializeField] public SoundTag Tag { get; private set; }
    }


    public enum SoundType
    {
        None = 0,
        MelodyBG = 1,

        BallVsBall0 = 100,
        BallVsBall1 = 101,
        BallVsBall2 = 102,
        BallVsBall3 = 103,
        BallVsBall4 = 104,

        BallVsWall1 = 110,
        BallVsWall2 = 111,

        BallPocketed1 = 121,
        BallPocketed2 = 122,

        BallShot1 = 131,
        BallShot2 = 132,
        BallShot3 = 133,
    }

    public enum SoundTag
    {
        background,
        ui
    }
}