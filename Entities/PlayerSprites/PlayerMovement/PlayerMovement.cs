

namespace MasterGame
{
    public abstract class PlayerMovement
    {
        protected PlayerStateMachine state;
        protected int gameWidth;
        protected int gameHeight;
        protected int floor;
        //light hard coded physics
        //seperate movement and state 
        protected float yVel = 0;
        protected float xVel = 0;
        protected float jumpVel = 10f;
        protected float floatVelX = 5f;
        protected float floatVelY = 5f;
        protected float walkingVel = .375f;
        protected float runningVel = .75f;
        protected float gravity = -10f;

        //change kirby velocity to go left
        public PlayerMovement()
        {
            gameWidth = Game1.self.gameWidth;
            gameHeight = Game1.self.gameHeight;
            floor =  gameHeight * 4/5;
            state = PlayerStateMachine.Instance;
    
        }

        public void StopMovement()
        {
            xVel = 0;
        }

        #region Walking
        public void Walk()
        {   
            if(state.IsLeft()){
                xVel = walkingVel * -1;
            } else {
                xVel = walkingVel;
            }
        }
        #endregion

        #region Running
        public void RunLeft()
        {
            
        }
        #endregion

        #region Jumping
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

        }
        #endregion

        #region slide
        #endregion

        #region move sprite
        //update kirby position in UI
        public void UpdatePosition(Player kirby)
        {
            //float newY =(float) (yVel * frameRate + .5 * gravity * frameRate * frameRate + kirby.position.Y);
            kirby.position.X += xVel;
            //kirby.position.Y = newY;
            //yVel += gravity *frameRate;
        }

        // checks palyer doesnt go out of frame (up and down)
        public void AdjustX(Player kirby)
        {
            if(kirby.position.X > gameWidth)
            {
                kirby.position.X  = gameWidth;  
            }
            if(kirby.position.X < 0)
            {
                kirby.position.X  = 0;  
            }
        }

        public void AdjustY(Player kirby)
        {
            //dont go through the floor
            if(kirby.position.Y > floor)
            {
                yVel = 0;
                //kirby.position.Y = (float) floor;
            }

            //dont go through the ceiling
            if(kirby.position.Y < 0)
            {
                yVel = 0;
                //kirby.position.Y = 0 + 10;
            }

        }
        //ensures sprite does not leave the window
        public void Adjust(Player kirby)
        {
            AdjustX(kirby);
            AdjustY(kirby);
        }
        //updates position and adjusts frame. 
        public void MovePlayer(Player kirby)
        {
            UpdatePosition(kirby);
            Adjust(kirby);
        }
        #endregion

    

    }
}