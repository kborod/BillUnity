using Cysharp.Threading.Tasks;
using Kborod.Services.ServerCommunication.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kborod.Tests
{
    public class TestNetControl : MonoBehaviour
    {
        [SerializeField] private Button _connectButton;
        [SerializeField] private TMP_InputField _inputfield;

        private SocketService _socketService;

        private void Start()
        {
            //Best.HTTP.Shared.HTTPManager.Logger.Level = Best.HTTP.Shared.Logger.Loglevels.All; ;
            _connectButton.onClick.AddListener(() => Connect().Forget());
        }

        private async UniTaskVoid Connect()
        {
            if (_socketService != null)
                await _socketService.Disconnect();

            _socketService = new SocketService();

            Debug.Log("Connecting...");

            await _socketService.ConnectHubAsync(_inputfield.text);

            Debug.Log("Connected");
        }
    }
}
