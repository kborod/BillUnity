using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kborod.Services.UIScreenManager
{
    [CreateAssetMenu(fileName = "ModalIconsSO", menuName = "ScriptableObjects/ModalIconsSO", order = 1)]
    public class ModalIconsSO : ScriptableObject
    {
        [SerializeField] private List<IconData> _icons;

        public IconData GetIcon(IconType type) =>
            _icons.FirstOrDefault(i => i.Type == type);
    }

    [Serializable]
    public struct IconData
    {
        [field: SerializeField] public IconType Type { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
    }

    public enum IconType
    {
        None,
        Checkmark,
        Attention
    }
}

/* Copyright: Made by Appfox */
