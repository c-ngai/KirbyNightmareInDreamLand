using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Audio;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class FloatingMovement : PlayerMovement
    {
        protected float floatVel = Constants.Physics.FLOAT_VEL;
        protected float floatGravity = Constants.Physics.FLOAT_GRAVITY;
        //protected new float gravity = Constants.Physics.FLOAT_GRAVITY2;
        protected bool endFloat = false;
        public FloatingMovement(Vector2 pos, Vector2 vel) : base(pos, vel)
        {
            landed = false;
        }
        public override void Walk(bool isLeft)
        {
            if (isLeft)
            {
                velocity.X = floatVel * -1; // times -1 to go in opposite direction
            }
            else
            {
                velocity.X = floatVel;
            }
        }

        public override void Run(bool isLeft)
        {
            Walk(isLeft);
        }

        //pressing x (jump) makes float go up
        public override void Jump(bool isLeft)
        {
            endFloat = false;
            landed = false;
            velocity.Y = floatVel * -1; //go up
            
        }

        public void AdjustYPositionWhileFloating(Player kirby)
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
        public void AdjustYPositionWhileNotFloating(Player kirby)
        {
            //dont go through the floor but floating was ended
            if (landed)
            {
                kirby.ChangeMovement();
            }
        }

        public override void AdjustY(Player kirby)
        {
            if (endFloat)
            {
                AdjustYPositionWhileNotFloating(kirby);
            }
            else
            {
                AdjustYPositionWhileFloating(kirby);
            }
            //dont go through the ceiling
            if (position.Y < Constants.Kirby.CEILING)
            {
                velocity.Y = 0;
                position.Y = Constants.Kirby.CEILING;
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

        //attack (or pressing z) undoes float
        public override void Attack(Player kirby)
        {
            //SoundManager.Play("spitair");
            floatGravity = gravity;
            velocity.Y = floatVel;
            kirby.ChangePose(KirbyPose.FloatingEnd);
            kirby.ChangeMovement();
            endFloat = true;
         
        }

    }
}
