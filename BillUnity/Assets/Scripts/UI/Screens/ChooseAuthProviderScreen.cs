using Cysharp.Threading.Tasks;
using Kborod.DomainModel.Auth;
using Kborod.Services.UIScreenManager;
using UnityEngine;
using UnityEngine.UI;

namespace Kborod.UI.Screens
{
    [UIScreen("UI/Screens/ChooseAuthProviderScreen.prefab", true)]
    public class ChooseAuthProviderScreen : UIScreenBase
    {
        [SerializeField] private Button MailButton;
        [SerializeField] private Button YandexButton;

        private UniTaskCompletionSource<ProviderType> _tcs;

        private void Start()
        {
            MailButton.onClick.AddListener(MailClickHandler);
            YandexButton.onClick.AddListener(YandexClickHandler);
        }

        public async UniTask<ProviderType> GetSelectedProviderAsync()
        {
            _tcs = new UniTaskCompletionSource<ProviderType>();
            return await _tcs.Task;
        }

        private void MailClickHandler()
        {
            _tcs.TrySetResult(ProviderType.EmailPassword);
            Close();
        }

        private void YandexClickHandler()
        {
            _tcs.TrySetResult(ProviderType.Yandex);
            Close();
        }
    }
}
