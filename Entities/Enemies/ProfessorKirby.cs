using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.ProfessorKirbyState;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Projectiles;

namespace KirbyNightmareInDreamLand.Entities.Enemies
{
    public class ProfessorKirby : Enemy
    {

        // Jump variables
        private bool isJumping = false;

        // Briefcase ability
        //TO-DO: SWITCH FROM ENEMY BEAM TO BRIEFCASE PROJECTILE
         private EnemyBeam briefcase;
         private bool isBriefcaseActive = false;

        public bool IsJumping => isJumping;

        public ProfessorKirby(Vector2 startPosition) : base(startPosition, EnemyType.ProfessorKirby)
        {
            affectedByGravity = true;
            health = Constants.ProfessorKirby.HEALTH;
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
            if (stateMachine.IsLeft())
            {
                velocity.X = -Constants.ProfessorKirby.MOVE_SPEED;
            }
            else
            {
                velocity.X = Constants.ProfessorKirby.MOVE_SPEED;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (active)
            {
                if (isBriefcaseActive)
                {
                    briefcase.Update();
                    //TO-DO - CHANGE TO BRIEFCASE IS BRIEFCASE ACTIVE
                    if (!briefcase.IsBeamActive())
                    {
                        isBriefcaseActive = false;
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
                velocity.Y = -Constants.ProfessorKirby.JUMP_VELOCITY;
            }

            Move();
        }

        public override void Attack()
        {
            //TO-DO - INPUT PROFESSORKIRBYATTACK SOUND
            //SoundManager.Play("professorkirbyattack");

            //If active, create a new beam using the current position and direction
            if (!isBriefcaseActive)
            {
                //TO-DO- CHANGE TO NEW BRIEFCASE ATTACK
                briefcase = new EnemyBeam(ProjectilePosition(), !stateMachine.IsLeft());
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
            isFalling = false;
            isJumping = false;
            position.Y = intersection.Y;
            if (currentState.GetType().Equals(typeof(ProfessorKirbyJumpingState)))
            {
                ChangeState(new ProfessorKirbyWalkingState(this));
            }
            velocity.Y = 0;
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