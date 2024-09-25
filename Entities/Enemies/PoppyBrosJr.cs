using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class PoppyBrosJr : IEnemy
    {
        private Vector2 position;
        private int health;
        private bool isDead;
        private Sprite enemySprite;
        private EnemyStateMachine stateMachine;
        private Vector2 leftBoundary = new Vector2(100, 100);
        private Vector2 rightBoundary = new Vector2(230, 100);
        private string oldState;

        //hopping
        private int hopCounter = 0;
        private int hopFrequency = 60; //framws between hops
        private float hopHeight = 1f;

        public PoppyBrosJr(Vector2 startPosition)
        {
            position = startPosition;
            health = 100;
            isDead = false;
            //stateMachine = new EnemyStateMachine(EnemyType.PoppyBrosJr);
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
                //need to add walking left/right
                if (stateMachine.GetPose() == EnemyPose.Walking)
                {
                    Move();
                    Hop();
                }

                //updates using state
                UpdateTexture();
                enemySprite.Update();
            }
        }

        private void Move()
        {
            //walking back and forth
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

        private void Hop()
        {
            hopCounter++;
            float t = (float)hopCounter / hopFrequency;

            //smooth hopping
            position.Y = position.Y - (float)(Math.Sin(t * Math.PI * 2) * hopHeight / 2);

            //reset hop counter for cycle
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
