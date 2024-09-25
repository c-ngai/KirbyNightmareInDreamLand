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

        private int stateCounter = 0;
        private string currentState = "HoppingForward"; //state name

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
                        Hop(tallHopHeight);
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

        private void Hop(float height)
        {
            hopCounter++;
            float t = (float) hopCounter / hopFrequency;

            //smooth hops
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * height / 2); // Adjust hop height

            bool isMovingRight = !stateMachine.IsLeft();

            //check boundaries
            if (isMovingRight)
            {
                //move right. if passed right boundary, switch direction
                position.X += hopSpeed; 
                if (position.X >= rightBoundary.X) 
                {
                    stateMachine.ChangeDirection();
                    UpdateTexture();
                }
            }
            else
            {
                //move left. if passed left boundary, switch direction
                position.X -= hopSpeed; 
                if (position.X <= leftBoundary.X) 
                {
                    stateMachine.ChangeDirection();
                    UpdateTexture();
                }
            }

            // reset and repeat
            if (hopCounter >= hopFrequency)
            {
                hopCounter = 0; 
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
