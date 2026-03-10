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
            _btnRunTest.onClick.AddListener(TestDeterministicFloat);
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

                var eq = b1.IsRemoved != b2.IsRemoved || (b1.IsRemoved == false && (b1.Xraw != b2.Xraw || b1.Yraw != b2.Yraw));
                log += $"{eq} {b1} - {b2}\n";
            }

            Debug.Log("Comparation prositions:\n" + log);
        }

        private void TestConvert()
        {
            var a = Fixed64.FromDouble(0.995036999927834f);
            var b = Fixed64.FromDouble(0.0995036999229342f);

            var aa = a * a;
            var bb = b * b;

            var sum = aa + bb;

            var sqrt = Fixed64.Sqrt(sum);

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
                    var rndD = i == 0 ? 0.995036999927834d : GetRndDouble(0.99d, 1d);
                    var rndD2 = GetRndDouble(-1, 1);
                    var rndD3 = GetRndDouble(-100000, 100000);
                    var rndD4 = GetRndDouble(-1000000000, 1000000000);

                    var rnd = GetRndFloat(-10000f, 10000f);
                    var rnd2 = GetRndFloat(-10000f, 10000f);
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



                    //Fixed64.Sin(Fixed64.FromFloat(rnd));
                    //Fixed64.Cos(Fixed64.FromFloat(rnd));
                    //Fixed64.Sqrt(Fixed64.FromFloat(rnd));
                    //Fixed64.Abs(Fixed64.FromFloat(rnd));
                    //Fixed64.Min(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2));
                    //Fixed64.Max(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2));
                    //Fixed64.Lerp(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2), Fixed64.FromFloat(rnd0_1));
                    //Fixed64.Clamp(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rndMin), Fixed64.FromFloat(rndMax));
                    //Fixed64.Clamp(Fixed64.FromFloat(rndForClampInner), Fixed64.FromFloat(rndMin), Fixed64.FromFloat(rndMax));






                    Compare($"MultiplyFloat({rnd}, {rnd2})", (Fixed64.FromFloat(rnd) * Fixed64.FromFloat(rnd2)).ToFloat(), rnd * rnd2);
                    Compare($"SquareFloat({rnd})", (Fixed64.FromFloat(rnd) * Fixed64.FromFloat(rnd)).ToFloat(), rnd * rnd);
                    if (rnd2 != 0)
                        Compare($"DivisionFloat({rnd}, {rnd2})", (Fixed64.FromFloat(rnd) / Fixed64.FromFloat(rnd2)).ToFloat(), rnd / rnd2);
                    Compare($"SumFloat({rnd}, {rnd2})", (Fixed64.FromFloat(rnd) + Fixed64.FromFloat(rnd2)).ToFloat(), rnd + rnd2);
                    Compare($"subtractionFloat({rnd}, {rnd2})", (Fixed64.FromFloat(rnd) - Fixed64.FromFloat(rnd2)).ToFloat(), rnd - rnd2);
                    Compare($"sin({rnd})", Fixed64.Sin(Fixed64.FromFloat(rnd)).ToFloat(), Mathf.Sin(rnd), 2, 0.01f);
                    Compare($"cos({rnd})", Fixed64.Cos(Fixed64.FromFloat(rnd)).ToFloat(), Mathf.Cos(rnd), 2, 0.01f);
                    if (rnd > 0)
                        Compare($"sqrtttt({rnd})", Fixed64.Sqrt(Fixed64.FromFloat(rnd)).ToFloat(), Mathf.Sqrt(rnd));
                    Compare($"abs({rnd})", Fixed64.Abs(Fixed64.FromFloat(rnd)).ToFloat(), Mathf.Abs(rnd));
                    Compare($"min({rnd}, {rnd2})", Fixed64.Min(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2)).ToFloat(), Mathf.Min(rnd, rnd2));
                    Compare($"max({rnd}, {rnd2})", Fixed64.Max(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2)).ToFloat(), Mathf.Max(rnd, rnd2));
                    Compare($"lerp({rnd}, {rnd2}, {rnd0_1})", Fixed64.Lerp(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rnd2), Fixed64.FromFloat(rnd0_1)).ToFloat(), Mathf.Lerp(rnd, rnd2, rnd0_1));
                    Compare($"Clamp({rnd}, {rndMin}, {rndMax})", Fixed64.Clamp(Fixed64.FromFloat(rnd), Fixed64.FromFloat(rndMin), Fixed64.FromFloat(rndMax)).ToFloat(), Mathf.Clamp(rnd, rndMin, rndMax));
                    Compare($"Clamp({rndForClampInner}, {rndMin}, {rndMax})", Fixed64.Clamp(Fixed64.FromFloat(rndForClampInner), Fixed64.FromFloat(rndMin), Fixed64.FromFloat(rndMax)).ToFloat(), Mathf.Clamp(rndForClampInner, rndMin, rndMax));
                    Compare($"ConvertFloat({rnd})", Fixed64.FromDouble(rnd).ToFloat(), rnd);


                    Compare($"MultiplyDouble({rndD}, {rndD2})", (Fixed64.FromDouble(rndD) * Fixed64.FromDouble(rndD2)).ToDouble(), rndD * rndD2);
                    Compare($"SquareDouble({rndD})", (Fixed64.FromDouble(rndD) * Fixed64.FromDouble(rndD)).ToDouble(), rndD * rndD);
                    if (rndD2 != 0)
                        Compare($"DivisionDouble({rndD}, {rndD2})", (Fixed64.FromDouble(rndD) / Fixed64.FromDouble(rndD2)).ToDouble(), rndD / rndD2);
                    Compare($"SumDouble({rndD}, {rndD2})", (Fixed64.FromDouble(rndD) + Fixed64.FromDouble(rndD2)).ToDouble(), rndD + rndD2);
                    Compare($"subtractionFloat({rndD}, {rndD2})", (Fixed64.FromDouble(rndD) - Fixed64.FromDouble(rndD2)).ToDouble(), rndD - rndD2);

                    Compare($"ConvertDouble({rndD})", Fixed64.FromDouble(rndD).ToDouble(), rndD);
                    Compare($"ConvertDouble({rndD2})", Fixed64.FromDouble(rndD2).ToDouble(), rndD2);
                    Compare($"ConvertDouble({rndD3})", Fixed64.FromDouble(rndD3).ToDouble(), rndD3);
                    Compare($"ConvertDouble({rndD4})", Fixed64.FromDouble(rndD4).ToDouble(), rndD4);
                }

                Debug.Log($"TestCompleted: {Time.realtimeSinceStartup - t} сек");
            }

        }


        private void Compare(string title, float f1, float f2, int digits = 3, float? differenceAllowed = null)
        {
            if (MathF.Round(f1, digits, MidpointRounding.AwayFromZero) != MathF.Round(f2, digits, MidpointRounding.AwayFromZero))
            {
                if (differenceAllowed != null)
                {
                    var d = f1 - f2;
                    if (d > -differenceAllowed && d < differenceAllowed)
                    {
                        return;
                    }
                }
                    
                Debug.Log($"-----{title}-----");
                Debug.Log(f1);
                Debug.Log(f2);
            }
        }


        private void Compare(string title, double f1, double f2, int digits = 3)
        {
            if (Math.Round(f1, digits, MidpointRounding.AwayFromZero) != Math.Round(f2, digits, MidpointRounding.AwayFromZero))
            {
                Debug.Log($"-----{title}-----");
                Debug.Log(f1);
                Debug.Log(f2);
            }
        }

        private System.Random rnd = new System.Random();

        private float GetRndFloat(float min, float max)
        {
            return (float) GetRndDouble(min, max);
        }

        private double GetRndDouble(double min, double max)
        {
            return min + (rnd.NextDouble() * (max - min));
        }
    }
}
