using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class PoppyBrosJr : Enemy
    {
        private int hopCounter = 0; //number of hops

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
            // Walking back and forth until left/right boundary
            if (stateMachine.IsLeft())
            {
                position.X -= Constants.PoppyBrosJr.MOVE_SPEED;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X += Constants.PoppyBrosJr.MOVE_SPEED;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                }
            }
        }

        private void Hop()
        {
            hopCounter++;
            float t = (float)hopCounter / Constants.PoppyBrosJr.HOP_FREQUENCY;

            // Smooth hopping math
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * Constants.PoppyBrosJr.HOP_HEIGHT / 2);

            // Reset hop counter for cycle
            if (hopCounter >= Constants.PoppyBrosJr.HOP_FREQUENCY)
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
