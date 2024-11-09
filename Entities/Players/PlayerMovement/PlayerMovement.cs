using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using KirbyNightmareInDreamLand.Time;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.VisualBasic;
using KirbyNightmareInDreamLand.Levels;
using System.Diagnostics;
using System;
using KirbyNightmareInDreamLand.Particles;

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
        protected float damageVel = Constants.Physics.DAMAGE_VELOCITY;
        protected int ceiling = 20;
        public ITimeCalculator timer;
        protected bool landed = true;

        private int levelBoundsLeft = 10;
        private int levelBoundsRight = -10;

        protected Vector2 position;
        //constructor
        public PlayerMovement(Vector2 pos)
        {
            timer = new TimeCalculator();
            position = pos;
        }
        public Vector2 GetPosition()
        {
            return position;
        }

        public void StopMovement()
        {
            xVel = 0;
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
        public void ReleaseDamageSpin()
        {
            xVel = 0;
            yVel += -1;
        }
        //starts floating pose animation
        public void DeathSpin()
        {
            yVel = -8;
            
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
        public virtual void AdjustY(Player kirby)
        {
            //dont go through the floor
            // if (landed)
            // {
            //     yVel = 0;
            // } 
            yVel +=  gravity * dt; 

            //dont go through the ceiling
            if (position.Y < 15)
            {
                yVel = 0;
                position.Y = 15;
            }
            if(position.Y > Game1.Instance.Level.CurrentRoom.Height)
            {
                Console.WriteLine("here");
                kirby.RestartKirby();
                Game1.Instance.Level.LoadRoom(Game1.Instance.Level.CurrentRoom.Name);
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
        public void Fall()
        {
            ////kirby.ChangePose(Kirby.FreeFall);
            //yVel = gravity;
        }
        public void ChangeKirbyLanded(bool land)
        {
            landed = land;
        }

        #region TileCollision
        public virtual void AdjustFromBottomCollisionBlock(Rectangle intersection)
        {
            yVel = 0;
            position.Y = intersection.Y;
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

        public void AdjustFromBottomCollisionPlatform(Rectangle intersection, IPlayerStateMachine state)
        {
            // Only adjust if kirby was moving downwards during the collision
            if (yVel > 0 || state.GetPose() == KirbyPose.FreeFall || state.GetPose() == KirbyPose.JumpFalling)
            {
                position.Y = intersection.Y;
                yVel = 0;
                ChangeKirbyLanded(true);
            }
        }

        public void AdjustOnSlopeCollision(IPlayerStateMachine state, Tile tile, float slope, float yIntercept)
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
        }
        #endregion
    }
}
