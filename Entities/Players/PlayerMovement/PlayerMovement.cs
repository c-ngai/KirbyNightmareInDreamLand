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

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public abstract class PlayerMovement
    {
        //light hard coded physics
        //seperate movement and state 
        //make these #define

        protected float yVel = 0;
        protected float xVel = 0;
        protected float walkingVel = Constants.Physics.WALKING_VELOCITY;
        protected float runningVel = Constants.Physics.RUNNING_VELOCITY;
        protected float gravity = Constants.Physics.GRAVITY;
        protected float dt = Constants.Physics.DT;
        protected float groundCollisionOffset = 1 - Constants.Physics.FLOAT_GRAVITY * Constants.Physics.DT;
        protected float damageVel = Constants.Physics.DAMAGE_VELOCITY;
        protected float ceiling = Constants.Kirby.CEILING;
        public ITimeCalculator timer;
        protected bool landed = true;
        public bool onSlope { get; set; }

        private int levelBoundsLeft =  Constants.Kirby.BOUNDS;
        private int levelBoundsRight = Constants.Kirby.BOUNDS * -1;

        protected Vector2 position;
        //constructor
        public PlayerMovement(Vector2 pos)
        {
            timer = new TimeCalculator();
            position = pos;
            onSlope = false;
        }
        public Vector2 GetPosition()
        {
            return position;
        }
        public Vector2 GetVelocity()
        {
            return new Vector2(xVel, yVel);
        }

        public void StopMovement()
        {
            xVel = 0;
        }
        public void DeathMovement()
        {
            xVel = 0;
            yVel = 0;
        }

        public void GoToRoomSpawn()
        {
            position = Game1.Instance.Level.SpawnPoint;
        }

        #region Walking
        public virtual void Walk(bool isLeft)
        {
            xVel = isLeft ? walkingVel * -1 : walkingVel;
        }
        #endregion

        #region Running
        public virtual void Run(bool isLeft)
        {
            xVel = isLeft ? runningVel * -1 : xVel = runningVel;
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
                xVel = damageVel;
            }
            else
            {
                xVel = damageVel * -1;
            }

            yVel = 0;
        }
        //starts floating pose animation
        public void DeathSpin()
        {
            yVel = Constants.Physics.DEATH_VELOCITY;
            
        }
        #endregion

        public virtual void Jump(bool isLeft)
        {
            //does nothing -- overwritten by other classes
        }
        #region Move Sprite
        //update kirby position in UI
        public virtual void UpdatePosition(GameTime gameTime)
        {
            yVel += gravity * dt;
            
            position.X += xVel;
            position.Y += yVel; // + gravity * dt *dt *.5f;
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
        public void FallOffScreenOne(Player kirby)
        {
            if(kirby.DEAD == true) // game over 
            {
                Game1.Instance.Level.GameOver();
                kirby.FillFullHealth();
            } else {
                kirby.RestartKirby();
                Game1.Instance.Level.LoadRoom(Game1.Instance.Level.CurrentRoom.Name);
                Game1.Instance.Level.ChangeToPlaying();
            }
        }
        public void FallOffScreenTwo(Player kirby)
        {
            kirby.FallOffScreenDeath();

        }
        public virtual void AdjustY(Player kirby)
        {
            //dont go through the ceiling
            if (position.Y < ceiling)
            {
                yVel = 0;
                position.Y = ceiling;
            }
            if(position.Y > Game1.Instance.Level.CurrentRoom.Height)
            {
                if(kirby.CollisionActive){
                    FallOffScreenTwo(kirby);
                } else {
                    FallOffScreenOne(kirby);
                }
            }
        }
        //ensures sprite does not leave the window
        public virtual void Adjust(Player kirby)
        {
            AdjustX(kirby);
            AdjustY(kirby);
        }
        //updates position and adjusts frame. 
        public virtual void MovePlayer(Player kirby, GameTime gameTime)
        {
            UpdatePosition(gameTime);
            Adjust(kirby);
        }
        #endregion
        public void ChangeKirbyLanded(bool land)
        {
            landed = land;
        }

        #region TileCollision
        public virtual void AdjustFromBottomCollisionBlock(Rectangle intersection)
        {
            yVel = 0;
            position.Y = (float)intersection.Y + groundCollisionOffset;
            ChangeKirbyLanded(true);
        }

        public virtual void AdjustFromRightCollisionBlock(Rectangle intersection)
        {
            position.X -= intersection.Width;
            xVel = 0;
        }

        public virtual void AdjustFromLeftCollisionBlock(Rectangle intersection)
        {
            position.X += intersection.Width;
            xVel = 0;
        }

        public virtual void AdjustFromTopCollisionBlock(Rectangle intersection)
        {
            position.Y += intersection.Height;
        }

        public void AdjustFromBottomCollisionPlatform(Rectangle intersection, IPlayerStateMachine state)
        {
            // Only adjust if kirby was moving downwards during the collision
            if (yVel > 0)
            {
                yVel = 0;
                position.Y = (float)intersection.Y + groundCollisionOffset;
                ChangeKirbyLanded(true);
            }
        }

        public void AdjustOnSlopeCollision(PlayerStateMachine state, Tile tile, float slope, float yIntercept)
        {
            Rectangle intersection = tile.rectangle;
            if (position.X > intersection.Left && position.X < intersection.Right)
            {
                float offset = position.X - intersection.X;

                float kirbyAdjustment = (intersection.Y + Constants.Level.TILE_SIZE) - (offset * slope) - yIntercept;
                if (position.Y > kirbyAdjustment || state.CanMove() ) // "is kirby moving on the ground in a way where we want him to stay locked on the ground"
                {
                    position.Y = kirbyAdjustment;
                    yVel = Math.Abs(xVel); // If on a slope, set yVel to the absolute value of xVel so that kirby magnetizes down to the slope
                    ChangeKirbyLanded(true);
                }
            }
            if (!landed && !state.IsJumping())
            {
                state.ChangePose(KirbyPose.Standing);
                IParticle star = new CollisionStar(position);
            }
            onSlope = true;
        }
        #endregion
    }
}
