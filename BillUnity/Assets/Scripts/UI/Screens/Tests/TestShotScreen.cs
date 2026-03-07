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
        [SerializeField] private Button _btnRunTest;

        private void Start()
        {
            _btnRunShot.onClick.AddListener(RunClickHandler);
            _btnRunShot.onClick.AddListener(TestDeterministicFloat);
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

        private void TestDeterministicFloat()
        {
            //Debug.Log(Fixed64.Sqrt(Fixed64.FromFloat(1f)).ToFloat());
            //Debug.Log(Fixed64.Sqrt(Fixed64.FromFloat(4f)).ToFloat());
            //Debug.Log(Fixed64.Sqrt(Fixed64.FromFloat(2f)).ToFloat());
            //Debug.Log(Fixed64.Sqrt(Fixed64.FromFloat(9f)).ToFloat());
            //Debug.Log(Fixed64.Sqrt(Fixed64.FromFloat(0.25f)).ToFloat());
            //Debug.Log(Fixed64.Sqrt(Fixed64.FromFloat(0.64f)).ToFloat());
            //Debug.Log(Fixed64.Sqrt(Fixed64.FromFloat(64f)).ToFloat());
            //Debug.Log(Fixed64.Sqrt(Fixed64.FromFloat(2273.3824f)).ToFloat());


            for (int i = 0; i < 5; i++)
            {
                var t = Time.realtimeSinceStartup;
                Debug.Log("TestStarted");
                for (int j = 0; j < 10000; j++)
                {
                    var rnd = UnityEngine.Random.Range(-10000000f, 10000000f);
                    var rnd2 = UnityEngine.Random.Range(-10000000f, 10000000f);
                    var rnd0_1 = UnityEngine.Random.Range(0f, 1f);
                    var rndMin = UnityEngine.Random.Range(0f, 10f);
                    var rndMax = UnityEngine.Random.Range(10f, 20f);
                    var rndForClampInner = UnityEngine.Random.Range(0f, 20f);

                    //Mathf.Sin(rnd);
                    //Mathf.Cos(rnd);
                    //Mathf.Sqrt(rnd);
                    //Mathf.Abs(rnd);
                    //Mathf.Min(rnd, rnd2);
                    //Mathf.Max(rnd, rnd2);
                    //Mathf.Lerp(rnd, rnd2, rnd0_1);
                    //Mathf.Clamp(rnd, rndMin, rndMax);
                    //Mathf.Clamp(rndForClampInner, rndMin, rndMax);

                    Fixed64.Sin(Fixed64.FromFloat(rnd));
                    Fixed64.Cos(Fixed64.FromFloat(rnd));
                    Fixed64.Sqrt(Fixed64.FromFloat(rnd));
                    Fixed64.Abs(Fixed64.FromFloat(rnd));
                    Fixed64.Min(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2));
                    Fixed64.Max(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2));
                    Fixed64.Lerp(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2), Fixed64.FromFloat(rnd0_1));
                    Fixed64.Clamp(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rndMin), Fixed64.FromFloat(rndMax));
                    Fixed64.Clamp(Fixed64.FromFloat(rndForClampInner), Fixed64.FromFloat(rndMin), Fixed64.FromFloat(rndMax));




                    //Compare($"sin({rnd})", Fixed64.Sin(Fixed64.FromFloat(rnd)).ToFloat(), Mathf.Sin(rnd));
                    //Compare($"cos({rnd})", Fixed64.Cos(Fixed64.FromFloat(rnd)).ToFloat(), Mathf.Cos(rnd));
                    //if (rnd > 0)
                    //    Compare($"sqrtttt({rnd})", Fixed64.Sqrt(Fixed64.FromFloat(rnd)).ToFloat(), Mathf.Sqrt(rnd));
                    //Compare($"abs({rnd})", Fixed64.Abs(Fixed64.FromFloat(rnd)).ToFloat(), Mathf.Abs(rnd));
                    //Compare($"min({rnd}, {rnd2})", Fixed64.Min(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2)).ToFloat(), Mathf.Min(rnd, rnd2));
                    //Compare($"max({rnd}, {rnd2})", Fixed64.Max(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2)).ToFloat(), Mathf.Max(rnd, rnd2));
                    //Compare($"lerp({rnd}, {rnd2}, {rnd0_1})", Fixed64.Lerp(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2), Fixed64.FromFloat(rnd0_1)).ToFloat(), Mathf.Lerp(rnd, rnd2, rnd0_1));
                    //Compare($"Clamp({rnd}, {rndMin}, {rndMax})", Fixed64.Clamp(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rndMin), Fixed64.FromFloat(rndMax)).ToFloat(), Mathf.Clamp(rnd, rndMin, rndMax));
                    //Compare($"Clamp({rndForClampInner}, {rndMin}, {rndMax})", Fixed64.Clamp(Fixed64.FromFloat(rndForClampInner), Fixed64.FromFloat(rndMin), Fixed64.FromFloat(rndMax)).ToFloat(), Mathf.Clamp(rndForClampInner, rndMin, rndMax));
                }
                Debug.Log($"TestCompleted: {Time.realtimeSinceStartup - t} сек");
            }

        }


        private void Compare(string title, float f1, float f2)
        {
            if (MathF.Round(f1, 1, MidpointRounding.AwayFromZero) != MathF.Round(f2, 1, MidpointRounding.AwayFromZero))
            {
                var k = f1 / f2;
                if (f1 != 0 && f2 != 0 && k > 0.995 && k < 1.005)
                    return;
                Debug.Log($"-----{title}-----");
                Debug.Log(f1);
                Debug.Log(f2);
            }
        }
    }
}
