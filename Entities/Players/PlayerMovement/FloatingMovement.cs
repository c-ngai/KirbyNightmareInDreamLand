using System.Threading.Tasks.Dataflow;

namespace MasterGame
{
    public class FloatingMovement : PlayerMovement
    {
        public FloatingMovement()
        {
            floating = true;
        }

        public override void Run(bool isLeft)
        {   
            Walk(isLeft);
        }

        public void GoUp(bool isLeft)
        {
            if(isLeft){
                xVel = walkingVel * -1;
            } else {
                xVel = walkingVel;
            }
        }
        public override void Jump()
        {
            
        }

    }
}
