using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Kborod.UI.Screens.Table
{
    [CreateAssetMenu(menuName = "ScriptableObjects/BallsSO", fileName = "BallsSO", order = 0)]
    public class BallsSO : ScriptableObject
    {
        [SerializeField]
        private List<BallData> _poolBalls;

        public BallData GetPoolBall(int ballNumber)
        {
            var result = _poolBalls.FirstOrDefault(b => b.BallNumber == ballNumber);

            if (result == null)
                Debug.LogError($"Pool ball {ballNumber} not found");

            return result;
        }
    }

    [Serializable]
    public class BallData
    {
        [field: SerializeField] public int BallNumber { get; private set; }
        [field: SerializeField] public Texture2D Texture { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}