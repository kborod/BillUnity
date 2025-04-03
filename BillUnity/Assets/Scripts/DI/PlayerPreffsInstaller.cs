using Zenject;
using Kborod.Services.LocalCache;

namespace Kborod.Services.DI
{
    public class PlayerPreffsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerPrefsService>().FromNew().AsSingle().NonLazy();
        }
    }
}
