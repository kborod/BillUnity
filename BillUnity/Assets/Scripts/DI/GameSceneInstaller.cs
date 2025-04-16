using Zenject;
using Kborod.MatchManagement;
using Kborod.BilliardCore;

namespace Kborod.Services.DI
{
    public class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //Container.Bind<Engine>().FromNew().AsSingle();
            Container.Bind(typeof(Engine), typeof(IEngineForUI)).To<Engine>().AsSingle();
            Container.Bind<EnginePlayer>().FromNew().AsSingle();
            Container.Bind<Match>().FromNew().AsSingle();
        }
    }
}
