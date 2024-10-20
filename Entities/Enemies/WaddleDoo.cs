using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class WaddleDoo : Enemy
    {
        // Jump variables
        private bool isJumping = false;
        private float originalY;
        //private float jumpVelocity = 0;

        // Beam ability
        private EnemyBeam beam;
        private bool isBeamActive = false;

        public bool IsJumping => isJumping;

        public WaddleDoo(Vector2 startPosition) : base(startPosition, EnemyType.WaddleDoo)
        {
            //Initialize pose
            stateMachine.ChangePose(EnemyPose.Walking);
            ChangeState(new WaddleDooWalkingState(this));
            //TO-DO: spawn facing the direction kirby is in
            stateMachine.ChangeDirection();
            yVel = 0;
            xVel = Constants.WaddleDoo.MOVE_SPEED;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isDead)
            {
                IncrementFrameCounter();
                currentState.Update();
                enemySprite.Update();
                GetHitBox();

                if (isFalling)
                {
                    Fall();
                }

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
                position.X -= xVel;
            }
            else
            {
                position.X += xVel;
            }
            UpdateTexture();
        }

        public override void Jump()
        {
            if (!isJumping)
            {
                // Start jumping and store initial y
                isJumping = true;
                yVel = -Constants.WaddleDoo.JUMP_HEIGHT;
            }

            position.Y += yVel;

            Move();
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

        public override void BottomCollisionWithBlock(Rectangle intersection)
        {
            isFalling = false;
            isJumping = false;
            position.Y = intersection.Y;
            yVel = 0;
            ChangeState(new WaddleDooWalkingState(this));
        }
    }
}