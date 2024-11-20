using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Particles;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class JumpMovement : PlayerMovement
    {
        public const float jumpCeiling = Constants.Physics.JUMP_CEILING;

        protected float jumpVel = Constants.Physics.JUMP_VEL;
        protected float jumpHeight = Constants.Physics.JUMP_MAX_HEIGHT;
        protected int jumpFrames = Constants.Physics.JUMP_MAX_FRAMES;
        protected int frameCounter;
        protected int lastFrameJumpCalled;

        private float startingY;

        public new float yVel = Constants.Physics.JUMP_VEL;
        public JumpMovement(Vector2 pos) : base(pos)
        {
            landed = false;
            startingY = position.Y;
            frameCounter = 0;
            lastFrameJumpCalled = Game1.Instance.UpdateCounter;
        }

        public override void Walk(bool isLeft)
        {
            xVel = isLeft ? jumpVel :jumpVel * -1;
        }
        #region Jumping
        public void FinishJump(Player kirby)
        {
            kirby.ChangePose(KirbyPose.Standing);
            yVel = 0;
            kirby.ChangeMovement();
            SoundManager.Play("land");
            IParticle star = new CollisionStar(position);

        }
        //checks if kirby is going down to start the falling animation
        public void JumpCheck(Player kirby)
        {
            if (yVel > 0) // && kirby.GetKirbyPose() == KirbyPose.JumpRising)
            {
                kirby.ChangePose(KirbyPose.JumpFalling);
            }
        }
        public override void Jump(bool isLeft)
        {
            //if (position.Y > startingY - jumpHeight && yVel < 0)
            //{ //makes it so kirby can only jump so high
            //    yVel = jumpVel;
            //}

            int updateCounter = Game1.Instance.UpdateCounter;
            // jump if:
            //   1. haven't run out of frames yet
            //   2. AND if jump was called no later than last frame. (if you let go of jump for even a single frame you stop rising)
            if (frameCounter < jumpFrames && updateCounter <= lastFrameJumpCalled + 1)
            { //makes it so kirby can only jump so high
                yVel = jumpVel;
                frameCounter++;
                lastFrameJumpCalled = updateCounter;
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
            yVel += gravity * dt;
            position.X += xVel;
            position.Y += yVel ;//+ gravity * dt *dt *.5f;
        }

        public override void AdjustY(Player kirby)
        {
            //dont go through the floor
            if (landed)
            {
                FinishJump(kirby);
                //once he is back on the floor kirby is normal again
            }
            //dont go through the ceiling
            if (position.Y < ceiling)
            {
                //yVel = 0;
                position.Y = ceiling;
            }
             if(position.Y > Game1.Instance.Level.CurrentRoom.Height)
            {
                if(kirby.CollisionActive){
                    FallOffScreenTwo(kirby);
                } else {
                    FallOffScreenOne(kirby);
                }
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