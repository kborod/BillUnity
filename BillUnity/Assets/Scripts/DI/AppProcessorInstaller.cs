using Zenject;
using Kborod.Loader;

namespace Kborod.Services.DI
{
    public class AppProcessorInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AppProcessor>().FromNew().AsSingle().NonLazy();
        }
    }
}
