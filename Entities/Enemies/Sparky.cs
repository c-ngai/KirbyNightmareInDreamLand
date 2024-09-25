using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class Sparky : IEnemy
    {
        private Vector2 position;
        private int health;
        private bool isDead;
        private Sprite enemySprite;
        private EnemyStateMachine stateMachine;
        private Vector2 leftBoundary = new Vector2(100, 100);
        private Vector2 rightBoundary = new Vector2(230, 100);
        private string oldState;

        private int hopCounter = 0;
        private int hopFrequency = 60; // frames between hops
        private float shortHopHeight = 1f;
        private float tallHopHeight = 2f;
        private float hopSpeed = 0.4f; // speed

        private int stateCounter = 0; // Counter for tracking state durations
        private string currentState = "HoppingForward"; // Current state name

        public Sparky(Vector2 startPosition)
        {
            position = startPosition;
            health = 100;
            isDead = false;
            //stateMachine = new EnemyStateMachine(EnemyType.Sparky);
            stateMachine = new EnemyStateMachine(EnemyType.WaddleDee);
            stateMachine.ChangePose(EnemyPose.Walking);
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Sprite EnemySprite
        {
            set { enemySprite = value; }
        }

        public void TakeDamage()
        {
            stateMachine.ChangePose(EnemyPose.Hurt);
            health -= 10;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        private void Die()
        {
            isDead = true;

            //eventual death pose/animation
            stateMachine.ChangePose(EnemyPose.Hurt);
            UpdateTexture();
        }

        public void Attack()
        {
            stateMachine.ChangePose(EnemyPose.Attacking);
            UpdateTexture();
        }

        public void UpdateTexture()
        {
             if(!stateMachine.GetStateString().Equals(oldState)){
                enemySprite = SpriteFactory.Instance.createSprite(stateMachine.GetSpriteParameters());
                 oldState = stateMachine.GetStateString();
             } 
        }

        public void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                stateCounter++;

                switch (currentState)
                {
                    case "HoppingForward":
                        Hop(shortHopHeight);
                        if (stateCounter >= hopFrequency) // Short hop duration
                        {
                            stateCounter = 0;
                            currentState = "Pausing"; // Transition to pause
                        }
                        break;

                    case "Pausing":
                        if (stateCounter >= 30) // Pause duration
                        {
                            stateCounter = 0;
                            currentState = "HoppingTall"; // Transition to tall hop
                        }
                        break;

                    case "HoppingTall":
                        Hop(tallHopHeight);
                        if (stateCounter >= hopFrequency) // Tall hop duration
                        {
                            stateCounter = 0;
                            currentState = "PausingAgain"; // Transition to pause again
                        }
                        break;

                    case "PausingAgain":
                        if (stateCounter >= 30) // Pause duration
                        {
                            stateCounter = 0;
                            currentState = "Attacking"; // Transition to attack
                        }
                        break;

                    case "Attacking":
                        Attack();
                        if (stateCounter >= 120) // Attack duration
                        {
                            stateCounter = 0;
                            currentState = "HoppingForward"; // Cycle back to hopping
                            stateMachine.ChangePose(EnemyPose.Walking); // Set walking pose again
                        }
                        break;
                }

                // Update texture and enemy sprite
                UpdateTexture();
                enemySprite.Update();
            }
        }

        private void Hop(float height)
        {
            hopCounter++;
            float t = (float)hopCounter / hopFrequency; // Normalize hopCounter to [0, 1]

            // Smooth vertical movement using sine function
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * height / 2); // Adjust hop height

            // Determine direction from the state machine
            bool isMovingRight = !stateMachine.IsLeft(); // Assuming IsLeft() returns true if moving left

            // Forward movement with boundary checking
            if (isMovingRight)
            {
                position.X += hopSpeed; // Move forward
                if (position.X >= rightBoundary.X) // Check right boundary
                {
                    stateMachine.ChangeDirection(); // Change direction
                    UpdateTexture(); // Update texture if needed
                }
            }
            else
            {
                position.X -= hopSpeed; // Move backward
                if (position.X <= leftBoundary.X) // Check left boundary
                {
                    stateMachine.ChangeDirection(); // Change direction
                    UpdateTexture(); // Update texture if needed
                }
            }

            // Reset hopCounter to create a repeating cycle
            if (hopCounter >= hopFrequency)
            {
                hopCounter = 0; // Reset for the next hop cycle
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                enemySprite.Draw(position, spriteBatch);
            }
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeDirection();
        }
    }
}
