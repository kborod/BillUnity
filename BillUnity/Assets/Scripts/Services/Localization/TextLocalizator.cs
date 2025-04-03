using System.Collections.Generic;
using UnityEngine;

namespace Kborod.Services.Localization
{
    /// <summary>
    /// Локализатор текстового поля
    /// </summary>
    public class TextLocalizator : LocalizatorByIdBase
    {
        [SerializeField] private string translationId = default;

        [Header("Для вставки через string.Format()")]
        [SerializeField] private List<InjectionParams> injections = default;

        protected override void Start()
        {
            base._translationId = translationId;
            base._injections = injections;
            base.Start();
        }

        public override void SetId(string id, bool withUpdate = true)
        {
            base.SetId(id, withUpdate);
            translationId = id;
        }

        public override void InjectString(string value)
        {
            base.InjectString(value);
            injections = base._injections;
        }

        public override void InjectStrings(params string[] values)
        {
            base.InjectStrings(values);
            injections = base._injections;
        }

        public override void InjectParams(params InjectionParams[] values)
        {
            base.InjectParams(values);
            injections = base._injections;
        }
    }
}
/* Copyright: Made by Appfox */