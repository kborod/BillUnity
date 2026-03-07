using Kborod.BilliardCore;
using Kborod.Services.UIScreenManager;
using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Kborod.UI.Screens
{
    [UIScreen("UI/Screens/TestShotScreen.prefab", true)]
    public class TestShotScreen : UIScreenBase
    {
        [SerializeField] private TMP_InputField _shotContext;
        [SerializeField] private Button _btnRunShot;

        private void Start()
        {
            _btnRunShot.onClick.AddListener(RunClickHandler);
        }

        private void RunClickHandler()
        {
            var context = JsonConvert.DeserializeObject<CalculatePoolShotContext>(_shotContext.text);

            var calculator = new PoolShotCalculator();

            var result = calculator.CalculateShot(context);

            var log = string.Empty;
            foreach (var b1 in context.BallDatas)
            {
                var b2 = result.RulesResult.BallDatas[b1.Number];

                var eq = b1.IsRemoved != b2.IsRemoved || (b1.IsRemoved == false && (b1.X != b2.X || b1.Y != b2.Y));
                log += $"{eq} {b1} - {b2}\n";
            }

            Debug.Log("Comparation prositions:\n" + log);
        }
    }
}
