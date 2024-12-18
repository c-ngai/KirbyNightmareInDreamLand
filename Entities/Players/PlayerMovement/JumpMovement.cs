using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Particles;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class JumpMovement : PlayerMovement
    {
        public const float jumpCeiling = Constants.Physics.JUMP_CEILING;

        protected float jumpYVel = Constants.Physics.JUMP_VEL;
        protected float jumpXVel = Constants.Physics.JUMPING_XVELOCITY;
        protected int jumpFrames = Constants.Physics.JUMP_MAX_FRAMES;
        protected int frameCounter;
        protected int lastFrameJumpCalled;

        public JumpMovement(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            landed = false;
            frameCounter = 0;
            lastFrameJumpCalled = Game1.Instance.UpdateCounter;
            velocity.Y = Constants.Physics.JUMP_VEL;
        }

        public override void Walk(bool isLeft)
        {
            velocity.X += isLeft ? Constants.Physics.JUMPING_XACCELLERATION * -1 : Constants.Physics.JUMPING_XACCELLERATION;
            if (velocity.X > Constants.Physics.JUMPING_XVELOCITY)
            {
                velocity.X = Constants.Physics.JUMPING_XVELOCITY;
            }
            else if (velocity.X < -Constants.Physics.JUMPING_XVELOCITY)
            {
                velocity.X = -Constants.Physics.JUMPING_XVELOCITY;
            }
        }

        public override void Run(bool isLeft)
        {
            // Horizontal movement in midair while running is identical to walking
            Walk(isLeft);
        }

        #region Jumping
        public void FinishJump(Player kirby)
        {
            kirby.ChangeToNormalMovement();
            SoundManager.Play("land");
        }
        //checks if kirby is going down to start the falling animation
        public void JumpCheck(Player kirby)
        {
            if (velocity.Y > 0 && kirby.GetKirbyPose() == KirbyPose.JumpRising)
            {
                kirby.ChangePose(KirbyPose.JumpFalling);
            }
        }
        public override void Jump(bool isLeft)
        {

            int updateCounter = Game1.Instance.UpdateCounter;
            // jump if:
            //   1. haven't run out of frames yet
            //   2. AND if jump was called no later than last frame. (if you let go of jump for even a single frame you stop rising)
            if (frameCounter < jumpFrames && updateCounter <= lastFrameJumpCalled + 1 && velocity.Y < 0)
            { //makes it so kirby can only jump so high
                velocity.Y = jumpYVel;
                frameCounter++;
                
            }
            lastFrameJumpCalled = updateCounter;
        }

        #endregion
        public override void Attack(Player kirby)
        {
            //does nothing
        }
        #region Move Sprite

        public override void AdjustY(Player kirby)
        {
            base.AdjustY(kirby);
            
            //dont go through the floor
            if (landed)
            {
                FinishJump(kirby);
                //once he is back on the floor kirby is normal again
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
            base.MovePlayer(kirby, gameTime);

            // If Kirby has just let go of jump and is still rising, cut his vertical velocity off a bit. This is a subtle feature in the actual game that makes jumping feel snappier and easier to control
            if (Game1.Instance.UpdateCounter == lastFrameJumpCalled + 1 && velocity.Y < Constants.Physics.JUMP_RELEASE_VEL)
            {
                velocity.Y = Constants.Physics.JUMP_RELEASE_VEL;
            }
        }
        #endregion

    }
}