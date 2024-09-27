using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public abstract class Enemy : IEnemy
    {
        protected Vector2 position;
        protected int health;
        protected bool isDead;
        protected Sprite enemySprite;
        protected EnemyStateMachine stateMachine;
        protected Vector2 leftBoundary;
        protected Vector2 rightBoundary;
        protected string oldState;

        protected Enemy(Vector2 startPosition, EnemyType type)
        {
            position = startPosition;
            health = 100;
            isDead = false;
            stateMachine = new EnemyStateMachine(type);
            leftBoundary = new Vector2(100, 100);
            rightBoundary = new Vector2(230, 100);
            oldState = string.Empty;
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

        public void UpdateTexture()
        {
            if (!stateMachine.GetStateString().Equals(oldState))
            {
                enemySprite = SpriteFactory.Instance.createSprite(stateMachine.GetSpriteParameters());
                oldState = stateMachine.GetStateString();
            }
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeDirection();
        }

        // Abstract methods to be implemented by subclasses
        public abstract void Update(GameTime gameTime);
        protected abstract void Move();

        public abstract void Draw(SpriteBatch spritebatch);

        public abstract void Attack(); // Implemented in subclasses as needed
    }
}
