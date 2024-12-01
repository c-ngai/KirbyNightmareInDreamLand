using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Projectiles;

namespace KirbyNightmareInDreamLand.Entities.Players
{   
    public class NormalPlayerMovement : PlayerMovement
    {
        public NormalPlayerMovement(Vector2 pos, Vector2 vel) : base(pos, vel) {}

        //puff is a whole nother thing
        public override void Attack(Player kirby)
        {
            //beam aatack finishes animation (once)
            //exhale is only a one time event
            //puff is one time

            //attacks are stationary
            // No they're not? commented out this line
            //if (!kirby.IsWithEnemy()) StopMovement();
        }


    }
}
