using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState;
using KirbyNightmareInDreamLand.Levels;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class BrontoBurt : Enemy
    {
        private int sineCounter; // wave time counter
        private float lastYVelocity; // janky way of tracking accelleration

        public BrontoBurt(Vector2 startPosition) : base(startPosition, EnemyType.BrontoBurt)
        {
            affectedByGravity = false;
        }

        public override void Spawn()
        {
            base.Spawn();
            ChangePose(EnemyPose.FlyingSlow);
            ChangeState(new BrontoBurtFlyingState(this)); // Set initial state
            sineCounter = 0;

            //velocity.Y = 0;
            //velocity.X = Constants.BrontoBurt.MOVE_SPEED;
        }

        public override void Move()
        {
            base.Move();
            // Increment the time counter to maintain oscillation
            sineCounter++;

            // Oscillate vertically using a sine wave pattern
            velocity.Y = Constants.BrontoBurt.WAVE_AMPLITUDE * (float)Math.Cos(sineCounter * Constants.BrontoBurt.WAVE_FREQUENCY);
            position.Y += velocity.Y; // Apply the Y-axis oscillation

            // Move horizontally according to direction and speed
            velocity.X = stateMachine.IsLeft() ? -Constants.BrontoBurt.MOVE_SPEED : Constants.BrontoBurt.MOVE_SPEED;

            // if accellerating upward (not to be confused with moving upward)
            if (velocity.Y < lastYVelocity)
            {
                ChangePose(EnemyPose.FlyingFast);
            }
            // if accellerating downward (not to be confused with moving downward)
            else
            {
                ChangePose(EnemyPose.FlyingSlow);
            }

            lastYVelocity = velocity.Y;
        }


        public override void BottomCollisionWithBlock(Rectangle intersection)
        {
            // Do nothing, Bronto Burt does not collide with tiles
        }

        public override void TopCollisionWithBlock(Rectangle intersection)
        {
            // Do nothing, Bronto Burt does not collide with tiles
        }

        public override void RightCollisionWithBlock(Rectangle intersection)
        {
            // Do nothing, Bronto Burt does not collide with tiles
        }

        public override void LeftCollisionWithBlock(Rectangle intersection)
        {
            // Do nothing, Bronto Burt does not collide with tiles
        }

        public override void BottomCollisionWithPlatform(Rectangle intersection)
        {
            // Do nothing, Bronto Burt does not collide with tiles
        }
        public override void AdjustOnSlopeCollision(Tile tile, float slope, float yIntercept)
        {
            // Do nothing, Bronto Burt does not collide with tiles
        }

    }
}
