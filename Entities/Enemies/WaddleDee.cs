using Microsoft.Xna.Framework;

namespace MasterGame
{
    public class WaddleDee : IEnemy
    {
        private Vector2 position;
        private int health;
        private bool isDead;
        private Sprite enemySprite;
        private EnemyStateMachine stateMachine;
        private Vector2 leftBoundary = new Vector2(170, 100);
        private Vector2 rightBoundary = new Vector2(210, 100);
        private string oldState;

        public WaddleDee(Vector2 startPosition)
        {
            position = startPosition;
            health = 100;
            isDead = false;
            stateMachine = new EnemyStateMachine(EnemyType.WaddleDee);
            //stateMachine.ChangePose(EnemyPose.Walking);
           enemySprite = SpriteFactory.Instance.createSprite("waddledee_walking_right");
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
