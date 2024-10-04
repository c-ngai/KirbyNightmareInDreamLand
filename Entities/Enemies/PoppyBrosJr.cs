using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class PoppyBrosJr : Enemy
    {

        //Keep track of current frame
        private int frameCounter = 0;

        private int hopCounter = 0; //number of hops

        public PoppyBrosJr(Vector2 startPosition) : base(startPosition, EnemyType.PoppyBrosJr)
        {
            //initialize first sprite
            stateMachine.ChangePose(EnemyPose.Hop);
        }

        public override void Attack()
        {
            //NOTE: Poppy Bros Jr does not have attack sprite
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                frameCounter++;

                // Switch case to handle enemy states
                switch (stateMachine.GetPose())
                {
                    case EnemyPose.Hop:
                        Move();
                        Hop();

                        // Transition to Hurt state after hopFrames
                        if (frameCounter >= Constants.PoppyBrosJr.HOP_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Hurt);
                            frameCounter = 0; // Reset frame counter
                            UpdateTexture();  // Update texture for the new pose
                        }
                        break;

                    case EnemyPose.Hurt:
                        // Transition back to Hopping after hurtFrames
                        if (frameCounter >= Constants.PoppyBrosJr.HURT_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Hop);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                }
                UpdateTexture();
                // Update the enemy sprite
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
            position.Y -= (float)(Math.Sin(t * Math.PI * 2) * Constants.PoppyBrosJr.HOP_HEIGHT / 2);

            // Reset hop counter for cycle
            if (hopCounter >= Constants.PoppyBrosJr.HOP_FREQUENCY)
            {
                hopCounter = 0;
                enemySprite.ResetAnimation(); // Mark addition: since hop is a non-looping animation that we want to repeat but we already have that sprite, just call ResetAnimation on it.
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draws if enemy is alive
            if (!isDead)
            {
                enemySprite.LevelDraw(position, spriteBatch);
            }
        }
    }
}
