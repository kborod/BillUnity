using Kborod.DomainModel.Implementations;
using Kborod.DomainModel;
using Zenject;
using Kborod.DomainModel.Auth;

namespace Kborod.Services.DI
{
    public class DomainModelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AccountModel>().To<AccountModelLocal>().FromNew().AsSingle();

            Container.Bind<AuthService>().FromNew().AsSingle().NonLazy();

            //AsTransient - При инжекте каждый раз новый генерится
            Container.Bind<EmailPasswordAuthProvider>().FromNew().AsTransient().Lazy();
        }
    }
}
