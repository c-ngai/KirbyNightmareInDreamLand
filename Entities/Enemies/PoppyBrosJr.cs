using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.PoppyBrosJrState;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.SparkyState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class PoppyBrosJr : Enemy
    {
        private bool landed = false;
        private bool mainDirection;

        public PoppyBrosJr(Vector2 startPosition) : base(startPosition, EnemyType.PoppyBrosJr)
        {
            affectedByGravity = true;
            gravity = Constants.PoppyBrosJr.GRAVITY;
        }

        public override void Spawn()
        {
            base.Spawn();
            stateMachine.ChangePose(EnemyPose.Hop);
            currentState = new PoppyBrosJrHopState(this);
            mainDirection = stateMachine.IsLeft();
        }

        public override void Move()
        {
            base.Move();
            velocity.X = stateMachine.IsLeft() ? -Constants.PoppyBrosJr.MOVE_SPEED : Constants.PoppyBrosJr.MOVE_SPEED;
        }

        public override void Jump()
        {
            if (landed)
            {
                // Change direction if facing away from the main direction, or on a random 1/4 chance
                if (stateMachine.IsLeft() != mainDirection || random.Next(0, 4) == 0)
                {
                    ChangeDirection();
                }
                ChangePose(EnemyPose.Hop);
                enemySprite.ResetAnimation();
                velocity.Y = Constants.PoppyBrosJr.JUMP_VELOCITY;
                landed = false;
            }
        }

        public override void BottomCollisionWithBlock(Rectangle intersection)
        {
            base.BottomCollisionWithBlock(intersection);
            
            landed = true;
            // Note (Mark) THIS IS A BIT JANK
            // Basically: if colliding with a block from above, change to walking state if jumping
            if (currentState.GetType().Equals(typeof(PoppyBrosJrHopState)))
            {
                ChangeState(new PoppyBrosJrLandState(this));
            }
        }

        public override void BottomCollisionWithPlatform(Rectangle intersection)
        {
            base.BottomCollisionWithPlatform(intersection);

            landed = true;
            // Note (Mark) THIS IS A BIT JANK
            // Basically: if colliding with a block from above, change to walking state if jumping
            if (currentState.GetType().Equals(typeof(PoppyBrosJrHopState)))
            {
                ChangeState(new PoppyBrosJrLandState(this));
            }
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
                    landed = true;
                    // TODO: remove band-aid. waddle doo is always still inside the slope on the first frame of his jump, but he shouldn't be. check the order that velocity and position changes happen. position should change by velocity ONCE at the end of an update
                    if (currentState.GetType().Equals(typeof(PoppyBrosJrHopState)) && frameCounter > 0)
                    {
                        ChangeState(new PoppyBrosJrLandState(this));
                    }
                    velocity.Y = 0;
                }
                //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
            }
        }


    }
}
