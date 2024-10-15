using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class WaddleDee : Enemy
    {
        //Keep track of current frame
        //private int frameCounter = 0;
        private ICollidable collidable;
        public WaddleDee(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDee)
        {
            //Set pose and sprite
            stateMachine.ChangePose(EnemyPose.Walking);
            UpdateTexture();
            collidable = new WaddleDeeCollisionHandler((int)startPosition.X, (int)startPosition.Y, this);
        }

        public override void Move()
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

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                currentState.Update(this); // Delegate update to current state
                UpdateTexture(); // Update the texture if the state has changed
                enemySprite.Update(); // Update the enemy sprite
            }
        }
    }
}
