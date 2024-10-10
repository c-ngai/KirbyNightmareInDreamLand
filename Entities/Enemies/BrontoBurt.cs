using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Enemies
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
                        if (frameCounter >= Constants.BrontoBurt.SLOW_FLY_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.FlyingFast);
                            frameCounter = 0; // Reset frame counter
                            UpdateTexture();  // Update texture for the new pose
                        }
                        break;
                    case EnemyPose.FlyingFast:
                        Move();
                        // Transition back to walking after hurtFrames
                        if (frameCounter >= Constants.BrontoBurt.FAST_FLY_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Hurt);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                    case EnemyPose.Hurt:
                        // Transition back to walking after hurtFrames
                        if (frameCounter >= Constants.BrontoBurt.HURT_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Standing);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                    case EnemyPose.Standing:
                        // Transition back to walking after hurtFrames
                        if (frameCounter >= Constants.BrontoBurt.STANDING_FRAMES)
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
            timeCounter += Constants.BrontoBurt.WAVE_FREQUENCY;
            position.Y = initialY + Constants.BrontoBurt.WAVE_AMPLITUDE * (float)Math.Sin(timeCounter);

            //Checks to change if X value is within left/right bounds
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
            //Draw if enemy is alive
            if (!isDead)
            {
                enemySprite.LevelDraw(position, spriteBatch);
            }
        }
    }
}
