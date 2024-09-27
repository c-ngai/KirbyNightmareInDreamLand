using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MasterGame
{
    public class WaddleDoo : Enemy
    {
        private const float MoveSpeed = 0.5f;

        // Walking and attacking frames
        private int frameCounter = 0;

        // Define a list of tuples (EnemyPose, number of frames)
        private List<(EnemyPose pose, int frames)> poseFrames;
        private int currentPoseIndex = 0;
        private int walkingFrames = 180;
        private int chargingFrames = 60;
        private int attackingFrames = 33;

        // Beam
        private EnemyBeam beam; // Change from List<EnemyBeam> to a single instance
        private bool isBeamActive = false;

        public WaddleDoo(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDoo)
        {
            enemySprite = SpriteFactory.Instance.createSprite("waddledoo_walking_right");

            // Initialize the list of pose and frame pairs
            poseFrames = new List<(EnemyPose, int)>
            {
                (EnemyPose.Walking, walkingFrames),
                (EnemyPose.Charging, chargingFrames), 
                (EnemyPose.Attacking, attackingFrames) 
            };

            // Start with the first pose
            stateMachine.ChangePose(poseFrames[currentPoseIndex].pose);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                frameCounter++;

                // Get the current pose and frames
                var (currentPose, requiredFrames) = poseFrames[currentPoseIndex];

                if (frameCounter >= requiredFrames)
                {
                    // Move to the next pose in the cycle
                    currentPoseIndex = (currentPoseIndex + 1) % poseFrames.Count;
                    frameCounter = 0;

                    // Update the state machine and texture for the new pose
                    stateMachine.ChangePose(poseFrames[currentPoseIndex].pose);
                    UpdateTexture();
                }

                // Perform specific actions for the current pose
                if (currentPose == EnemyPose.Walking)
                {
                    Move();
                }
                else if (currentPose == EnemyPose.Attacking)
                {
                    Attack();
                }

                enemySprite.Update();

                // Update the beam if it's active
                if (isBeamActive)
                {
                    beam.Update();
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