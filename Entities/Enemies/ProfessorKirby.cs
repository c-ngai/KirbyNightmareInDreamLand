using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.ProfessorKirbyState;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class ProfessorKirby : Enemy
    {
        // left level boundary
        private int levelBoundsLeft = 0;

        // Jump variables
        private bool isJumping = false;

        // Briefcase ability
         private EnemyBriefcase briefcase;
         private bool isBriefcaseActive = false;

        public bool IsJumping => isJumping;

        public ProfessorKirby(Vector2 startPosition) : base(startPosition, EnemyType.ProfessorKirby)
        {
            affectedByGravity = true;
            health = Constants.ProfessorKirby.HEALTH;
        }

        public virtual void AdjustXPositionToLevelBoundaries()
        {
            // ensures Professor Kirby wont go through the level bounds 
            if (position.X < levelBoundsLeft)
            {
                position.X = levelBoundsLeft;
                ChangeDirection();
            }
            if (position.X > Game1.Instance.Level.CurrentRoom.Width)
            {
                position.X = Game1.Instance.Level.CurrentRoom.Width;
                ChangeDirection();
            }
        }

        public override void Spawn()
        {
            base.Spawn();
            stateMachine.ChangePose(EnemyPose.Walking);
            ChangeState(new ProfessorKirbyWalkingState(this));
        }
        public override void Move()
        {
            base.Move();
            AdjustXPositionToLevelBoundaries();
            velocity.X = stateMachine.IsLeft() ? -Constants.ProfessorKirby.MOVE_SPEED : Constants.ProfessorKirby.MOVE_SPEED;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (active)
            {
                if (isBriefcaseActive)
                {
                    if (!briefcase.IsDone())
                    {
                        isBriefcaseActive = false;
                    }
                }
            } 
        }

        private Vector2 ProjectilePosition()
        {
            // Adjust briefcase based on direction facing
            return stateMachine.IsLeft() ? new Vector2(position.X - 17, position.Y - 7) : new Vector2(position.X + 17, position.Y - 7);
        }

        public override void Jump()
        {
            if (!isJumping)
            {
                // Start jumping and store initial y
                isJumping = true;
                velocity.Y = -Constants.ProfessorKirby.JUMP_VELOCITY;
            }
        }

        public override void Attack()
        {
            //If active, create a new beam using the current position and direction
            if (!isBriefcaseActive)
            {
                briefcase = new EnemyBriefcase(ProjectilePosition(), stateMachine.IsLeft());
                isBriefcaseActive = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (active)
            {
                // Draw the briefcase if it's active
                if (isBriefcaseActive)
                {
                    briefcase.Draw(spriteBatch);
                }
            }
        }

        public override void BottomCollisionWithBlock(Rectangle intersection)
        {
            base.BottomCollisionWithBlock(intersection);

            isJumping = false;
            if (currentState.GetType().Equals(typeof(ProfessorKirbyJumpingState)))
            {
                ChangeState(new ProfessorKirbyWalkingState(this));
            }
        }

        public override void AdjustOnSlopeCollision(Tile tile, float slope, float yIntercept)
        {
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
                    if (currentState.GetType().Equals(typeof(ProfessorKirbyJumpingState)) && frameCounter > 0)
                    {
                        ChangeState(new ProfessorKirbyWalkingState(this));
                    }
                    velocity.Y = 0;
                }
            }
        }

        public override KirbyType PowerType()
        {
            return KirbyType.Professor;
        }

    }
}