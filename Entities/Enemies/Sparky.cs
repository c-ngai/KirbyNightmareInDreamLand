using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MasterGame
{
    public class Sparky : Enemy
    {
        private int hopCounter = 0;
        private float shortHopHeight = 1f;
        private float tallHopHeight = 2f;
        private float hopSpeed = 0.4f; // speed

        // List of (state, frames) tuples in order
        private List<(string state, int frames)> stateFrames;
        private int currentStateIndex = 0;
        private int stateCounter = 0;

        public Sparky(Vector2 startPosition) : base(startPosition, EnemyType.Sparky)
        {
            // Initialize the list of states and corresponding frames
            stateFrames = new List<(string, int)>
            {
                ("HoppingForward", 60),    // Short hop with 60 frames
                ("Pausing", 30),           // Pause with 30 frames
                ("HoppingTall", 60),       // Tall hop with 60 frames
                ("PausingAgain", 30),      // Another pause with 30 frames
                ("Attacking", 120)         // Attacking with 120 frames
            };

            // Start with the first state
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

                // Get the current state and number of frames
                var (currentState, requiredFrames) = stateFrames[currentStateIndex];

                if (stateCounter >= requiredFrames)
                {
                    // Move to the next state in the cycle
                    currentStateIndex = (currentStateIndex + 1) % stateFrames.Count;
                    stateCounter = 0;

                    // Update pose or attack state based on the current state
                    if (currentState == "Attacking")
                    {
                        stateMachine.ChangePose(EnemyPose.Attacking);
                        Attack();
                    }
                    else
                    {
                        stateMachine.ChangePose(EnemyPose.Walking); // Reset to walking for hops
                    }
                }

                // Execute specific actions for each state
                if (currentState == "HoppingForward" || currentState == "HoppingTall")
                {
                    Move(); // Now Move doesn't need parameters
                }

                // Update texture and sprite
                UpdateTexture();
                enemySprite.Update();
            }
        }

        protected override void Move()
        {
            hopCounter++;

            // Use current state to determine hop height
            var currentState = stateFrames[currentStateIndex].state;
            float height = (currentState == "HoppingForward") ? shortHopHeight : tallHopHeight;
            float t = (float)hopCounter / stateFrames[currentStateIndex].frames;

            // Smooth hopping motion
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * height / 2);

            bool isMovingRight = !stateMachine.IsLeft();

            // Move horizontally and check for boundaries
            if (isMovingRight)
            {
                position.X += hopSpeed;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X -= hopSpeed;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
            }

            // Reset hop counter
            if (hopCounter >= stateFrames[currentStateIndex].frames)
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
