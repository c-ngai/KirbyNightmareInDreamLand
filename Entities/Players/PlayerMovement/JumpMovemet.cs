using Microsoft.Xna.Framework;
namespace MasterGame
{   
    public class JumpMovement : PlayerMovement
    {
        public const float jumpCeiling = (float)Constants.Graphics.FLOOR * 1.5f/5f;
        public JumpMovement() 
        {
            jumping = true;
        }

        public override void Walk(bool isLeft)
        {   
            if(isLeft){
                xVel = jumpVel;
            } else {
                xVel = jumpVel *-1;
            }
            JumpXY(isLeft);
        }

        #region Jumping
        public void FinishJump(Player kirby)
        {
            if(jumping)
            {
                kirby.ChangePose(KirbyPose.Standing);
                xVel = 0;
                jumping = false;
                kirby.ChangeMovement();
                
            }
        }
        public void JumpCheck(Player kirby)
        {
            if(yVel > 0)
            {
                kirby.ChangePose(KirbyPose.JumpFalling);
            }
        }
        public override void Jump()
        {
            yVel = jumpVel;
        }
        public void JumpXY(bool isLeft)
        {
            Jump();
            if(isLeft){
                xVel = jumpVelX * -1;
            } else {
                xVel = jumpVelX;
            }
        }
        #endregion

        #region Move Sprite
        //update kirby position in UI
        public override void UpdatePosition(Player kirby, GameTime gameTime)
        {
            kirby.PositionX += xVel;
            kirby.PositionY += yVel;
            yVel += gravity *  (float)gameTime.ElapsedGameTime.TotalSeconds;
            
        }


        public override void AdjustY(Player kirby)
        {
            //dont go through the floor
            if(kirby.PositionY > Constants.Graphics.FLOOR)
            {
                yVel = 0;
                kirby.PositionY = (float) Constants.Graphics.FLOOR;
                FinishJump(kirby);
            }

            //dont go through the ceiling
            //check if left and right are overriding the state change
            if(kirby.PositionY < jumpCeiling)
            {
                yVel = 0;
                kirby.PositionY = jumpCeiling;
            }
            if(!jumping){
                yVel = 0;
            }

        }
        //ensures sprite does not leave the window
        public override void Adjust(Player kirby)
        {
            AdjustX(kirby);
            AdjustY(kirby);
            JumpCheck(kirby);
        }
        //updates position and adjusts frame. 
        public override void MovePlayer(Player kirby, GameTime gameTime)
        {
            UpdatePosition(kirby, gameTime);
            Adjust(kirby);
        }
        #endregion
    }
}