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
            affectedByGravity = true;
        }

        public override void Spawn()
        {
            base.Spawn();
            stateMachine.ChangePose(EnemyPose.Walking);
            ChangeState(new WaddleDooWalkingState(this));
        }

        public override void Move()
        {
            base.Move();
            velocity.X = stateMachine.IsLeft() ? -Constants.WaddleDoo.MOVE_SPEED : Constants.WaddleDoo.MOVE_SPEED;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (active)
            {
                // Handle the beam if active
                if (isBeamActive)
                {
                    if (beam.IsDone())
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
                velocity.Y = -Constants.WaddleDoo.JUMP_VELOCITY;
            }
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
            base.BottomCollisionWithBlock(intersection);

            isJumping = false;
            
            // Note (Mark) THIS IS A BIT JANK
            // Basically: if colliding with a block from above, change to walking state if jumping
            if (currentState.GetType().Equals(typeof(WaddleDooJumpingState)))
            {
                ChangeState(new WaddleDooWalkingState(this));
            }
        }

        public override void BottomCollisionWithPlatform(Rectangle intersection)
        {
            base.BottomCollisionWithPlatform(intersection);

            isJumping = false;

            // Note (Mark) THIS IS A BIT JANK
            // Basically: if colliding with a block from above, change to walking state if jumping
            if (currentState.GetType().Equals(typeof(WaddleDooJumpingState)))
            {
                ChangeState(new WaddleDooWalkingState(this));
            }
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
                    velocity.Y = 0;
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