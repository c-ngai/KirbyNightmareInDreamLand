using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.PoppyBrosJrState;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class PoppyBrosJr : Enemy
    {
        private bool isJumping = false;
        public bool IsJumping => isJumping;

        public PoppyBrosJr(Vector2 startPosition) : base(startPosition, EnemyType.PoppyBrosJr)
        {
            stateMachine.ChangePose(EnemyPose.Hop);
            ChangeState(new PoppyBrosJrHopState(this));
            yVel = 0;
            xVel = Constants.PoppyBrosJr.MOVE_SPEED;
        }

        public override void Jump()
        {
            if (!isJumping)
            {
                // Start jumping and store initial y
                isJumping = true;
                yVel = -Constants.PoppyBrosJr.JUMP_VELOCITY;
            }
            Move();
        }

        public override void BottomCollisionWithBlock(Rectangle intersection)
        {
            isFalling = false;
            isJumping = false;
            position.Y = intersection.Y;

            // Note (Mark) THIS IS A BIT JANK
            // Basically: if colliding with a block from above, change to walking state if jumping
            if (currentState.GetType().Equals(typeof(PoppyBrosJrHopState)))
            {
                ChangeState(new PoppyBrosJrLandState(this));
            }
            yVel = 0;
        }

        public override void AdjustOnSlopeCollision(Tile tile, float slope, float yIntercept)
        {

            //GameDebug.Instance.LogPosition(position);

            Rectangle intersection = tile.rectangle;
            if (position.X > intersection.Left && position.X < intersection.Right)
            {
                float offset = position.X - intersection.X;
                //Debug.WriteLine($"Starting Y position: {position.Y}");
                float slopeY = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
                //GameDebug.Instance.LogPosition(intersection.Location.ToVector2());
                if (position.Y > slopeY)
                {
                    position.Y = slopeY;

                    isFalling = false;
                    isJumping = false;
                    // TODO: remove band-aid. waddle doo is always still inside the slope on the first frame of his jump, but he shouldn't be. check the order that velocity and position changes happen. position should change by velocity ONCE at the end of an update
                    if (currentState.GetType().Equals(typeof(PoppyBrosJrHopState)) && frameCounter > 0)
                    {
                        ChangeState(new PoppyBrosJrLandState(this));
                    }
                    yVel = 0;
                }
                //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
            }
        }
    }
}
