using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;

namespace KirbyNightmareInDreamLand.Entities.Players
{   
    public class NormalPlayerMovement : PlayerMovement
    {
        public NormalPlayerMovement(Vector2 pos) : base(pos) {}

        //puff is a whole nother thing
        public override void Attack(Player kirby)
        {
            //beam aatack finishes animation (once)
            //exhale is only a one time event
            //puff is one time

            //attacks are stationary
            StopMovement();
            //if kirby is in normal state he inhales first and attacks one he has an enemy
            // if(kirby.GetKirbyType().Equals("Normal"))// && !kirby.GetPose().Equals("WithEnemy"))
            // {
            //     kirby.ChangePose(KirbyPose.Inhaling);
            // } else {
                //all other states immediately attack
                if(!kirby.GetKirbyType().Equals("Normal")){
                    kirby.ChangePose(KirbyPose.Attacking);
                    kirby.ChangeAttackBool(true);
                } else {
                    kirby.ChangePose(KirbyPose.Inhaling);
                }
            //}
        }

        public override void AttackPressed(Player kirby)
        {
            //electric is while z is pressed
            //fire is while z is pressed 
            //inhaling is while z is pressed

            //attacks are stationary
            StopMovement();
            //if kirby is in normal state he inhales first and attacks one he has an enemy
            // if(kirby.GetKirbyType().Equals("Normal"))// && !kirby.GetPose().Equals("WithEnemy"))
            // {
            if(!kirby.GetKirbyType().Equals("Normal")){
                kirby.ChangePose(KirbyPose.Attacking);
                kirby.ChangeAttackBool(true);
            } else {
                kirby.ChangePose(KirbyPose.Inhaling);
            }
            // } else {
            //     //all other states immediately attack
            //     kirby.ChangePose(KirbyPose.Attacking);
            //     kirby.ChangeAttackBool(true);
            // }
        }
        public override void MovePlayer(Player kirby, GameTime gameTime)
        {
            if(!kirby.attackIsActive)
            {
                UpdatePosition(gameTime);
            }
            Adjust(kirby);
        }

    }
}
