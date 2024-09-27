using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class WaddleDoo : Enemy
    {
        private const float MoveSpeed = 0.5f;

        //Walking and attacking frames
        private int frameCounter = 0;
        private int walkFrames = 180;
        private int stopFrames = 60;
        private int attackFrames = 100;

        public WaddleDoo(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDoo)
        {
            stateMachine.ChangePose(EnemyPose.Walking);
            enemySprite = SpriteFactory.Instance.createSprite("waddledoo_walking_right");
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                frameCounter++;

                switch (stateMachine.GetPose())
                {
                    case EnemyPose.Walking:
                        Move();
                        if (frameCounter >= walkFrames)
                        {
                            stateMachine.ChangePose(EnemyPose.Charging);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;

                    case EnemyPose.Charging:
                        if (frameCounter >= stopFrames)
                        {
                            stateMachine.ChangePose(EnemyPose.Attacking);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;

                    case EnemyPose.Attacking:
                        if (frameCounter >= attackFrames)
                        {
                            stateMachine.ChangePose(EnemyPose.Walking);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                }

                enemySprite.Update();
            }
        }

        protected override void Move()
        {
            if (stateMachine.IsLeft())
            {
                position.X -= MoveSpeed;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X += MoveSpeed;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                }
            }
            UpdateTexture();
        }

        public override void Attack()
        {
            //Beam attack
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
