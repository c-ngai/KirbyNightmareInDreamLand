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

        private readonly float initialY; // initial height
        private float timeCounter = 0f; // wave time counter

        public BrontoBurt(Vector2 startPosition) : base(startPosition, EnemyType.BrontoBurt)
        {
            //Initialize starting Y position,
            initialY = startPosition.Y;
            stateMachine.ChangePose(EnemyPose.FlyingSlow);
            ChangeState(new BrontoBurtFlyingSlowState(this)); // Set initial state

            yVel = 0;
            xVel = Constants.BrontoBurt.MOVE_SPEED;
        }

        public override void Move()
        {
            //Creats Y oscillation using sin. Smooth flying motion up and down
            timeCounter += Constants.BrontoBurt.WAVE_FREQUENCY;
            position.Y = initialY + Constants.BrontoBurt.WAVE_AMPLITUDE * (float)Math.Sin(timeCounter);

            //Checks to change if X value is within left/right bounds
            if (stateMachine.IsLeft())
            {
                position.X -= xVel;
            }
            else
            {
                position.X += xVel;
            }
        }

    }
}
