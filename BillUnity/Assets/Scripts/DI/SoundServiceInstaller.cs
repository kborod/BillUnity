using Zenject;
using UnityEngine;
using Kborod.Services.Sound;

namespace Kborod.Services.DI
{
    public class SoundServiceInstaller : MonoInstaller
    {
        [SerializeField] private AudioAssetsSO _soundsAssets;
        [SerializeField] private AudioSourcesProvider _audioSourcesProvider;
        [SerializeField] private SoundService _soundService;

        public override void InstallBindings()
        {
            Container.Bind<AudioAssetsSO>().FromInstance(_soundsAssets).AsSingle();
            Container.Bind<AudioSourcesProvider>().FromComponentInNewPrefab(_audioSourcesProvider).AsSingle().NonLazy();
            Container.Bind<SoundService>().FromComponentInNewPrefab(_soundService).AsSingle().NonLazy();
            Container.Bind<ISoundVolumeSettings>().To<SoundSettingsLocal>().AsSingle().NonLazy();
        }
    }
}
