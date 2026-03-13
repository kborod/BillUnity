using Kborod.Services.Localization;
using Kborod.Services.UIScreenManager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kborod.UI.Screens
{
    [UIScreen("UI/Screens/DialogModal.prefab", true)]
    public class DialogModal : UIScreenBase
    {
        [SerializeField] private TextLocalizator _text;
        [SerializeField] private List<Button> _closeButtons;
        [SerializeField] private Button _buttonOk;
        [SerializeField] private Button _buttonCancel;

        private Action<bool> _callback = null;

        private void Start()
        {
            _closeButtons.ForEach(b => b.onClick.AddListener(CloseClickHandler));
            _buttonOk.onClick.AddListener(OkClickHandler);
            _buttonCancel.onClick.AddListener(CancelClickHandler);
        }

        private void CloseClickHandler()
        {
            Close();
        }

        private void OkClickHandler()
        {
            _callback?.Invoke(true);
            Close();
        }

        private void CancelClickHandler()
        {
            _callback?.Invoke(false);
            Close();
        }
        public void Setup(string textId, Action<bool> callback = null)
        {
            _text.SetId(textId);
            _callback = callback;

            RefreshUI();
        }

        private void RefreshUI()
        {
            _buttonCancel.gameObject.SetActive(_callback != null);
        }
    }
}
