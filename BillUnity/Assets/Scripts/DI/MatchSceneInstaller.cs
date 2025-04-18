using Zenject;
using Kborod.MatchManagement;
using Kborod.BilliardCore;

namespace Kborod.Services.DI
{
    public class MatchSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(Engine), typeof(IEngineForUI)).To<Engine>().AsSingle();
            Container.Bind<EnginePlayer>().FromNew().AsSingle();
            Container.Bind<MatchBase>().To<MatchPoolEight>().AsSingle();
        }
    }
}
