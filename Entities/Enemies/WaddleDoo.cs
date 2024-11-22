using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDeeState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.BrontoBurtState;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Audio;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class WaddleDoo : Enemy
    {
        // Jump variables
        private bool isJumping = false;

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
            affectedByGravity = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (active)
            {
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

        public override void Jump()
        {
            if (!isJumping)
            {
                // Start jumping and store initial y
                isJumping = true;
                yVel = -Constants.WaddleDoo.JUMP_VELOCITY;
            }

            Move();
        }

        public override void Attack()
        {
            SoundManager.Play("waddledooattack");
            //If active, create a new beam using the current position and direction
            if (!isBeamActive)
            {
                beam = new EnemyBeam(ProjectilePosition(), !stateMachine.IsLeft());
                isBeamActive = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (active)
            {
                // Draw the beam if it's active
                if (isBeamActive)
                {
                    beam.Draw(spriteBatch);
                }
                //spriteBatch.DrawString(LevelLoader.Instance.Font, isJumping.ToString(), position + new Vector2(-24, -30), Color.Black);
            }
        }

        public override void BottomCollisionWithBlock(Rectangle intersection)
        {
            isFalling = false;
            isJumping = false;
            position.Y = intersection.Y;
            
            // Note (Mark) THIS IS A BIT JANK
            // Basically: if colliding with a block from above, change to walking state if jumping
            if (currentState.GetType().Equals(typeof(WaddleDooJumpingState)))
            {
                ChangeState(new WaddleDooWalkingState(this));
            }
            yVel = 0;
        }

        public override void AdjustOnSlopeCollision(Tile tile, float slope, float yIntercept)
        {

            //GameDebug.Instance.LogPosition(position);

            Rectangle intersection = tile.rectangle;
            if (position.X > intersection.Left && position.X < intersection.Right)
            {
                float offset = position.X - intersection.X;
                //Debug.WriteLine($"Starting Y position: {position.Y}");
                float slopeY = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
                //GameDebug.Instance.LogPosition(intersection.Location.ToVector2());
                if (position.Y > slopeY)
                {
                    position.Y = slopeY;
                    
                    isFalling = false;
                    isJumping = false;
                    // TODO: remove band-aid. waddle doo is always still inside the slope on the first frame of his jump, but he shouldn't be. check the order that velocity and position changes happen. position should change by velocity ONCE at the end of an update
                    if (currentState.GetType().Equals(typeof(WaddleDooJumpingState)) && frameCounter > 0)
                    {
                        ChangeState(new WaddleDooWalkingState(this));
                    }
                    yVel = 0;
                }
                //Debug.WriteLine($"(0,0) point: {intersection.Y + 16}, offset {offset}, slope {slope}, yInterceptAdjustment {yIntercept}");
            }
        }

        public override KirbyType PowerType()
        {
            return KirbyType.Beam;
        }

    }

}