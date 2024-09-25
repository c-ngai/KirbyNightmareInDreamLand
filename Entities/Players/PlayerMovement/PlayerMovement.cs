using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;

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
        protected float walkingVel = .375f;
        protected float runningVel = .75f;
        protected float gravity = 10f;  // Gravity in pixels per second^2
        protected float damageVel = 2f;

        //protected static float slideStarting;
        //protected const float slideDistance = Constants.Graphics.GAME_WIDTH /3;
        protected float frameRate = 0.03125f;

        //decrease public access
        public bool floating;
        public bool crouching;
        public bool jumping;
        public ITimeCalculator timer;
        public bool normal;

        //change kirby velocity to go left
        public PlayerMovement()
        {
            floating = false;
            jumping = false;
            timer = new TimeCalculator();
            crouching = false;
            normal = false;
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
        #endregion

        #region Attacked
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
        public void Slide(Player kirby, bool isLeft)
        {
            //slideStarting = kirby.PositionX;
            if(isLeft){
                xVel = runningVel;
            } else {
                xVel = runningVel * -1;
            }
        }
       
        #endregion

        public virtual void Jump()
        {
            
        }
        #region Move Sprite
        //update kirby position in UI
        public virtual void UpdatePosition(Player kirby, GameTime gameTime)
        {
            kirby.PositionX += xVel;
            kirby.PositionY += yVel;
            if(jumping || floating){
                yVel += gravity *  (float)timer.GetElapsedTimeInS(gameTime);
            }
            if(kirby.PositionY > 0){
                yVel += gravity *  (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
        }

        // checks palyer doesnt go out of frame (up and down)
        public void AdjustX(Player kirby)
        {
            if(kirby.PositionX > Constants.Graphics.GAME_WIDTH)
            {
                kirby.PositionX  = Constants.Graphics.GAME_WIDTH;  
            }
            if(kirby.PositionX < 0)
            {
                kirby.PositionX  = 0;  
            }
        }

        public virtual void AdjustY(Player kirby)
        {
            //dont go through the floor
            if(kirby.PositionY > Constants.Graphics.FLOOR)
            {
                yVel = 0;
                kirby.PositionY = (float) Constants.Graphics.FLOOR;
            }

            //dont go through the ceiling
            if(kirby.PositionY < 10)
            {
                yVel = 0;
                kirby.PositionY = 10;
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
            UpdatePosition(kirby, gameTime);
            Adjust(kirby);
        }
        #endregion

    

    }
}