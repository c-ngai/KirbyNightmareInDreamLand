using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class Sparky : Enemy
    {
        private int hopCounter = 0;
        private int stateCounter = 0;
        private string currentState = "HoppingForward"; // state name

        public Sparky(Vector2 startPosition) : base(startPosition, EnemyType.Sparky)
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
                stateCounter++;

                //TO-DO: Change switch case into state pattern design
                switch (currentState)
                {
                    case "HoppingForward":
                        Move(); // Call Move without parameters for short hop
                        if (stateCounter >= Constants.Sparky.HOP_FREQUENCY) // short hop
                        {
                            stateCounter = 0;
                            currentState = "Pausing";
                        }
                        break;

                    case "Pausing":
                        if (stateCounter >= Constants.Sparky.PAUSE_TIME) // pause
                        {
                            stateCounter = 0;
                            currentState = "HoppingTall";
                        }
                        break;

                    case "HoppingTall":
                        Move(); // Call Move without parameters for tall hop
                        if (stateCounter >= Constants.Sparky.HOP_FREQUENCY) // tall hop
                        {
                            stateCounter = 0;
                            currentState = "PausingAgain";
                        }
                        break;

                    case "PausingAgain":
                        if (stateCounter >= Constants.Sparky.PAUSE_TIME) // Pause 
                        {
                            stateCounter = 0;
                            currentState = "Attacking";
                        }
                        break;

                    case "Attacking":
                        Attack();
                        if (stateCounter >= Constants.Sparky.ATTACK_TIME) // Attack
                        {
                            stateCounter = 0;
                            currentState = "HoppingForward"; // back to hop
                            stateMachine.ChangePose(EnemyPose.Walking);
                        }
                        break;
                }

                // Update texture and enemy sprite
                UpdateTexture();
                enemySprite.Update();
            }
        }
        protected override void Move()
        {
            hopCounter++;
            float height = (currentState == "HoppingForward") ? Constants.Sparky.SHORT_HOP_HEIGHT : Constants.Sparky.TALL_HOP_HEIGHT; // Choose height based on state
            float t = (float)hopCounter / Constants.Sparky.HOP_FREQUENCY;

            // Smooth hops
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * height / 2);

            bool isMovingRight = !stateMachine.IsLeft();

            // Check direction for boundaries 
            if (isMovingRight)
            {
                position.X += Constants.Sparky.HOP_SPEED;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X -= Constants.Sparky.HOP_SPEED;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
            }

            // Reset and repeat hops
            if (hopCounter >= Constants.Sparky.HOP_FREQUENCY)
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
