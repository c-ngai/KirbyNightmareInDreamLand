using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using static MasterGame.Constants.BrontoBurtConst;

namespace MasterGame.Entities.Enemies
{
    public class BrontoBurt : Enemy
    {
        //Keep track of current frame
        private int frameCounter = 0;

        private readonly float initialY; // initial height
        private float timeCounter = 0f; // wave time counter

        public BrontoBurt(Vector2 startPosition) : base(startPosition, EnemyType.BrontoBurt)
        {
            //Initialize starting Y position,
            initialY = startPosition.Y;
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
                frameCounter++;

                //TO-DO: Change switch case into state pattern design
                switch (stateMachine.GetPose())
                {
                    case EnemyPose.FlyingSlow:
                        Move();
                        // Transition to Hurt state after hopFrames
                        if (frameCounter >= SLOW_FLY_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.FlyingFast);
                            frameCounter = 0; // Reset frame counter
                            UpdateTexture();  // Update texture for the new pose
                        }
                        break;
                    case EnemyPose.FlyingFast:
                        Move();
                        // Transition back to walking after hurtFrames
                        if (frameCounter >= FAST_FLY_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Hurt);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                    case EnemyPose.Hurt:
                        // Transition back to walking after hurtFrames
                        if (frameCounter >= HURT_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Standing);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                    case EnemyPose.Standing:
                        // Transition back to walking after hurtFrames
                        if (frameCounter >= STANDING_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.FlyingSlow);
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
            //Creats Y oscillation using sin. Smooth flying motion up and down
            timeCounter += WAVE_FREQUENCY;
            position.Y = initialY + WAVE_AMPLITUDE * (float)Math.Sin(timeCounter);

            //Checks to change if X value is within left/right bounds
            if (stateMachine.IsLeft())
            {
                position.X -= MOVE_SPEED;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X += MOVE_SPEED;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw if enemy is alive
            if (!isDead)
            {
                enemySprite.Draw(position, spriteBatch);
            }
        }
    }
}
