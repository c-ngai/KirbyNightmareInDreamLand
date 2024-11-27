using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Audio;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class FloatingMovement : PlayerMovement
    {
        //protected new float gravity = Constants.Physics.FLOAT_GRAVITY2;
        protected bool endFloat = false;
        public FloatingMovement(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            landed = false;
            gravity = Constants.Physics.FLOAT_GRAVITY;
            terminalVelocity = Constants.Physics.FLOATING_TERMINAL_VELOCITY;
        }
        public override void Walk(bool isLeft)
        {
            velocity.X += isLeft ? Constants.Physics.FLOATING_XACCELLERATION * -1 : Constants.Physics.FLOATING_XACCELLERATION;
            if (velocity.X > Constants.Physics.FLOATING_XVELOCITY)
            {
                velocity.X = Constants.Physics.FLOATING_XVELOCITY;
            }
            else if (velocity.X < -Constants.Physics.FLOATING_XVELOCITY)
            {
                velocity.X = -Constants.Physics.FLOATING_XVELOCITY;
            }
        }

        public override void Run(bool isLeft)
        {
            // Horizontal movement in midair while running is identical to walking
            Walk(isLeft);
        }

        //pressing x (jump) makes float go up
        public override void Jump(bool isLeft)
        {
            endFloat = false;
            landed = false;
            velocity.Y += -0.3f; //go up
            if (velocity.Y < Constants.Physics.FLOAT_MIN_YVEL)
            {
                velocity.Y = Constants.Physics.FLOAT_MIN_YVEL;
            }
            gravity = Constants.Physics.FLOAT_GRAVITY;
            terminalVelocity = Constants.Physics.FLOATING_TERMINAL_VELOCITY;
        }

        public void AdjustPoseWhileFloating(Player kirby)
        {
            //dont go through the floor but float state as not been terminated
            if (kirby.state.GetPose() != KirbyPose.FloatingStart)
            {
                if (landed)
                {
                    kirby.ChangePose(KirbyPose.FloatingGrounded);
                }
                else if (velocity.Y > 0)
                {
                    kirby.ChangePose(KirbyPose.FloatingFalling);
                }
                else
                {
                    kirby.ChangePose(KirbyPose.FloatingRising);
                }
            }
        }
        public void LandIfOnGround(Player kirby)
        {
            //dont go through the floor but floating was ended
            if (landed)
            {
                kirby.ChangeMovement();
            }
        }

        public override void AdjustY(Player kirby)
        {
            base.AdjustY(kirby);

            if (endFloat)
            {
                LandIfOnGround(kirby);
            }
            else
            {
                AdjustPoseWhileFloating(kirby);
            }
        }

        //attack (or pressing z) undoes float
        public override void Attack(Player kirby)
        {
            gravity = Constants.Physics.GRAVITY;
            terminalVelocity = Constants.Physics.TERMINAL_VELOCITY;
            kirby.ChangePose(KirbyPose.FloatingEnd);
            kirby.ChangeMovement();
            endFloat = true;
         
        }

    }
}
