using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class BrontoBurt : Enemy
    {
        private float waveAmplitude = 10f; // height of wave
        private float waveFrequency = 0.05f; // wave speed
        private float initialY; // initial height
        private float timeCounter = 0f; // wave time counter

        public BrontoBurt(Vector2 startPosition) : base(startPosition, EnemyType.BrontoBurt)
        {
            initialY = startPosition.Y;
            health = 100;
            isDead = false;
            stateMachine.ChangePose(EnemyPose.Walking);
        }

        public override void Attack()
        {
            stateMachine.ChangePose(EnemyPose.Walking);
            UpdateTexture();
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                if (stateMachine.GetPose() == EnemyPose.Walking)
                {
                    Move();
                }

                // updates using state
                UpdateTexture();
                enemySprite.Update();
            }
        }

        protected override void Move()
        {
            timeCounter += waveFrequency;

            // Y oscillation using sin. Smooth flying
            position.Y = initialY + waveAmplitude * (float)Math.Sin(timeCounter);

            if (stateMachine.IsLeft())
            {
                position.X -= 0.5f;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X += 0.5f;
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
