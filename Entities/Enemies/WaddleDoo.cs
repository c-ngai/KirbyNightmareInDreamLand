using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MasterGame
{
    public class WaddleDoo : Enemy
    {
        // Walking and attacking frames
        private int frameCounter = 0;

        // Beam
        private EnemyBeam beam; // Change from List<EnemyBeam> to a single instance
        private bool isBeamActive = false;

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

                //TO-DO: Change switch case into state pattern design
                switch (stateMachine.GetPose())
                {
                    case EnemyPose.Walking:
                        Move();
                        if (frameCounter >= Constants.WaddleDoo.WALK_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Charging);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;

                    case EnemyPose.Charging:
                        if (frameCounter >= Constants.WaddleDoo.STOP_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Attacking);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;

                    case EnemyPose.Attacking:
                        Attack();
                        if (frameCounter >= Constants.WaddleDoo.ATTACK_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Walking);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                }

                enemySprite.Update();

                // Update the beam if it's active
                if (isBeamActive)
                {
                    beam.Update();

                    // If the beam is no longer active, reset the state
                    if (!beam.IsBeamActive())
                    {
                        isBeamActive = false;
                    }
                }
            }
        }

        private Vector2 ProjectilePosition()
        {
            // Adjust flamethrower position based on Hothead's facing direction
            return stateMachine.IsLeft() ? new Vector2(position.X - 17, position.Y - 7) : new Vector2(position.X + 17, position.Y - 7); // TODO: These values probably need to be changed to be accurate. Check how far the position for hothead is from the edges of the sprite.
        }

        protected override void Move()
        {
            if (stateMachine.IsLeft())
            {
                position.X -= Constants.WaddleDoo.MOVE_SPEED;
                if (position.X <= leftBoundary.X)
                {
                    ChangeDirection();
                }
            }
            else
            {
                position.X += Constants.WaddleDoo.MOVE_SPEED;
                if (position.X >= rightBoundary.X)
                {
                    ChangeDirection();
                }
            }
            UpdateTexture();
        }

        public override void Attack()
        {
            if (!isBeamActive)
            {
                // Create a new beam using the current position and direction
                beam = new EnemyBeam(ProjectilePosition(), !stateMachine.IsLeft());
                isBeamActive = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isDead)
            {
                // Draw the beam if it's active
                if (isBeamActive)
                {
                    beam.Draw(spriteBatch);
                }
                enemySprite.Draw(position, spriteBatch);
            }
        }
    }
}