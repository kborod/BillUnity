using Kborod.BilliardCore;
using UnityEngine;

namespace Kborod.UI.Screens
{
    public class AimLines : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer collisionIcon;
        [SerializeField] private Sprite collisionSpriteNormal;
        [SerializeField] private Sprite collisionSpriteBlock;
        [SerializeField] private Transform collisionPosition;
        [SerializeField] private LineRenderer lineToCollision;
        [SerializeField] private LineRenderer lineToCollisionShadow;
        [SerializeField] private LineRenderer lineFromCollision;
        [SerializeField] private LineRenderer lineFromCollisionShadow;
        [SerializeField] private LineRenderer lineFromAimBall;
        [SerializeField] private LineRenderer lineFromAimBallShadow;

        private const float LINES_Z = -0.25f;
        private const float LINES_SHADOWS_Z = -0.24f;
        private float bounceLinesLength = 0.7f;

        public void Show(AimObject aimData, Transform ballsRoot, bool isBlocked)
        {
            gameObject.SetActive(true);
            collisionPosition.localPosition = ballsRoot.TransformPoint(new Vector2(aimData.AimBallX, aimData.AimBallY) * Config.MODEL_COORD_TO_WORLD_KOEF);
            collisionIcon.sprite = isBlocked ? collisionSpriteBlock : collisionSpriteNormal;

            lineToCollision.gameObject.SetActive(true);
            lineToCollisionShadow.gameObject.SetActive(true);
            lineToCollision.positionCount =lineToCollisionShadow.positionCount = 2;
            var p0 = new Vector3(
                aimData.AimBallX0 * Config.MODEL_COORD_TO_WORLD_KOEF, 
                aimData.AimBallY0 * Config.MODEL_COORD_TO_WORLD_KOEF,
                LINES_Z);
            var p1 = new Vector3(
                aimData.AimBallX * Config.MODEL_COORD_TO_WORLD_KOEF, 
                aimData.AimBallY * Config.MODEL_COORD_TO_WORLD_KOEF,
                LINES_Z);
            lineToCollision.SetPosition(0, ballsRoot.TransformPoint(p0));
            lineToCollision.SetPosition(1, ballsRoot.TransformPoint(p1));
            lineToCollisionShadow.SetPosition(0, ballsRoot.TransformPoint(new Vector3(p0.x,p0.y,LINES_SHADOWS_Z)));
            lineToCollisionShadow.SetPosition(1, ballsRoot.TransformPoint(new Vector3(p1.x, p1.y, LINES_SHADOWS_Z)));

            if (aimData.FirstCollBallNum < 0 || isBlocked)
            {
                lineFromCollision.gameObject.SetActive(false);
                lineFromCollisionShadow.gameObject.SetActive(false);
                lineFromAimBall.gameObject.SetActive(false);
                lineFromAimBallShadow.gameObject.SetActive(false);
            }
            else
            {
                lineFromCollision.gameObject.SetActive(true);
                lineFromCollisionShadow.gameObject.SetActive(true);
                lineFromCollision.positionCount = lineFromCollisionShadow.positionCount = 2;
                p0 = new Vector3(
                    aimData.AimBallX * Config.MODEL_COORD_TO_WORLD_KOEF, 
                    aimData.AimBallY * Config.MODEL_COORD_TO_WORLD_KOEF, 
                    LINES_Z);
                p1 = new Vector3(
                    (aimData.AimBallX * Config.MODEL_COORD_TO_WORLD_KOEF + aimData.AimBallBounceVx * bounceLinesLength),
                    (aimData.AimBallY * Config.MODEL_COORD_TO_WORLD_KOEF + aimData.AimBallBounceVy * bounceLinesLength),
                    LINES_Z);
                lineFromCollision.SetPosition(0, ballsRoot.TransformPoint(p0));
                lineFromCollision.SetPosition(1, ballsRoot.TransformPoint(p1));
                lineFromCollisionShadow.SetPosition(0, ballsRoot.TransformPoint(new Vector3(p0.x, p0.y, LINES_SHADOWS_Z)));
                lineFromCollisionShadow.SetPosition(1, ballsRoot.TransformPoint(new Vector3(p1.x, p1.y, LINES_SHADOWS_Z)));

                lineFromAimBall.gameObject.SetActive(true);
                lineFromAimBallShadow.gameObject.SetActive(true);
                lineFromAimBall.positionCount = lineFromAimBallShadow.positionCount = 2;
                p0 = new Vector3(
                    aimData.CollBallX0 * Config.MODEL_COORD_TO_WORLD_KOEF, 
                    aimData.CollBallY0 * Config.MODEL_COORD_TO_WORLD_KOEF, 
                    LINES_Z);
                p1 = new Vector3(
                    (aimData.CollBallX0 * Config.MODEL_COORD_TO_WORLD_KOEF + aimData.CollBallBounceVx * bounceLinesLength), 
                    (aimData.CollBallY0 * Config.MODEL_COORD_TO_WORLD_KOEF + aimData.CollBallBounceVy * bounceLinesLength),
                    LINES_Z);
                lineFromAimBall.SetPosition(0, ballsRoot.TransformPoint(p0));
                lineFromAimBall.SetPosition(1, ballsRoot.TransformPoint(p1));
                lineFromAimBallShadow.SetPosition(0, ballsRoot.TransformPoint(new Vector3(p0.x, p0.y, LINES_SHADOWS_Z)));
                lineFromAimBallShadow.SetPosition(1, ballsRoot.TransformPoint(new Vector3(p1.x, p1.y, LINES_SHADOWS_Z)));
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
