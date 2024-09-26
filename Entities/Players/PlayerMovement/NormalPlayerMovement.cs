using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{   
    public class NormalPlayerMovement : PlayerMovement
    {
        public NormalPlayerMovement(Vector2 pos) : base(pos)
        {
            normal = true;
        }
   
        public override void Attack(Player kirby)
        {
            if(kirby.GetKirbyType().Equals("Normal") && kirby.GetPose().Equals("WithEnemy"))
            {
                kirby.ChangePose(KirbyPose.ThrowEnemy);
            } else {
                kirby.ChangePose(KirbyPose.Attacking);
            }
        }

    }
}
