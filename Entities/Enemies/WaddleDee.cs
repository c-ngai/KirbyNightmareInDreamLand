using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class WaddleDee : IEnemy
    {
        private Vector2 position;
        private int health;
        private int damage;
        private ISprite sprite;
        private IEnemyStateMachine stateMachine;
        private bool isDead;

        private Vector2 leftBoundary = new Vector2(100, 100);
        private Vector2 rightBoundary = new Vector2(300, 100);

        public WaddleDee(Vector2 startPosition)
        {
            position = startPosition;
            health = 100; // health value
            damage = 10;  // damage default
            stateMachine = new EnemyStateMachine();
            stateMachine.ChangeType(EnemyType.WaddleDee);
            stateMachine.ChangePose(EnemyPose.Walking);
            isDead = false;

            //initialize sprite
            sprite = SpriteFactory.Instance.createSprite("kirby_normal_walking");
        }

        //QUESTION: Should we have a int damageTaken param for this?
        public void TakeDamage(/*int damageTaken*/)
        {

            //hp goes down 10
            health -= 10;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        private void Die()
        {
            //DEATH/DAMAGE ENUM? Placeholder for rn
            stateMachine.ChangePose(EnemyPose.LoadingAttack); 

            //flag as dead
            isDead = true;
        }

        public void Attack()
        {
            //Add actual attacks after after collision is added
            //For right now just a pose state change
            stateMachine.ChangePose(EnemyPose.Attacking);
        }

        //QUESTION: Do we need gameTime param for Update?
        public void Update(/*GameTime gameTime*/)
        {
            if (!isDead)
            {
                // Update WaddleDee position or other logic
                if (stateMachine.GetPose() == EnemyPose.Walking)
                {
                    Move();
                }

                // Update sprite based on state. Do I have gameTime?
                //sprite.Update(gameTime, stateMachine.GetSpriteParameters());
                sprite.Update();
            }
        }

        //right now is hard coded movement/placement
        private void Move()
        {
      
            if (stateMachine.IsLeft())
            {
                position.X -= 1;

                //if waddledee position is less than __ postion, turn left
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection(); // Reverse direction
                }
            }
            else
            {
                position.X += 1;
                //if waddledee position greater than __ position, turn right
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection(); // Reverse direction
                }
            }

        }

        //QUESTION: Do we need a spriteBatch param here?
        public void Draw(/*SpriteBatch spriteBatch*/)
        {

            if (!isDead)
            {
                //draw sprite
                //sprite.Draw(spriteBatch, position, stateMachine.IsLeft());
                sprite.Draw(new Vector2(100, 130));
            }
        }
        public void ChangeDirection()
        {
            stateMachine.ChangeDirection();
        }

       
    }

}
