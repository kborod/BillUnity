using Kborod.Services.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Kborod.UI.Screens
{
    public class SoundMuteButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _buttonIcon;
        [SerializeField] private Sprite _spriteOn;
        [SerializeField] private Sprite _spriteOf;

        [Inject] private ISoundVolumeSettings _soundVolumeSettings;

        private void Start()
        {
            _button.onClick.AddListener(ClickHandler);
            _soundVolumeSettings.SoundSettingsChanged += RefreshIU;
            RefreshIU();
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
            _soundVolumeSettings.SoundSettingsChanged -= RefreshIU;
        }

        private async void ClickHandler()
        {
            _soundVolumeSettings.SetMuteState(!_soundVolumeSettings.GetMuteState());
        }

        private void RefreshIU()
        {
            _buttonIcon.sprite = !_soundVolumeSettings.GetMuteState() ? _spriteOn : _spriteOf;
        }
    }
}
