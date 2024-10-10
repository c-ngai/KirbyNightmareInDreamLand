using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class WaddleDee : Enemy
    {
        //Keep track of current frame
        private int frameCounter = 0;

        public WaddleDee(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDee)
        {
            //Set pose and sprite
            stateMachine.ChangePose(EnemyPose.Walking);
            UpdateTexture();
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                frameCounter++;

                //TO-DO: Change switch case into state pattern design
                switch (stateMachine.GetPose())
                {
                    case EnemyPose.Walking:
                        Move();
                        // Transition to Hurt state after hopFrames
                        if (frameCounter >= Constants.WaddleDee.WALK_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Hurt);
                            frameCounter = 0; // Reset frame counter
                            UpdateTexture();  // Update texture for the new pose
                        }
                        break;

                    case EnemyPose.Hurt:
                        // Transition back to walking after hurtFrames
                        if (frameCounter >= Constants.WaddleDee.HURT_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Walking);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                }
                UpdateTexture();
                // Update the enemy sprite
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
                enemySprite.LevelDraw(position, spriteBatch);
            }
        }
    }
}
