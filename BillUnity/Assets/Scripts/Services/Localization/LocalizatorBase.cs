using TMPro;
using UnityEngine;
using Zenject;

namespace Kborod.Services.Localization
{
    public abstract class LocalizatorBase : MonoBehaviour
    {
        public TextMeshProUGUI TextComponent { get; private set; }
        protected LocalizationService localization;

        protected bool inited = false;

        public string CurrentText => TextComponent == null ? string.Empty : TextComponent.text;

        [Inject]
        private void Construct(LocalizationService localization)
        {
            this.localization = localization;
        }

        private void Awake()
        {
            TextComponent = GetComponent<TextMeshProUGUI>();
        }

        protected virtual void Start()
        {
            Initialization();
            UpdateLocalization();
        }

        private void OnDestroy()
        {
            if (inited) localization.LanguageChanged -= UpdateLocalization;
        }

        protected abstract void UpdateLocalization();

        private void Initialization()
        {
            localization.LanguageChanged += UpdateLocalization;
            inited = true;
        }
    }
}
/* Copyright: Made by Appfox */