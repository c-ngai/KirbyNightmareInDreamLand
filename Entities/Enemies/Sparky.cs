using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class Sparky : Enemy
    {
        private int hopCounter = 0;
        private int hopFrequency = 60; // frames between hops
        private float shortHopHeight = 1f;
        private float tallHopHeight = 2f;
        private float hopSpeed = 0.4f; // speed

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

                switch (currentState)
                {
                    case "HoppingForward":
                        Move(); // Call Move without parameters for short hop
                        if (stateCounter >= hopFrequency) // short hop
                        {
                            stateCounter = 0;
                            currentState = "Pausing";
                        }
                        break;

                    case "Pausing":
                        if (stateCounter >= 30) // pause
                        {
                            stateCounter = 0;
                            currentState = "HoppingTall";
                        }
                        break;

                    case "HoppingTall":
                        Move(); // Call Move without parameters for tall hop
                        if (stateCounter >= hopFrequency) // tall hop
                        {
                            stateCounter = 0;
                            currentState = "PausingAgain";
                        }
                        break;

                    case "PausingAgain":
                        if (stateCounter >= 30) // Pause 
                        {
                            stateCounter = 0;
                            currentState = "Attacking";
                        }
                        break;

                    case "Attacking":
                        Attack();
                        if (stateCounter >= 120) // Attack
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

        // Move method updated to have no parameters
        protected override void Move()
        {
            hopCounter++;
            float height = (currentState == "HoppingForward") ? shortHopHeight : tallHopHeight; // Choose height based on state
            float t = (float)hopCounter / hopFrequency;

            // Smooth hops
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * height / 2); // Adjust hop height

            bool isMovingRight = !stateMachine.IsLeft();

            // Check boundaries
            if (isMovingRight)
            {
                // Move right. If passed right boundary, switch direction
                position.X += hopSpeed;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection(); // Use method from base class
                }
            }
            else
            {
                // Move left. If passed left boundary, switch direction
                position.X -= hopSpeed;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection(); // Use method from base class
                }
            }

            // Reset and repeat
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
