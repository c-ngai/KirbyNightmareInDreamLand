using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{   
    public class NormalPlayerMovement : PlayerMovement
    {
        public NormalPlayerMovement(ref PlayerStateMachine pState) : base(ref pState)
        {

        }
   

        public override void Attack()
        {
            if(state.GetPose() == KirbyPose.Attacking){
                state.ChangePose(KirbyPose.ThrowEnemy);
            } else {
                state.ChangePose(KirbyPose.Attacking);
            }
        }
        //change kirby yVelcoity to jump
        public void Jump() 
        {
            if(yVel == 0){
                yVel = jumpVel;
            }
        }

        //floats kirby
        public void Float()
        {
            
        }

        
    }
}
