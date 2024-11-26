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
            //if(!kirby.IsWithEnemy()) StopMovement();
        }

        public override void MovePlayer(Player kirby, GameTime gameTime)
        {
            //if kirby is not attacking
            //or in the case he is it doesnt apply for the star attack
            if((kirby.attack == null) || kirby.starAttackOne != null || kirby.starAttackTwo != null )// if he is not attacking or is spweing an enemey
            {
                UpdatePosition(kirby);
            }
            Adjust(kirby);
            DeathBarrierCheck(kirby);
        }

    }
}
