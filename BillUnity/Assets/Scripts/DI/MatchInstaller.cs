using Kborod.MatchManagement;
using Zenject;

namespace Kborod.Services.DI
{
    public class MatchInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind(typeof(IMatchServices), typeof(MatchServices)).To<MatchServices>().FromNew().AsSingle();
        }
    }
}
