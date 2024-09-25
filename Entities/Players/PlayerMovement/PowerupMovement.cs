using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{  
    public class PowerupMovement : PlayerMovement
    {
        public PowerupMovement(ref PlayerStateMachine pState) : base(ref pState)
        {}
        public override void Attack()
        {
            state.ChangePose(KirbyPose.Attacking);
        }
    }

}