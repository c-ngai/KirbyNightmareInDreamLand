using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class PoppyBrosJr : Enemy
    {
        private const float MoveSpeed = 0.5f;
        // Hopping Variables
        private int hopCounter = 0; //number of hops
        private int hopFrequency = 60; // frames between hops
        private float hopHeight = 1f; //height of hops

        public PoppyBrosJr(Vector2 startPosition) : base(startPosition, EnemyType.PoppyBrosJr)
        {
            stateMachine.ChangePose(EnemyPose.Hop);
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
                if (stateMachine.GetPose() == EnemyPose.Hop)
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
            // Walking back and forth until left/right boundary
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

        private void Hop()
        {
            hopCounter++;
            float t = (float)hopCounter / hopFrequency;

            // Smooth hopping math
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * hopHeight / 2);

            // Reset hop counter for cycle
            if (hopCounter >= hopFrequency)
            {
                hopCounter = 0;
                enemySprite.ResetAnimation(); // Mark addition: since hop is a non-looping animation that we want to repeat but we already have that sprite, just call ResetAnimation on it.
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
