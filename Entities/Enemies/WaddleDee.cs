using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class WaddleDee : Enemy
    {
        private const float MoveSpeed = 0.5f;

        public WaddleDee(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDee)
        {
            stateMachine.ChangePose(EnemyPose.Walking); // Set initial pose
            UpdateTexture(); // Update the sprite based on the initial state
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                if (stateMachine.GetPose() == EnemyPose.Walking)
                {
                    Move(); // Call the move logic
                }

                // Update sprite based on state
                UpdateTexture();
                enemySprite.Update();
            }
        }

        protected override void Move()
        {
            // Implementing the movement logic
            if (stateMachine.IsLeft())
            {
                position.X -= MoveSpeed;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection(); // Change direction if hitting left boundary
                }
            }
            else
            {
                position.X += MoveSpeed;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection(); // Change direction if hitting right boundary
                }
            }
        }

        public override void Attack()
        {
            stateMachine.ChangePose(EnemyPose.Attacking);
            UpdateTexture();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                enemySprite.Draw(position, spriteBatch);
            }
        }
    }
}
