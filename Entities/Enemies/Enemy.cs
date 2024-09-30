using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public abstract class Enemy : IEnemy
    {
        protected Vector2 position; //Where enemy is drawn on screen
        protected int health; //Enemy health
        protected bool isDead;  //If enemy is dead
        protected Sprite enemySprite;   
        protected EnemyStateMachine stateMachine;
        protected Vector2 leftBoundary; //Boundaries for where enemy will turn around on screen
        protected Vector2 rightBoundary;
        protected string oldState; //Previous state

        protected Enemy(Vector2 startPosition, EnemyType type)
        {
            //Initialize all variables
            position = startPosition;
            health = 1;
            isDead = false;
            stateMachine = new EnemyStateMachine(type);
            leftBoundary = new Vector2(100, 100);
            rightBoundary = new Vector2(230, 100);
            oldState = string.Empty;
        }

        public Vector2 Position
        {
            //Returns position on screen
            get { return position; }
            set { position = value; }
        }

        public Sprite EnemySprite
        {
            //Returns Sprite
            set { enemySprite = value; }
        }

        public void TakeDamage()
        {
            //If damage is taken, the enemy's pose will change and flag isDead
            stateMachine.ChangePose(EnemyPose.Hurt);
            health -= 1;
            if (health <= 0)
            {
                health = 0;
                isDead = true;
            }
        }

        public void UpdateTexture()
        {
            if (!stateMachine.GetStateString().Equals(oldState))
            {
                enemySprite = SpriteFactory.Instance.CreateSprite(stateMachine.GetSpriteParameters());
                oldState = stateMachine.GetStateString();
            }
        }

        public void ChangeDirection()
        {
            //Changes direction from right to left
            stateMachine.ChangeDirection();
        }

        // Abstract methods to be implemented by subclasses, since they all differ between enemies.
        public abstract void Update(GameTime gameTime);
        protected abstract void Move();
        public abstract void Draw(SpriteBatch spritebatch);

        public abstract void Attack();
    }
}
