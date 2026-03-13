using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Kborod.AsyncProcesses
{
    public class OpenSceneProcess
    {
        private string _sceneName;

        public OpenSceneProcess(string sceneName)
        {
            _sceneName = sceneName;
        }

        public async UniTask Run()
        {
            try
            {
                await SceneManager.LoadSceneAsync(_sceneName);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
