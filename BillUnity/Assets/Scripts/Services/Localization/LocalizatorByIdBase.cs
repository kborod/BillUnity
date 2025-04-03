using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kborod.Services.Localization
{
    public abstract class LocalizatorByIdBase : LocalizatorBase
    {
        protected string _translationId = default;
        protected List<InjectionParams> _injections = default;

        public virtual void SetId(string id, bool withUpdate = true)
        {
            _translationId = id.ToString(); ;
            if (withUpdate)
            {
                UpdateLocalization();
            }
        }

        public virtual void InjectString(string value)
        {
            InjectParams(new InjectionParams() { type = InjectionType.simpleString, value = value });
        }

        public virtual void InjectStrings(params string[] values)
        {
            var a = new InjectionParams[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                a[i] = new InjectionParams() { type = InjectionType.simpleString, value = values[i] };
            }
            InjectParams(a);
        }

        public virtual void InjectParams(params InjectionParams[] values)
        {
            _injections = values.ToList();
            UpdateLocalization();
        }

        protected override void UpdateLocalization()
        {
            if (inited == false) return;
            var s = localization.GetTranslationById(_translationId);
            if (_injections != null && _injections.Count > 0)
            {
                var injectionStrings = _injections.Select(injection =>
                {
                    if (injection.type == InjectionType.simpleString) return injection.value;
                    return localization.GetTranslationById(injection.value);
                }).ToArray();
                try
                {
                    s = string.Format(s, injectionStrings);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"TextLocalizator injection error. Injections count must be more or equal "
                        + $"than the count of params in source string. Error: {ex}");
                }
            }
            TextComponent.text = s;
        }
    }


    [Serializable]
    public struct InjectionParams
    {
        public InjectionType type;
        public string value;

        public InjectionParams(InjectionType newType, string newValue)
        {
            type = newType;
            value = newValue;
        }
    }

    [Serializable]
    public enum InjectionType
    {
        simpleString,
        localizationId
    }
}
/* Copyright: Made by Appfox */