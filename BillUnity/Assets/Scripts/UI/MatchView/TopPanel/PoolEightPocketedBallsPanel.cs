using Kborod.MatchManagement;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens.Table.TopPanel
{
    public class PoolEightPocketedBallsPanel : MonoBehaviour
    {
        [SerializeField] private BallIconItem ballPrefab;
        [SerializeField] private Transform ballsRoot;

        [Inject] DiContainer _diContainer;

        Match _match;
        PoolEightPlayer _player;

        private Queue<BallIconItem> _ballIcons = new Queue<BallIconItem>();

        private void OnDestroy()
        {
            if (_match != null)
                _match.ShotCompleted -= ShotCompletedHandler;
        }

        public void Setup(Match match, PoolEightPlayer player)
        {
            _match = match;
            _player = player;

            _match.ShotCompleted += ShotCompletedHandler;

            CreateIcons();
        }

        private void CreateIcons()
        {
            foreach (Transform child in ballsRoot) 
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < _player.MaxPocketedBallsCount; i++)
            {
                _ballIcons.Enqueue(_diContainer.InstantiatePrefabForComponent<BallIconItem>(ballPrefab, ballsRoot));
            }
        }

        private void ShotCompletedHandler(ShotResultData data)
        {
            foreach (var pocketedBall in data.ShotResult.pocketedBalls)
            {
                if (data.ReturnedPocketedBalls.Contains(pocketedBall))
                    continue;

                if (_match.PoolEightRules.GetBallType(pocketedBall) == _player.BallType)
                    BallPocketedHandler(pocketedBall);
            } 
        }

        private void BallPocketedHandler(int ballNumber)
        {
            if (_ballIcons.Count <= 0)
                return;
            _ballIcons.Dequeue().SetBall(ballNumber, GameType.PoolEight);
        }
    }
}
