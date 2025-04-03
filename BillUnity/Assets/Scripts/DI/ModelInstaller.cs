using Kborod.Model.Implementations;
using Kborod.Model;
using Zenject;

namespace Kborod.Services.DI
{
    public class ModelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var accountModelLocal = new AccountModelLocal();
            Container.Bind<AccountModel>().FromInstance(accountModelLocal).AsSingle();
        }
    }
}
