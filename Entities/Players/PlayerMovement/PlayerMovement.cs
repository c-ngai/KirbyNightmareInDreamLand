using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using KirbyNightmareInDreamLand.Time;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public abstract class PlayerMovement
    {
        //light hard coded physics
        //seperate movement and state 
        //make these #define

        protected float yVel = Constants.Physics.GRAVITY;
        protected float xVel = 0;
        protected float walkingVel = Constants.Physics.WALKING_VELOCITY;
        protected float runningVel = Constants.Physics.RUNNING_VELOCITY;
        protected float gravity = Constants.Physics.GRAVITY;
        protected float damageVel = Constants.Physics.DAMAGE_VELOCITY;
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
            if (isLeft)
            {
                xVel = walkingVel * -1;
            }
            else
            {
                xVel = walkingVel;
            }
        }
        #endregion

        #region Running
        public virtual void Run(bool isLeft)
        {
            if (isLeft)
            {
                xVel = runningVel * -1;
            }
            else
            {
                xVel = runningVel;
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
            if (yVel > 0)
            {
                yVel *= -1;
            }
            else
            {
                yVel *= -1;
            }
        }
        #endregion

        #region slide
        public virtual void Slide(Player kirby)
        {
            //slideStarting = kirby.PositionX;
            if(kirby.IsSliding())
            {
                xVel = kirby.IsLeft() ? runningVel * -1 :runningVel;
            }
        }

        #endregion

        #region Floating
        //starts floating pose animation
        #endregion

        public virtual void Jump(bool isLeft)
        {
            //does nothing -- overwritten by other classes
        }
        #region Move Sprite
        //update kirby position in UI
        public virtual void UpdatePosition(GameTime gameTime)
        {
            position.X += xVel;
            position.Y += yVel;
            if (position.Y > 0)
            {
                yVel += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
        public virtual void AdjustY(Player kirby)
        {
            //dont go through the floor
            if (landed)
            {
                yVel = 0;
            } else {
                yVel = gravity;
            }

            //dont go through the ceiling
            if (position.Y < 10)
            {
                yVel = 0;
                position.Y = 10;
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
            yVel = gravity;
        }
        public void ChangeKirbyLanded(bool land)
        {
            landed = land;
        }

        #region TileCollision
        public void AdjustFromBottomCollisionBlock(Rectangle intersection)
        {
            position.Y = intersection.Y + 1;
            yVel = 0;
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

        public void AdjustFromBottomCollisionPlatform(Rectangle intersection)
        {
            position.Y = intersection.Y + 1;
            position.Y += 0;
            ChangeKirbyLanded(true);
        }

        // TODO: Figure out slope collisions
        //public void AdjustFromBottomCollisionSlope(Player kirby, Tile tile)
        //{
        //    Vector2 center = CollisionManager.Instance.GetCenter(tile.rectangle);
        //    position.Y = center.X;
        //    yVel = 0;

        //}
        #endregion
    }
}
