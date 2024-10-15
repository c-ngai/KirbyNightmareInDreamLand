using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class WaddleDoo : Enemy, IJumpable
    {
        // Jump variables
        private bool isJumping = false;
        private float originalY;
        private float jumpVelocity = 0;

        // Beam ability
        private EnemyBeam beam;
        private bool isBeamActive = false;
        private ICollidable collidable;

        public bool IsJumping => isJumping;

        public WaddleDoo(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDoo)
        {
            //Initialize pose
            stateMachine.ChangePose(EnemyPose.Walking);
            currentState = new WaddleDooWalkingState();
        }

        
        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                IncrementFrameCounter(); 
                currentState.Update(this);

                // Update the sprite
                enemySprite.Update();

                // Handle the beam if active
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
            // Adjust beam based on direction facing
            return stateMachine.IsLeft() ? new Vector2(position.X - 17, position.Y - 7) : new Vector2(position.X + 17, position.Y - 7);
        }

        public override void Move()
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

        public void Jump()
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