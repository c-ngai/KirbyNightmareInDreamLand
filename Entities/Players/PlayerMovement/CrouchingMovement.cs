using Microsoft.Xna.Framework;
namespace MasterGame
{
    public class CrouchingMovement : PlayerMovement
    {
        public CrouchingMovement(Vector2 pos) : base(pos)
        {
            crouching = true;
        }

        public override void Walk(bool isLeft)
        {   
            //does nothing
        }

        public override void Run(bool isLeft)
        {   
            //does nothing
        }

        
    }
}