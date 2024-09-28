using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class WaddleDee : Enemy
    {
        public WaddleDee(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDee)
        {
            //Set pose and sprite
            stateMachine.ChangePose(EnemyPose.Walking);
            UpdateTexture();
        }

        public override void Update(GameTime gameTime)
        {
            //Walk forward if alive
            if (!isDead)
            {
                if (stateMachine.GetPose() == EnemyPose.Walking)
                {
                    Move();
                }

                // Update sprite based on state
                UpdateTexture();
                enemySprite.Update();
            }
        }

        protected override void Move()
        {
            //X movement logic. Moves until boundaries
            if (stateMachine.IsLeft())
            {
                position.X -= Constants.WaddleDee.MOVE_SPEED;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection(); // Change direction if hitting left boundary
                }
            }
            else
            {
                position.X += Constants.WaddleDee.MOVE_SPEED;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection(); // Change direction if hitting right boundary
                }
            }
        }

        public override void Attack()
        {
            //WaddleDee has no attack sprite
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw if alive
            if (!isDead)
            {
                enemySprite.Draw(position, spriteBatch);
            }
        }
    }
}
