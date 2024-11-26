using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using KirbyNightmareInDreamLand.Time;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.VisualBasic;
using KirbyNightmareInDreamLand.Levels;
using System.Diagnostics;
using System;
using KirbyNightmareInDreamLand.Particles;
using static KirbyNightmareInDreamLand.Constants;
using System.Xml.Linq;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public abstract class PlayerMovement
    {
        //light hard coded physics
        //seperate movement and state 
        //make these #define

        protected Vector2 position;
        protected Vector2 velocity;

        protected float walkingVel = Constants.Physics.WALKING_VELOCITY;
        protected float runningVel = Constants.Physics.RUNNING_VELOCITY;
        protected float gravity = Constants.Physics.GRAVITY;
        protected float terminalVelocity = Constants.Physics.TERMINAL_VELOCITY;
        protected float groundCollisionOffset = 1 - Constants.Physics.FLOAT_GRAVITY;
        protected float damageVel = Constants.Physics.DAMAGE_VELOCITY;
        protected float ceiling = Constants.Kirby.CEILING;
        private ITimeCalculator timer;
        protected bool landed = true;
        public bool onSlope { get; private set; }

        private int levelBoundsLeft =  Constants.Kirby.BOUNDS;
        private int levelBoundsRight = Constants.Kirby.BOUNDS * -1;

        //constructor
        public PlayerMovement(Vector2 pos, Vector2 vel)
        {
            position = pos;
            velocity = vel;
            onSlope = false;
        }
        public Vector2 GetPosition()
        {
            return position;
        }
        public Vector2 GetVelocity()
        {
            return velocity;
        }

        public void StopMovement()
        {
            velocity.X = 0;
        }
        public void CancelVelocity()
        {
            velocity.X = 0;
            velocity.Y = 0;
        }

        public void GoToRoomSpawn()
        {
            position = Game1.Instance.Level.SpawnPoint;
        }

        public void SetOnSlope(bool isOnSlope)
        {
            onSlope = isOnSlope;
        }

        #region Walking
        public virtual void Walk(bool isLeft)
        {
            velocity.X += isLeft ? Constants.Physics.WALKING_ACCELLERATION * -1 : Constants.Physics.WALKING_ACCELLERATION;
            if (velocity.X > Constants.Physics.WALKING_VELOCITY)
            {
                velocity.X = Constants.Physics.WALKING_VELOCITY;
            }
            else if (velocity.X < -Constants.Physics.WALKING_VELOCITY)
            {
                velocity.X = -Constants.Physics.WALKING_VELOCITY;
            }
        }
        #endregion

        #region Running
        public virtual void Run(bool isLeft)
        {
            velocity.X += isLeft ? Constants.Physics.RUNNING_ACCELLERATION * -1 : Constants.Physics.RUNNING_ACCELLERATION;
            if (velocity.X > Constants.Physics.RUNNING_VELOCITY)
            {
                velocity.X = Constants.Physics.RUNNING_VELOCITY;
            }
            else if (velocity.X < -Constants.Physics.RUNNING_VELOCITY)
            {
                velocity.X = -Constants.Physics.RUNNING_VELOCITY;
            }
        }
        #endregion


        #region Attack
        public virtual void Attack(Player kirby)
        {
            //overwritten by other methods
        }
        public virtual void AttackPressed(Player kirby)
        {
            //overwritten by other methods
        }
        public virtual void EndSlide()
        {
            //overwritten
        }
        #endregion

        #region DeathSpin
        public void ReceiveDamage(Rectangle intersection)
        {
            if (intersection.X <= position.X) 
            {
                velocity.X = damageVel;
            }
            else
            {
                velocity.X = damageVel * -1;
            }

            velocity.Y = 0;
        }
        //starts floating pose animation
        public void DeathSpin()
        {
            velocity.Y = Constants.Physics.DEATH_VELOCITY;
            
        }
        #endregion

        public virtual void Jump(bool isLeft)
        {
            //does nothing -- overwritten by other classes
        }

        public void bounceJump()
        {
            landed = false;
            velocity.Y = Constants.Physics.JUMP_VEL;
        }
        #region Move Sprite
        //update kirby position in UI
        public virtual void UpdatePosition(Player kirby)
        {
            // Apply gravity unless in DeathStun pose
            if (kirby.state.GetPose() != KirbyPose.DeathStun)
            {
                velocity.Y += gravity;
            }

            if (velocity.Y > terminalVelocity && !kirby.DEAD)
            {
                velocity.Y = terminalVelocity;
            }

            Decelerate(Constants.Physics.X_DECELERATION);

            position.X += velocity.X;
            position.Y += velocity.Y; // + gravity * dt *dt *.5f;
        }

        public void Decelerate(float deceleration)
        {
            if (velocity.X > 0)
            {
                velocity.X -= deceleration;
            }
            else if (velocity.X < 0)
            {
                velocity.X += deceleration;
            }
            if (velocity.X < deceleration && velocity.X > -deceleration)
            {
                velocity.X = 0;
            }
        }

        public virtual void AdjustX(Player kirby)
        {
            //checks kirby wont go through the level bounds 
            //the +/- 10 is on sprite size
            if(position.X < levelBoundsLeft)
            {
                position.X = levelBoundsLeft;
            }
            if(position.X > Game1.Instance.Level.CurrentRoom.Width + levelBoundsRight)
            {
                position.X = Game1.Instance.Level.CurrentRoom.Width + levelBoundsRight;
            }
        }

        public static int callCount = 0;
        public void FallOffScreenCollisionInactive(Player kirby)
        {
            callCount++;
            if (kirby.lives == 0) // game over 
            {
                Game1.Instance.Level.GameOver();
                kirby.FillLives();
            }
            else {
                //Game1.Instance.Level.LoadRoom(Game1.Instance.Level.CurrentRoom.Name);
                //Game1.Instance.Level.ChangeToPlaying();

                Game1.Instance.Level.NextRoom = Game1.Instance.Level.CurrentRoom.Name;
                Game1.Instance.Level.NextSpawn = Game1.Instance.Level.CurrentRoom.SpawnPoint;
                Game1.Instance.Level.ChangeToTransitionState();
                kirby.RestartKirby();
                //Game1.Instance.Level.LoadRoom(Game1.Instance.Level.CurrentRoom.Name);
                //Game1.Instance.Level.ChangeToPlaying();

                Game1.Instance.Level.NextRoom = Game1.Instance.Level.CurrentRoom.Name;
                Game1.Instance.Level.NextSpawn = Game1.Instance.Level.SpawnPoint;
                Game1.Instance.Level.ChangeToTransitionState();
            }
        }
   
        public virtual void AdjustY(Player kirby)
        {
            //dont go through the ceiling
            if (position.Y < ceiling && !kirby.DEAD)
            {
                //velocity.Y = 0;
                position.Y = ceiling;
            }
        }
        //ensures sprite does not leave the window
        public virtual void Adjust(Player kirby)
        {
            AdjustX(kirby);
            AdjustY(kirby);
        }

        // Checks if Kirby has fallen below the death barrier (1 block below bottom of stage)
        public void DeathBarrierCheck(Player kirby)
        {
            if (position.Y > Game1.Instance.Level.CurrentRoom.DeathBarrier)
            {
                // If Kirby was (previously) alive, kill him
                if (!kirby.DEAD)
                {
                    kirby.Die();
                }
            }
        }

        //updates position and adjusts frame. 
        public virtual void MovePlayer(Player kirby, GameTime gameTime)
        {
            UpdatePosition(kirby);
            Adjust(kirby);
            DeathBarrierCheck(kirby);
        }
        #endregion
        public void ChangeKirbyLanded(bool land)
        {
            landed = land;
        }

        #region TileCollision
        public virtual void AdjustFromBottomCollisionBlock(Rectangle intersection)
        {
            velocity.Y = 0;
            position.Y = (float)intersection.Y + groundCollisionOffset;
        }

        public virtual void AdjustFromRightCollisionBlock(Rectangle intersection)
        {
            position.X -= intersection.Width;
            velocity.X = 0;
        }

        public virtual void AdjustFromLeftCollisionBlock(Rectangle intersection)
        {
            position.X += intersection.Width;
            velocity.X = 0;
        }

        public virtual void AdjustFromTopCollisionBlock(Rectangle intersection)
        {
            position.Y += intersection.Height;
        }

        public void AdjustFromBottomCollisionPlatform(Rectangle intersection, IPlayerStateMachine state)
        {
            // Only adjust if kirby was moving downwards during the collision
            if (velocity.Y > 0)
            {
                velocity.Y = 0;
                position.Y = (float)intersection.Y + groundCollisionOffset;
            }
        }

        public void AdjustOnSlopeCollision(PlayerStateMachine state, Tile tile, float slope, float yIntercept, Player kirby)
        {
            Rectangle intersection = tile.rectangle;
            if (position.X > intersection.Left && position.X < intersection.Right)
            {
                float offset = position.X - intersection.X;

                float kirbyAdjustment = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
                if (position.Y > kirbyAdjustment || state.CanMove() ) // "is kirby moving on the ground in a way where we want him to stay locked on the ground"
                {
                    position.Y = kirbyAdjustment;
                    // needs this adjustment to ensure proper collision when non floating, if this is added when floating Kirby cannot float directly up when he's landed on the slope
                    if (!kirby.state.IsFloating())
                    {
                        position.Y += groundCollisionOffset;
                    }
                    velocity.Y = Math.Abs(velocity.X); // If on a slope, set velocity.Y to the absolute value of velocity.X so that kirby magnetizes down to the slope
                    ChangeKirbyLanded(true);
                    kirby.HandleFreeFall();
                }
            }
            onSlope = true;
        }
        #endregion
    }
}
