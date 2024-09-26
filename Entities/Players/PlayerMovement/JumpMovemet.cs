using Microsoft.Xna.Framework;
namespace MasterGame
{   
    public class JumpMovement : PlayerMovement
    {
        public const float jumpCeiling = (float)Constants.Graphics.FLOOR * 1.5f/5f;
        public float yVel = -2f;
        public JumpMovement(Vector2 pos) : base(pos) 
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
        public override void Jump(bool isLeft)
        {
            
        }
        public void JumpXY(bool isLeft)
        {
            if(isLeft){
                xVel = jumpVelX * -1;
            } else {
                xVel = jumpVelX;
            }
        }
        #endregion

        #region Move Sprite
        //update kirby position in UI
        public override void UpdatePosition(GameTime gameTime)
        {
            position.X += xVel;
            position.Y += yVel;
            yVel += gravity *  (float)gameTime.ElapsedGameTime.TotalSeconds;
            
        }


        public override void AdjustY(Player kirby)
        {
            //dont go through the floor
            if(position.Y > Constants.Graphics.FLOOR)
            {
                yVel = 0;
                position.Y = (float) Constants.Graphics.FLOOR;
                FinishJump(kirby);
            }
            //dont go through the ceiling
            //check if left and right are overriding the state change
            if(position.Y < jumpCeiling)
            {
                yVel = 0;
                position.Y = jumpCeiling;
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
            UpdatePosition(gameTime);
            Adjust(kirby);
        }
        #endregion
    }
}