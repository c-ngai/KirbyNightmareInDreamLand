using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{   
    public class NormalPlayerMovement : PlayerMovement
    {
        protected float walkingVel = .25f;
        public NormalPlayerMovement() 
        {
            normal = true;
        }
   
        public override void Attack(Player kirby)
        {
            if(kirby.GetPose().Equals("Attacking")){
                kirby.ChangePose(KirbyPose.ThrowEnemy);
            } else {
                kirby.ChangePose(KirbyPose.Attacking);
            }
        }

    }
}
