using UnityEngine;
using Zenject;

namespace Kborod.Loader
{
    public class Bootstraper : MonoBehaviour
    {
        [Inject] private AppProcessor _appProcessor;

        private async void Start()
        {
            await _appProcessor.StartApplication();
        }
    }
}
