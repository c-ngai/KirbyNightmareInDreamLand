using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class FloatingMovement : PlayerMovement
    {
        protected float floatVel = Constants.Physics.FLOAT_VEL;
        protected float floatGravity = Constants.Physics.FLOAT_GRAVITY;
        protected new float gravity = Constants.Physics.FLOAT_GRAVITY2;
        private float floatFallingWindow = Constants.Graphics.FLOOR + 50;
        protected bool endFloat = false;
        public FloatingMovement(Vector2 pos) : base(pos){}
        public override void Walk(bool isLeft)
        {
            if (isLeft)
            {
                xVel = floatVel * -1; // times -1 to go in opposite direction
            }
            else
            {
                xVel = floatVel;
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
            yVel = floatVel * -1; //go up
        }

        public void AdjustYPositionWhileFloating(Player kirby)
        {
            //dont go through the floor but float state as not been terminated
            if (position.Y > Constants.Graphics.FLOOR)
            {
                yVel = 0;
                xVel = 0;
                position.Y = Constants.Graphics.FLOOR;
                kirby.ChangePose(KirbyPose.FloatingGrounded);
            }
        }
        public void AdjustYPositionWhileNotFloating(Player kirby)
        {
            //dont go through the floor but floating was ended
            if (position.Y > Constants.Graphics.FLOOR)
            {
                position.Y = Constants.Graphics.FLOOR;
                kirby.ChangePose(KirbyPose.Standing);
                kirby.ChangeMovement();
            }
        }

        public override void AdjustY(Player kirby)
        {
            if (endFloat)
            {
                AdjustYPositionWhileNotFloating(kirby);
            } else {
                AdjustYPositionWhileFloating(kirby);
            }

            //dont go through the ceiling
            if (position.Y < 20)
            {
                yVel = 0;
                position.Y = 20;
            }
        }

        public override void UpdatePosition(GameTime gameTime)
        {
            position.X += xVel;
            position.Y += yVel;
            yVel += floatGravity;
        }
        //the animation of kirby letting air go
        public void FloatingEndAnimation(Player kirby)
        {
            kirby.ChangePose(KirbyPose.FloatingEnd);
        }

        public async void FloatingFallingAnimation(Player kirby)
        {
            //floating doesnt go into the falling animation within a certain distance from floor
            if (position.Y < floatFallingWindow)
            {
                await Task.Delay(Constants.Physics.DELAY2);
                kirby.ChangePose(KirbyPose.JumpFalling);

            }
        }

        //attack (or pressing z) undoes float
        public override void Attack(Player kirby)
        {
            kirby.ChangeAttackBool(true);
            FloatingEndAnimation(kirby);
            floatGravity = gravity;
            endFloat = true;
            yVel = floatVel;
            FloatingFallingAnimation(kirby);
        }

    }
}
