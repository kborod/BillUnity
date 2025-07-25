using Zenject;
using Kborod.MatchManagement;
using Kborod.BilliardCore;
using UnityEngine;
using Kborod.UI.Screens.Table;

namespace Kborod.Services.DI
{
    public class MatchSceneInstaller : MonoInstaller
    {
        [SerializeField] private MyShotInput _myShotInput; 

        public override void InstallBindings()
        {
            Container.Bind(typeof(Engine), typeof(IEngineForUI)).To<Engine>().AsSingle();
            Container.Bind<EngineShotMaker>().FromNew().AsSingle();
            Container.Bind<MatchBase>().To<MatchPoolEight>().AsSingle();
            Container.Bind<MyShotInput>().FromInstance(_myShotInput).AsSingle();
        }
    }
}
