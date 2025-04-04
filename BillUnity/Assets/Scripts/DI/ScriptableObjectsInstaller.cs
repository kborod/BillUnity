using Kborod.UI.Screens.Table;
using UnityEngine;
using Zenject;

namespace Kborod.Services.DI
{
    public class ScriptableObjectsInstaller : MonoInstaller
    {
        [SerializeField] private BallsSO _ballsSO;

        public override void InstallBindings()
        {
            Container.Bind<BallsSO>().FromScriptableObject(_ballsSO).AsSingle();
        }
    }
}
