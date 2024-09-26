using Microsoft.Xna.Framework;
using System.Threading.Tasks.Dataflow;

namespace MasterGame
{
    public class FloatingMovement : PlayerMovement
    {

        protected float floatVel = .50f;
        protected float floatGravity = 5f;
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

        public override void UpdatePosition(GameTime gameTime)
        {
            position.X += xVel;
            position.Y += yVel;
            yVel += floatGravity *  (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Attack(Player kirby)
        {
            
        }

    }
}
