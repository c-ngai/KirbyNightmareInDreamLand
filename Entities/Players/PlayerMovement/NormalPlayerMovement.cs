using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Players
{   
    public class NormalPlayerMovement : PlayerMovement
    {
        public NormalPlayerMovement(Vector2 pos) : base(pos)
        {
            normal = true;
        }
   
        public override void Attack(Player kirby)
        {
            //attacks are stationary
            xVel = 0;
            //if kirby is in normal state he inhales first and attacks one he has an enemy
            if(kirby.GetKirbyType().Equals("Normal"))// && !kirby.GetPose().Equals("WithEnemy"))
            {
                kirby.ChangePose(KirbyPose.Inhaling);
            } else {
                //all other states immediately attack
                kirby.ChangePose(KirbyPose.Attacking);
                kirby.ChangeAttackBool(true);
            }
        }

    }
}
