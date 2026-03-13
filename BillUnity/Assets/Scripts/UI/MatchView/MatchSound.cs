using Kborod.BilliardCore;
using Kborod.MatchManagement;
using Kborod.Services.Sound;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens.Table
{
    public class MatchSound: MonoBehaviour
    {
        [Inject] private SoundService _soundService; 
        [Inject] private MatchServices _matchServices;
        private MatchBase _match => _matchServices.Match;


        private void Start()
        {
            _match.CueBallHittedWithPower += PlayCueBallHitted;
            _match.ShotTickCompleted += PlayPartyTickSounds;
            _match.TurningPlayerChanged += PlaySwapTurn;
        }

        private void OnDestroy()
        {
            _match.CueBallHittedWithPower -= PlayCueBallHitted;
            _match.ShotTickCompleted -= PlayPartyTickSounds;
            _match.TurningPlayerChanged -= PlaySwapTurn;
        }

        private void PlaySwapTurn()
        {
            _soundService.PlaySound(SoundType.SwapTurn);
        }

        private void PlayCueBallHitted(float power)
        {
            if (power == 0)
                return;
            if (power < 0.3)
                _soundService.PlaySound(SoundType.BallShot3, 0.5f + 0.5f * (power * 3.33f));
            else if (power < 0.5)
                _soundService.PlaySound(SoundType.BallShot2, 0.7f + 0.3f * ((power - 0.3f) * 5));
            else
                _soundService.PlaySound(SoundType.BallShot1, 0.8f + 0.2f * ((power - 0.5f) * 2f));
        }

        private void PlayPartyTickSounds(ShotTickResult tickResult)
		{

			if (tickResult.MaxBallsCollPower > Fixed64.Zero)
				PlayBallVsBall(tickResult.MaxBallsCollPower.ToFloat());

			if (tickResult.MaxWallsCollPower > Fixed64.Zero)
				PlayBallVsWall(tickResult.MaxWallsCollPower.ToFloat());

			if (tickResult.MaxPocketedPower > Fixed64.Zero)
				PlayBallPocketed(tickResult.MaxPocketedPower.ToFloat());
		}

        private void PlayBallVsBall(float power)
		{
			if (power < 0.1) 
				_soundService.PlaySound(SoundType.BallVsBall4, 0.2f + 0.8f * (power * 10)); 
			else if (power < 0.2)
                _soundService.PlaySound(SoundType.BallVsBall3, 0.6f + 0.4f * (power * 5));
			else
                _soundService.PlaySound(SoundType.BallVsBall1, 0.6f + 0.4f * (power * 2.5f));
		}


        private void PlayBallVsWall(float power)
		{
			if (power < 0.2)
                _soundService.PlaySound(SoundType.BallVsWall2, 0.2f + 0.8f * (power * 5));
            else
                _soundService.PlaySound(SoundType.BallVsWall1, 0.4f + 0.6f * ((power - 0.2f) * 1.2f));
		}

        private SoundType lastPocketSound = SoundType.BallPocketed1;
        private void PlayBallPocketed(float power)
		{
			lastPocketSound = (lastPocketSound == SoundType.BallPocketed1) ? SoundType.BallPocketed2 : SoundType.BallPocketed1;
            _soundService.PlaySound(lastPocketSound, 0.4f + 0.6f * power);
        }
    }
}
