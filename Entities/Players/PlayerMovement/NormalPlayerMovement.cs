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
             System.Console.WriteLine("here player movement");
            if(!kirby.IsWithEnemy()) StopMovement();
        }

        public override void AttackPressed(Player kirby)
        {
            //electric is while z is pressed
            //fire is while z is pressed 
            //inhaling is while z is pressed normal_inhaling

            //attacks are stationary
            StopMovement();
            kirby.ChangePose(KirbyPose.Attacking);
        }
        public override void MovePlayer(Player kirby, GameTime gameTime)
        {
            if(!kirby.attackIsActive || kirby.IsWithEnemy()) // if he is attacking or is spweing an enemey
            {
                UpdatePosition(gameTime);
            }
            Adjust(kirby);
        }

    }
}
