using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class BrontoBurt : IEnemy
    {
        private Vector2 position;
        private int health;
        private bool isDead;
        private Sprite enemySprite;
        private EnemyStateMachine stateMachine;
        private Vector2 leftBoundary = new Vector2(100, 100);
        private Vector2 rightBoundary = new Vector2(230, 100);
        private string oldState;

        private float waveAmplitude = 10f; //height of wave
        private float waveFrequency = 0.05f; //wave speed
        private float initialY; //initial height
        private float timeCounter = 0f; //wave time counter

        public BrontoBurt(Vector2 startPosition)
        {
            position = startPosition;
            initialY = startPosition.Y;
            health = 100;
            isDead = false;
            stateMachine = new EnemyStateMachine(EnemyType.BrontoBurt);
            //stateMachine = new EnemyStateMachine(EnemyType.WaddleDee);
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

            stateMachine.ChangePose(EnemyPose.Hurt);
            UpdateTexture();
        }

        public void Attack()
        {
            //stateMachine.ChangePose(EnemyPose.Walking);
            stateMachine.ChangePose(EnemyPose.FlyingSlow);
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
               //if (stateMachine.GetPose() == EnemyPose.Walking)
                if (stateMachine.GetPose() == EnemyPose.FlyingSlow)
                    {
                    Move();
                }

                //updates using state
                UpdateTexture();
                enemySprite.Update();
            }
        }

        private void Move()
        {
            timeCounter += waveFrequency;

            //Y oscillation using sin. smooth flying
            position.Y = initialY + waveAmplitude * (float)Math.Sin(timeCounter);

            if (stateMachine.IsLeft())
            {
                position.X -= 0.5f;
                if (position.X <= leftBoundary.X)
                {
                    stateMachine.ChangeDirection();
                    UpdateTexture();
                }
            }
            else
            {
                position.X += 0.5f;
                if (position.X >= rightBoundary.X)
                {
                    stateMachine.ChangeDirection();
                    UpdateTexture();
                }
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
