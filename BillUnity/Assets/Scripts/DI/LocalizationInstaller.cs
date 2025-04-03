using Zenject;
using Kborod.Services.Localization;
using UnityEngine;

namespace Kborod.Services.DI
{
    public class LocalizationInstaller : MonoInstaller
    {
        [SerializeField] private LocalizationService _localizationServicePrefab;

        public override void InstallBindings()
        {
            Container.Bind<LocalizationService>().FromComponentInNewPrefab(_localizationServicePrefab).AsSingle().NonLazy();
        }
    }
}
