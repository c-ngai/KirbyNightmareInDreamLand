using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class BrontoBurt : Enemy
    {
        private const float MoveSpeed = 0.5f;

        //Flying movement properties
        private float waveAmplitude = 10f; // height of wave
        private float waveFrequency = 0.05f; // wave speed
        private float initialY; // initial height
        private float timeCounter = 0f; // wave time counter

        public BrontoBurt(Vector2 startPosition) : base(startPosition, EnemyType.BrontoBurt)
        {
            //Initialization
            initialY = startPosition.Y;
            health = 100;
            isDead = false;
            stateMachine.ChangePose(EnemyPose.FlyingSlow);
        }

        public override void Attack()
        {
            //TO-DO: check if Bronto Burt has an attack animation/pose
            //stateMachine.ChangePose(EnemyPose.Walking);
            //UpdateTexture();
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                if (stateMachine.GetPose() == EnemyPose.FlyingSlow)
                {
                    Move();
                }

                UpdateTexture();
                enemySprite.Update();
            }
        }

        protected override void Move()
        {
            timeCounter += waveFrequency;

            // Y oscillation using sin. Smooth flying motion up and down
            position.Y = initialY + waveAmplitude * (float)Math.Sin(timeCounter);

            //Checks to change if within bounds
            if (stateMachine.IsLeft())
            {
                position.X -= MoveSpeed;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X += MoveSpeed;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                enemySprite.Draw(position, spriteBatch);
            }
        }
    }
}
