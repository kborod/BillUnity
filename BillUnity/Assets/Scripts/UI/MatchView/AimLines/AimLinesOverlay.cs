using Kborod.BilliardCore;
using Kborod.MatchManagement;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens
{
    public class AimLinesOverlay : MonoBehaviour
    {
        [SerializeField] private AimLinesDrawer aimLinesDrawer;

        [Inject] private IMatchServices _matchServices;
        private IEngineForUI _engine => _matchServices.EngineForUI;
        private MatchBase _match => _matchServices.Match;

        private Ball bTmp = new Ball(0);

        private void Start()
        {
            _match.AimInfoReceived += UpdateAimLines;
            _match.StateChanged += UpdateByMatchState;

            UpdateByMatchState(_match.State);
        }

        private void OnDestroy()
        {
            _match.AimInfoReceived -= UpdateAimLines;
            _match.StateChanged -= UpdateByMatchState;
        }

        private void UpdateByMatchState(MatchState state)
        {
            UpdateAimLines(_match.AimInfo);
        }

        private void UpdateAimLines(AimInfo info)
        {
            if (_match.State != MatchState.PrepeareTurn || _match.AimInfo.CueBall.HasValue == false || _match.AimInfo.IsBallMovingNow)
            {
                gameObject.SetActive(false);
                return;
            }

            bTmp.ChangeNumber(info.CueBall.Value);
            bTmp.v.p0.x = _engine.Balls[info.CueBall.Value].v.p0.x;
            bTmp.v.p0.y = _engine.Balls[info.CueBall.Value].v.p0.y;
            bTmp.v.vx = new Fixed64(info.DirectionXraw);
            bTmp.v.vy = new Fixed64(info.DirectionYraw);

            bTmp.v.updatePointsFromComponents();
            bTmp.v.makeVector();

            var aimData = _engine.GetAimObject(bTmp);

            var isBlockedAim = aimData.FirstCollBallNum >= 0 && _match.TurnSettings.BallsAvailableToAim.Contains(aimData.FirstCollBallNum) == false;
            aimLinesDrawer.Show(aimData, isBlockedAim);
        }
    }
}
