using Kborod.BilliardCore;
using Kborod.Extensions;
using Kborod.MatchManagement;
using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Kborod.UI.Screens.Table.BallsRemove
{
    public class BallsRemover : MonoBehaviour
    {
        public event Action<Ball, float> BallMoved;

        private const float BallSpeed = 15;

        [SerializeField] private PathCreator pathCreator;
        [SerializeField] private Transform ballsRoot;

        [Inject] MatchBase _match;

        private float maxPathDistance => pathCreator.path.length - ballsOnPathCount * Config.BALL_DIAM_PX * Config.MODEL_COORD_TO_WORLD_KOEF;

        //массив с забитыми шарами (элементы массива - экземпляры класса RemovedBall)
        private List<RemovedBall> removedBalls = new List<RemovedBall>();
        private int ballsOnPathCount = 0;


        private IEnumerator updateCoroutine;

        private void Start()
        {
            _match.ShotTickCompleted += ShotTickCompletedHandler;
            _match.ShotCompleted += ShotCompletedHandler;
        }

        private void OnDestroy()
        {
            _match.ShotTickCompleted -= ShotTickCompletedHandler;
            _match.ShotCompleted -= ShotCompletedHandler;
        }

        private void ShotTickCompletedHandler(ShotTickResult tickResult)
        {
            tickResult.PocketedBallsOrNull?.ForEach(b => AddBall(b, ballsRoot, false));
        }

        private void ShotCompletedHandler(ShotResultData result)
        {
            if (result.ReturnedPocketedBalls.Count > 0)
            {
                result.ReturnedPocketedBalls.ForEach(b => RemoveBall(b));
            }
        }

        /// <summary>
        /// Добавляем забитый шар в ремувер
        /// </summary>
        private void AddBall(Ball b, Transform ballsRoot, bool  withoutAnim = false)
		{
			var removedBall = new RemovedBall(b);
            this.ballsRoot = ballsRoot;
			
			if (withoutAnim) 
			{
				removedBall.SetState(RemoveState.STATE_WITHOUT_ANIM_WAIT);
			}
			else
			{
				removedBall.SetState(RemoveState.STATE_TO_POCKET_POINT);
				removedBall.ChangeMoveVector(RemoveState.STATE_TO_POCKET_POINT);
			}
            removedBalls.Add(removedBall);
			UpdateBall(removedBall, b.RemoveDeltaTime);
            TryStartCoroutine();
		}

        /// <summary>
        /// Удалить шар из ремувера
        /// </summary>
        private void RemoveBall(int ballNumber)
		{
            removedBalls.RemoveAll(rb => rb.ball.Number == ballNumber);
		}

        private void Clear()
		{
            foreach(var rb in removedBalls)
                rb.ball.StopBall();
            removedBalls.Clear();
			ballsOnPathCount = 0;
		}

        /// <summary>
        /// Обновить шар
        /// </summary>
        private void UpdateBall(RemovedBall removedBall, float deltaTime)
		{
			if (removedBall.state == RemoveState.STATE_ANIM_OVER)
			{
				return;
			}
			else if (removedBall.state == RemoveState.STATE_TO_POCKET_POINT)
			{
				if (Vector2.Distance(removedBall.ball.v.p0.ToVector2(), removedBall.pocket.pRemove.ToVector2()) > removedBall.ball.v.len* (deltaTime / Config.SPEED_UPDATE_DELTA )  )
				{
					removedBall.ball.v.p0.x += removedBall.ball.v.vx* (deltaTime / Config.SPEED_UPDATE_DELTA );
					removedBall.ball.v.p0.y += removedBall.ball.v.vy* (deltaTime / Config.SPEED_UPDATE_DELTA );
					removedBall.ball.v.updatePointsFromComponents();
					removedBall.ball.v.makeVector();
				}
				else
				{
					removedBall.ball.v.p0.x = removedBall.pocket.pRemove.x;
					removedBall.ball.v.p0.y = removedBall.pocket.pRemove.y;
					removedBall.SetState(RemoveState.STATE_FROM_POCKET);
					removedBall.ChangeMoveVector(RemoveState.STATE_FROM_POCKET);
					removedBall.ball.MoveToBottomLayer();
				}
			}
            else if (removedBall.state == RemoveState.STATE_FROM_POCKET)
            {
                if (Vector2.Distance(removedBall.ball.v.p0.ToVector2(), removedBall.pocket.pRemove.ToVector2()) < Config.POCKET_RAD_PX * 3)
                {
                    removedBall.ball.v.p0.x += removedBall.ball.v.vx * (deltaTime / Config.SPEED_UPDATE_DELTA);
                    removedBall.ball.v.p0.y += removedBall.ball.v.vy * (deltaTime / Config.SPEED_UPDATE_DELTA);
                    removedBall.ball.v.updatePointsFromComponents();
                }
                else
                {
                    removedBall.SetState(RemoveState.STATE_HIDDEN);
                }
            }
            else if (removedBall.state == RemoveState.STATE_HIDDEN)
            {
                if (removedBall.ball.Number == 0)
                {
                    removedBall.SetState(RemoveState.STATE_ANIM_OVER);
                    return;
                }
                if (AddBallToPathAnim(removedBall) == true)
                {
                    removedBall.SetState(RemoveState.STATE_PATH_MOVE);
                    return;
                }   
            }
            else if (removedBall.state == RemoveState.STATE_PATH_MOVE)
            {
                if (removedBall.currPathDis == removedBall.maxPathDis)
                {
                    removedBall.SetState(RemoveState.STATE_ANIM_OVER);
                    removedBall.ball.StopBall();
                    return;
                }

                removedBall.currPathDis += (BallSpeed * deltaTime / Config.SPEED_UPDATE_DELTA) * Config.MODEL_COORD_TO_WORLD_KOEF;
                if (removedBall.currPathDis > removedBall.maxPathDis) removedBall.currPathDis = removedBall.maxPathDis;
                Vector2 p = GetPathPointInModelCoord(removedBall.currPathDis);
                var moveVector = p - removedBall.ball.v.p0.ToVector2();
                moveVector.Normalize();
                moveVector = moveVector * BallSpeed;
                removedBall.ball.v.p0 = p.ToPoint();
                removedBall.ball.v.vx = moveVector.x;
                removedBall.ball.v.vy = moveVector.y;
                removedBall.ball.v.updatePointsFromComponents();
                removedBall.ball.vVertSpin.vx = removedBall.ball.v.vx;
                removedBall.ball.vVertSpin.vy = removedBall.ball.v.vy;
                removedBall.ball.vVertSpin.updateLen();
            }
            else if (removedBall.state == RemoveState.STATE_WITHOUT_ANIM_WAIT)
            {
                if (removedBalls.FirstOrDefault(rb => rb.state != RemoveState.STATE_ANIM_OVER && rb.state != RemoveState.STATE_WITHOUT_ANIM_WAIT) != null)
                    Debug.LogError("BallsRemover has moving balls");

                removedBall.ball.v.p0 = GetPathPointInModelCoord(maxPathDistance).ToPoint();
                ballsOnPathCount++;
                removedBall.SetState(RemoveState.STATE_ANIM_OVER);
                removedBall.ball.StopBall();
                removedBall.ball.MoveToBottomLayer();
            }


            BallMoved?.Invoke(removedBall.ball, deltaTime);
        }


        /// <summary>
        /// Начать анимацию качения шара по шароприемнику
        /// </summary>
        /// <returns>Возвращает false, если в данный момент шароприемник занят (чтобы не было наложения шаров друг на друга)</returns>
        private bool AddBallToPathAnim(RemovedBall removedBall)
        {
            if (removedBalls.FirstOrDefault(rb => rb.state == RemoveState.STATE_PATH_MOVE) != null)
                return false;

            removedBall.currPathDis = 0;
            removedBall.maxPathDis = pathCreator.path.length - ballsOnPathCount * Config.BALL_DIAM_PX * Config.MODEL_COORD_TO_WORLD_KOEF;
            removedBall.removeNumber = ballsOnPathCount;
            ballsOnPathCount++;
            return true;
        }

        private Vector2 GetPathPointInModelCoord(float distance)
		{
            return GetPathPointInBallsRootCoord(distance) / Config.MODEL_COORD_TO_WORLD_KOEF;
		}

        private Vector2 GetPathPointInBallsRootCoord(float distance)
		{
            return ballsRoot.InverseTransformPoint(pathCreator.path.GetPointAtDistance(distance, EndOfPathInstruction.Stop));
		}

        private void TryStartCoroutine()
        {
            if (updateCoroutine != null)
                return;
            StartCoroutine(updateCoroutine = UpdateCoroutine());
        }

        private IEnumerator UpdateCoroutine()
        {
            while(removedBalls.Any(b => b.state != RemoveState.STATE_ANIM_OVER))
            {
                yield return null;
                foreach (var rb in removedBalls)
                {
                    UpdateBall(rb, Time.deltaTime * 1000);
                }
            }
            updateCoroutine = null;
        }
    }
}
