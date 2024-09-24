using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class WaddleDoo : IEnemy
    {
        private Vector2 position;
        private int health;
        private bool isDead;
        private Sprite enemySprite;
        private EnemyStateMachine stateMachine;
        private Vector2 leftBoundary = new Vector2(100, 100);
        private Vector2 rightBoundary = new Vector2(230, 100);
        private string oldState;
        private int frameCounter = 0;
        private int walkFrames = 180;  //3 sec (if 60fps)
        private int stopFrames = 60;  //2 sec
        private int attackFrames = 100; //1 sec
        private int ichangedthis;

        public WaddleDoo(Vector2 startPosition)
        {
            position = startPosition;
            health = 100;
            isDead = false;
            stateMachine = new EnemyStateMachine(EnemyType.WaddleDoo);
            //stateMachine.ChangePose(EnemyPose.Walking);
           enemySprite = SpriteFactory.Instance.createSprite("waddledoo_walking_right");
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
            stateMachine.ChangePose(EnemyPose.Charging);
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
                frameCounter++;

                // Handle walking state
                if (stateMachine.GetPose() == EnemyPose.Walking)
                {
                    Move();

                    if (frameCounter >= walkFrames)
                    {
                        stateMachine.ChangePose(EnemyPose.Charging); // Stop after walking to load attack
                        frameCounter = 0;
                        UpdateTexture();
                    }
                }
                // Handle idle (stopped) state
                else if (stateMachine.GetPose() == EnemyPose.Charging) //If Loading Attack,
                {
                    if (frameCounter >= stopFrames)
                    {
                        stateMachine.ChangePose(EnemyPose.Attacking); // Attack after stopping
                        frameCounter = 0;
                        UpdateTexture();
                    }
                }
                // Handle attacking state
                else if (stateMachine.GetPose() == EnemyPose.Attacking) //If attacking
                {
                    if (frameCounter >= attackFrames)
                    {
                        stateMachine.ChangePose(EnemyPose.Walking); // Walk again after attacking 
                        frameCounter = 0;
                        UpdateTexture();
                    }
                }

                // Update sprite animation
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
