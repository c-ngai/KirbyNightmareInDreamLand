using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class BrontoBurt : Enemy
    {
        private float timeCounter = 0f; // wave time counter

        public BrontoBurt(Vector2 startPosition) : base(startPosition, EnemyType.BrontoBurt)
        {
            //Initialize starting Y position,
            stateMachine.ChangePose(EnemyPose.FlyingSlow);
            ChangeState(new BrontoBurtFlyingSlowState(this)); // Set initial state

            yVel = 0;
            xVel = Constants.BrontoBurt.MOVE_SPEED;
        }

        public override void Move()
        {
            // Increment the time counter to maintain oscillation
            timeCounter += Constants.BrontoBurt.WAVE_FREQUENCY;

            // Oscillate vertically using a sine wave pattern
            yVel = Constants.BrontoBurt.WAVE_AMPLITUDE * (float)Math.Sin(timeCounter);
            position.Y += yVel; // Apply the Y-axis oscillation

            // Move horizontally according to direction and speed
            if (stateMachine.IsLeft())
            {
                position.X -= xVel;
            }
            else
            {
                position.X += xVel;
            }

            // Update texture if needed
            UpdateTexture();
        }

        public override void Fall()
        {
            //empty- no falling
        }

    }
}
