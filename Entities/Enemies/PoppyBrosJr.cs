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
            //initialize first sprite
            stateMachine.ChangePose(EnemyPose.Walking);
        }

        public override void Attack()
        {
            //NOTE: Poppy Bros Jr does not have attack sprite
        }

        public override void Update(GameTime gameTime)
        {
            //Enemy hops left and right on screen and updates
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
            // Handles x movement. Walking back and forth until left/right boundary
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
            //Handles Y movement and calculates oscillation of hops.
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
            //Draws if enemy is alive
            if (!isDead)
            {
                enemySprite.Draw(position, spriteBatch);
            }
        }
    }
}
