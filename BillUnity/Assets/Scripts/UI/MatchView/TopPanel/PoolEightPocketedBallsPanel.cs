using Kborod.BilliardCore;
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
        [SerializeField] private bool isPlayer1;

        [Inject] DiContainer _diContainer;
        [Inject] private MatchBase _match;
        [Inject] private IEngineForUI _engineForUI;

        private MatchPoolEight _matchPollEight;
        private PoolEightPlayer _player;

        private List<BallIconItem> _ballIcons = new List<BallIconItem>(7);

        private void Start()
        {
            if (_match is not MatchPoolEight)
            {
                Destroy(gameObject);
                return;
            }

            _matchPollEight = (MatchPoolEight)_match;
            _player = (PoolEightPlayer)(isPlayer1 ? _match.Player1 : _match.Player2);

            _matchPollEight.BallTypesSelected += BallTypeSelectedHandler;
            _matchPollEight.ShotCompleted += ShotCompletedHandler;

            CreateIcons();
        }

        private void OnDestroy()
        {
            if (_matchPollEight != null)
            {
                _matchPollEight.BallTypesSelected -= BallTypeSelectedHandler; 
                _matchPollEight.ShotCompleted -= ShotCompletedHandler;
            }   
        }

        private void CreateIcons()
        {
            foreach (Transform child in ballsRoot) 
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < 7; i++)
            {
                _ballIcons.Add(_diContainer.InstantiatePrefabForComponent<BallIconItem>(ballPrefab, ballsRoot));
            }
        }

        private void BallTypeSelectedHandler()
        {
            for (int i = 0; i < 7; i++)
            {
                var ballNum = _player.BallType == PoolBallType.Solid ? i + 1 : i + 9;
                _ballIcons[i].SetBall(ballNum, GameType.PoolEight, 0.25f);
                if (_engineForUI.Balls[ballNum].isRemoved)
                    _ballIcons[i].SetTransparency0_1(1);
            }
        }

        private void ShotCompletedHandler(ShotResultData data)
        {
            foreach (var pocketedBall in data.ShotResult.PocketedBalls)
            {
                if (data.ReturnedPocketedBalls.Contains(pocketedBall))
                    continue;

                if (pocketedBall.GetPoolBallType() == _player.BallType)
                    BallPocketedHandler(pocketedBall);
            } 
        }

        private void BallPocketedHandler(int ballNumber)
        {
            _ballIcons[ballNumber.GetPoolBallType() == PoolBallType.Solid ? ballNumber - 1 : ballNumber - 9].SetTransparency0_1(1);
        }
    }
}
