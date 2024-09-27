using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class PoppyBrosJr : Enemy
    {
        // Hopping
        private int hopCounter = 0;
        private int hopFrequency = 60; // frames between hops
        private float hopHeight = 1f;

        public PoppyBrosJr(Vector2 startPosition) : base(startPosition, EnemyType.PoppyBrosJr)
        {
            stateMachine.ChangePose(EnemyPose.Walking);
        }

        public override void Attack()
        {
            stateMachine.ChangePose(EnemyPose.Attacking);
            UpdateTexture();
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                // Walking left/right
                if (stateMachine.GetPose() == EnemyPose.Walking)
                {
                    Move();
                    Hop();
                }

                // Update texture and enemy sprite
                UpdateTexture();
                enemySprite.Update();
            }
        }

        protected override void Move()
        {
            // Walking back and forth
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

        private void Hop()
        {
            hopCounter++;
            float t = (float)hopCounter / hopFrequency;

            // Smooth hopping
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * hopHeight / 2);

            // Reset hop counter for cycle
            if (hopCounter >= hopFrequency)
            {
                hopCounter = 0;
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
