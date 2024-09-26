using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MasterGame
{
    public abstract class PlayerMovement
    {
        //light hard coded physics
        //seperate movement and state 
        //make these #define
        
        protected float yVel = 0;
        protected float xVel = 0;
        protected float jumpVel = -2f;
        protected float jumpVelX = 2f;
        protected float floatVelX = 5f;
        protected float floatVelY = 5f;
        protected float walkingVel = .4f;
        protected float runningVel = .8f;
        protected float gravity = 10f;  // Gravity in pixels per second^2
        protected float damageVel = 2f;

        //protected static float slideStarting;
        //protected const float slideDistance = Constants.Graphics.GAME_WIDTH /3;
        protected float frameRate = 0.03125f;

        //decrease public access
        public bool floating = false;
        public bool crouching = false;
        public bool jumping = false;
        public bool normal = false;
        public ITimeCalculator timer;

        protected Vector2 position;

        //change kirby velocity to go left
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

        #region Walking
        public virtual void Walk(bool isLeft)
        {   
            if(isLeft){
                xVel = walkingVel * -1;
            } else {
                xVel = walkingVel;
            }
        }
        #endregion

        #region Running
        public virtual void Run(bool isLeft)
        {
            if(isLeft){
                xVel = runningVel * -1;
            } else {
                xVel = runningVel;
            }
        }
        #endregion


        #region Attack
        public virtual void Attack(Player kirby)
        {
            kirby.ChangePose(KirbyPose.Attacking);
        }
        public void ReceiveDamage(bool isLeft)
        {
            if(isLeft){
                xVel = damageVel;
            } else {
                xVel = damageVel * -1;
            }
            if(yVel > 0)
            {
                yVel *= -1;
            } else{
                yVel *= -1;
            }
        }
        #endregion

        #region slide
        public void Slide(bool isLeft)
        {
            //slideStarting = kirby.PositionX;
            if(isLeft){
                xVel = runningVel *-1;
            } else {
                xVel = runningVel;
            }
        }
       
        #endregion

        #region Floating
        public async void StartFloating(Player kirby)
        {
            kirby.ChangePose(KirbyPose.FloatingStart);
        }
        public virtual void EndFloat()
        {

        }
        #endregion

        public virtual void EndJump()
        {

        }
        public virtual void Jump(bool isLeft)
        {
            //does nothing
        }
        #region Move Sprite
        //update kirby position in UI
        public virtual void UpdatePosition(GameTime gameTime)
        {
            position.X += xVel;
            position.Y += yVel;
            if(jumping || floating){
                yVel += gravity *  (float)timer.GetElapsedTimeInS(gameTime);
            }
            if(position.Y > 0){
                yVel += gravity *  (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
        }

        // checks palyer doesnt go out of frame (up and down)
        public void AdjustX(Player kirby)
        {
            if(position.X > Constants.Graphics.GAME_WIDTH)
            {
                position.X  = Constants.Graphics.GAME_WIDTH;  
            }
            if(position.X < 0)
            {
                position.X  = 0;  
            }
        }

        public virtual void AdjustY(Player kirby)
        {
            //dont go through the floor
            if(position.Y > Constants.Graphics.FLOOR)
            {
                yVel = 0;
                position.Y = (float) Constants.Graphics.FLOOR;
            }

            //dont go through the ceiling
            if(position.Y < 10)
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

    

    }
}