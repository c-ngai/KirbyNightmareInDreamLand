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
                stateCounter++;

                switch (currentState)
                {

                    //TO-DO: Ask if State Machine should handle this
                    //Depending on pose, will play specific number of frames and swap to other pose
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
                            enemySprite.ResetAnimation();  // Mark addition: since hop is a non-looping animation that we want to repeat but we already have that sprite, just call ResetAnimation on it.
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
                            stateMachine.ChangePose(EnemyPose.Hop);
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
            float height = (currentState == "HoppingForward") ? shortHopHeight : tallHopHeight; // Choose height based on state
            float t = (float)hopCounter / hopFrequency;

            // Smooth hops
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * height / 2);

            bool isMovingRight = !stateMachine.IsLeft();

            // Check direction for boundaries 
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

            // Reset and repeat hops
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
