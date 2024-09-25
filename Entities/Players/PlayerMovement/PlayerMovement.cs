using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;

namespace MasterGame
{
    public abstract class PlayerMovement
    {
        protected PlayerStateMachine state;
        protected int floor;
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

        protected float frameRate = 0.03125f;

        //decrease public access
        public bool floating;
        public bool crouching;
        public bool jumping;
        public ITimeCalculator timer;

        //change kirby velocity to go left
        public PlayerMovement(ref PlayerStateMachine pState)
        {
            state = pState;
            floating = false;
            jumping = false;
            timer = new TimeCalculator();
        }

        public void StopMovement()
        {
            xVel = 0;
        }

        #region Walking
        public virtual void Walk()
        {   
            if(state.IsLeft()){
                xVel = walkingVel * -1;
            } else {
                xVel = walkingVel;
            }
            if(jumping){
                JumpXY();
            }
        }
        #endregion

        #region Running
        public virtual void Run()
        {
            if(state.IsLeft()){
                xVel = runningVel * -1;
            } else {
                xVel = runningVel;
            }
        }
        #endregion

        #region Jumping
        public void FinishJump(Player kirby)
        {
            if(jumping)
            {
                kirby.ChangePose(KirbyPose.Standing);
                jumping = false;
            }
        }
        public void JumpCheck(Player kirby)
        {
            if(jumping && yVel > 0)
            {
                kirby.ChangePose(KirbyPose.JumpFalling);
            }
        }
        public void JumpY()
        {
            jumping = true;
            yVel = jumpVel;
        }
        public void JumpXY()
        {
            JumpY();
            if(state.IsLeft()){
                xVel = jumpVelX * -1;
            } else {
                xVel = jumpVelX;
            }
        }
        #endregion

        #region Attack
        public virtual void Attack()
        {
            state.ChangePose(KirbyPose.Attacking);
        }
        #endregion

        #region Attacked
        public void ReceiveDamage()
        {
            if(state.IsLeft()){
                xVel = damageVel;
            } else {
                xVel = damageVel * -1;
            }
            if(yVel > 0 !)
            {
                yVel *= -1;
            } else{
                yVel *= -1;
            }
        }
        #endregion

        #region slide
        public void Slide()
        {
            state.ChangePose(KirbyPose.Sliding);
        }
        #endregion

        #region Move Sprite
        //update kirby position in UI
        public void UpdatePosition(Player kirby, GameTime gameTime)
        {

            kirby.PositionX += xVel;
            kirby.PositionY += yVel;
            if(jumping || floating){
                yVel += gravity *  (float)timer.GetElapsedTimeInS(gameTime);
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

        public void AdjustY(Player kirby)
        {
            //dont go through the floor
            if(kirby.PositionY > Constants.Graphics.FLOOR)
            {
                yVel = 0;
                kirby.PositionY = (float) Constants.Graphics.FLOOR;
                FinishJump(kirby);
            }

            //dont go through the ceiling
            if(kirby.PositionY < 10)
            {
                yVel = 0;
                kirby.PositionY = 10;
            }
            if(!jumping){
                yVel = 0;
            }

        }
        //ensures sprite does not leave the window
        public void Adjust(Player kirby)
        {
            AdjustX(kirby);
            AdjustY(kirby);
            JumpCheck(kirby);
        }
        //updates position and adjusts frame. 
        public void MovePlayer(Player kirby, GameTime gameTime)
        {
            UpdatePosition(kirby, gameTime);
            Adjust(kirby);
        }
        #endregion

    

    }
}