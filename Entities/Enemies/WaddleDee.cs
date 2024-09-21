using Microsoft.Xna.Framework;

namespace MasterGame
{
    public class WaddleDee : IEnemy
    {
        private Vector2 position;
        private int health;
        private bool isDead;
        private Sprite enemySprite;
        private IEnemyStateMachine stateMachine;
        private Vector2 leftBoundary = new Vector2(170, 100);
        private Vector2 rightBoundary = new Vector2(210, 100);

        public WaddleDee(Vector2 startPosition)
        {
            position = startPosition;
            health = 100;
            isDead = false;
            stateMachine = new EnemyStateMachine(EnemyType.WaddleDee);
            stateMachine.ChangePose(EnemyPose.Walking);

            //need to add eventual waddledee animation
            enemySprite = SpriteFactory.Instance.createSprite("kirby_normal_standing_right");
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
            stateMachine.ChangePose(EnemyPose.LoadingAttack);
        }

        public void Attack()
        {
            stateMachine.ChangePose(EnemyPose.Attacking);
        }

        public void Update()
        {
            if (!isDead)
            {
                //need to add walking left/right
                if (stateMachine.GetPose() == EnemyPose.Walking)
                {
                    Move();
                }

                //updates using state
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
                    ChangeDirection();
                }
            }
            else
            {
                position.X += 0.5f;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                }
            }
        }

        public void Draw()
        {
            if (!isDead)
            {
                enemySprite.Draw(position);
            }
        }

        public void ChangeDirection()
        {
            stateMachine.ChangeDirection();
        }
    }
}
