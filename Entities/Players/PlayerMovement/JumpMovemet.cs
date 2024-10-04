using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class JumpMovement : PlayerMovement
    {
        public const float jumpCeiling = Constants.Physics.JUMP_CEILING;

        protected float jumpVel = Constants.Physics.JUMP_VEL;
        public new float yVel = -2f;
        public JumpMovement(Vector2 pos) : base(pos){}

        public override void Walk(bool isLeft)
        {
            if (isLeft)
            {
                xVel = jumpVel;
            }
            else
            {
                xVel = jumpVel * -1;
            }
        }
        #region Jumping
        public void FinishJump(Player kirby)
        {
            kirby.ChangePose(KirbyPose.Standing);
            yVel = 0;
            xVel = 0;
            kirby.ChangeMovement();

        }
        //checks if kirby is going down to start the falling animation
        public void JumpCheck(Player kirby)
        {
            if (yVel > 0)
            {
                kirby.ChangePose(KirbyPose.JumpFalling);
            }
        }
        public override void Jump(bool isLeft)
        {
            if (position.Y > 80 && yVel < 0)
            { //makes it so kirby can only jump so hight
                yVel = jumpVel;
            }
        }

        #endregion
        public override void Attack(Player kirby)
        {
            //does nothing
        }
        #region Move Sprite
        //update kirby position in UI
        public override void UpdatePosition(GameTime gameTime)
        {
            position.X += xVel;
            position.Y += yVel;
            yVel += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        public override void AdjustY(Player kirby)
        {
            //dont go through the floor
            if (position.Y > Constants.Graphics.FLOOR)
            {
                yVel = 0;
                position.Y = Constants.Graphics.FLOOR;
                FinishJump(kirby); //once he is back on the floor kirby is normal again
            }
            //dont go through the ceiling
            if (position.Y < jumpCeiling)
            {
                yVel = 0;
                position.Y = jumpCeiling;
            }

        }
        //ensures sprite does not leave the window
        public override void Adjust(Player kirby)
        {
            //AdjustX(kirby);
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