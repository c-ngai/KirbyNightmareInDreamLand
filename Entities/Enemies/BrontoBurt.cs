using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class BrontoBurt : Enemy
    {

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
            //Note: Does not have attack pose
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
            timeCounter += Constants.BrontoBurt.WAVE_FREQUENCY;

            // Y oscillation using sin. Smooth flying motion up and down
            position.Y = initialY + Constants.BrontoBurt.WAVE_AMPLITUDE * (float)Math.Sin(timeCounter);

            //Checks to change if within bounds
            if (stateMachine.IsLeft())
            {
                position.X -= Constants.BrontoBurt.MOVE_SPEED;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X += Constants.BrontoBurt.MOVE_SPEED;
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
