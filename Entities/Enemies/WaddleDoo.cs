using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MasterGame.Projectiles;
using MasterGame.StateMachines;

namespace MasterGame.Entities.Enemies
{
    public class WaddleDoo : Enemy
    {
        //Keep track of current frame
        private int frameCounter = 0;

        // Jump variables
        private bool isJumping = false;
        private float originalY;
        private float jumpVelocity = 0;

        // Beam ability
        private EnemyBeam beam;
        private bool isBeamActive = false;

        public WaddleDoo(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDoo)
        {
            //Initialize pose
            stateMachine.ChangePose(EnemyPose.Walking);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                frameCounter++;

                //TO-DO: Change switch case into state pattern design
                switch (stateMachine.GetPose())
                {
                    //Move if walking for specfic number of frames. Transition to charging.
                    case EnemyPose.Walking:
                        Move();
                        if (frameCounter >= Constants.WaddleDoo.WALK_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Charging);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                    //Charge attack. Transitions to attack
                    case EnemyPose.Charging:
                        if (frameCounter >= Constants.WaddleDoo.STOP_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Attacking);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                    //Use beam attack, spawning beam projectile. Transitions back to walking.
                    case EnemyPose.Attacking:
                        Attack();
                        if (frameCounter >= Constants.WaddleDoo.ATTACK_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Hurt);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;
                    case EnemyPose.Hurt:
                        // Transition back to walking after hurtFrames
                        if (frameCounter >= Constants.WaddleDoo.HURT_FRAMES)
                        {
                            stateMachine.ChangePose(EnemyPose.Jumping);
                            frameCounter = 0;
                            UpdateTexture();
                        }
                        break;

                    case EnemyPose.Jumping:
                        Jump();
                        break;
                }

                //update waddle doo
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
            // Adjust beam based on direction facing
            return stateMachine.IsLeft() ? new Vector2(position.X - 17, position.Y - 7) : new Vector2(position.X + 17, position.Y - 7);
        }

        protected override void Move()
        {
            //X moevement left and right. Turns around at left/right boundary
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

        private void Jump()
        {
            if (!isJumping)
            {
                // Start jumping and store initial y
                isJumping = true;
                originalY = position.Y;
                jumpVelocity = -Constants.WaddleDoo.JUMP_HEIGHT; 
            }

            // Apply gravity and update Y position
            position.Y += jumpVelocity;
            jumpVelocity += Constants.WaddleDoo.GRAVITY;

            //Move right or left on x axis in jump
            if (stateMachine.IsLeft())
            {
                position.X -= Constants.WaddleDoo.FORWARD_MOVEMENT; 
            }
            else
            {
                position.X += Constants.WaddleDoo.FORWARD_MOVEMENT; 
            }

            // Check if the character has landed and stop walking
            if (position.Y >= originalY)
            {
                position.Y = originalY; 
                isJumping = false;
                stateMachine.ChangePose(EnemyPose.Walking);
            }
        }

        public override void Attack()
        {
            //If active, create a new beam using the current position and direction
            if (!isBeamActive)
            {
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
                //draw enemy
                enemySprite.Draw(position, spriteBatch);
            }
        }
    }
}