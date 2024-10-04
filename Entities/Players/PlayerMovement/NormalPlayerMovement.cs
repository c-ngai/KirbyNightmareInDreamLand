using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Players
{   
    public class NormalPlayerMovement : PlayerMovement
    {
        public NormalPlayerMovement(Vector2 pos) : base(pos) {}
   
        public override void Attack(Player kirby)
        {
            //beam aatack finishes animation (once)

            //electric is while z is pressed
            //fire is while z is pressed 
            //inhaling is while z is pressed
            //exhale is only a one time event

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
