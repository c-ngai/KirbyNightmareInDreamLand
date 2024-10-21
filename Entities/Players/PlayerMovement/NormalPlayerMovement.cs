using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Projectiles;

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
            if(!kirby.IsWithEnemy()) StopMovement();
        }

        public override void MovePlayer(Player kirby, GameTime gameTime)
        {
            //if kirby is not attacking
            //or in the case he is it doesnt apply for the star attack
            if((kirby.attack == null) || (kirby.attack.currentAttack.GetType() == typeof(KirbyStar)) )// if he is not attacking or is spweing an enemey
            {
                UpdatePosition(gameTime);
            }
            Adjust(kirby);
        }

    }
}
