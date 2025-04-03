using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Kborod.Services.UIScreenManager.Transitions
{
    public class InstantTransition : ITransition
    {
        private GameObject _gameObject;
        public InstantTransition(GameObject gameObject)
        {
            _gameObject = gameObject;
        }

        public async UniTask Show(ITransition prevOrNull)
        {
            if (prevOrNull != null)
                await prevOrNull.Hide(this);

            _gameObject.SetActive(true);
        }

        public async UniTask Hide(ITransition nextOrNull)
        {
            _gameObject.SetActive(false);
            await UniTask.CompletedTask;
        }
    }
}

/* Copyright: Made by Appfox */