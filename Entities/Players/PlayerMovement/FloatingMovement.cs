using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Threading.Tasks.Dataflow;

namespace MasterGame
{
    public class FloatingMovement : PlayerMovement
    {
        protected float floatVel = .50f;
        protected float floatGravity = 2f;
        protected bool endFloat = false;        
        public FloatingMovement(Vector2 pos) : base(pos)
        {
            floating = true;
        }
        public override void Walk(bool isLeft)
        {   
           if(isLeft){
                xVel = floatVel * -1;
            } else {
                xVel = floatVel;
            }
        }

        public override void Run(bool isLeft)
        {   
            Walk(isLeft);
        }

        public override void Jump(bool isLeft)
        {
            yVel = floatVel * -1;
        }

        public override void EndFloat()
        {
            yVel = floatVel;
        }
        public async void FloatingEndAnimation(Player kirby)
        {
            kirby.ChangePose(KirbyPose.FloatingEnd);
            await Task.Delay (500);
        }

        public override void AdjustY(Player kirby)
        {
            //dont go through the floor
            if(position.Y > Constants.Graphics.FLOOR  && endFloat)
            {
                FloatingEndAnimation(kirby);
                kirby.ChangePose(KirbyPose.Standing);
                kirby.ChangeMovement();
            }
            if(position.Y > Constants.Graphics.FLOOR  && !endFloat)
            {
                yVel = 0;
                xVel=0;
                position.Y = (float) Constants.Graphics.FLOOR;
                kirby.ChangePose(KirbyPose.FloatingGrounded);
            }
            //dont go through the ceiling
            if(position.Y < 20)
            {
                yVel = 0;
                position.Y = 10;
            }

        }

        public override void UpdatePosition(GameTime gameTime)
        {
            position.X += xVel;
            position.Y += yVel;
            yVel += floatGravity *  (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Attack(Player kirby)
        {
            floatGravity = gravity;
            endFloat = true;
            EndFloat();
            kirby.ChangePose(KirbyPose.JumpFalling);
        }

    }
}
