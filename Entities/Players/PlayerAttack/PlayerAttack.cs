
//using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Projectiles;
using System.Collections.Generic;
using KirbyNightmareInDreamLand;
using System;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class PlayerAttack
    {
        public IProjectile currentAttack {get; private set;}

        private Dictionary<string, Func<Player, IProjectile>> attackFactories;
        private Vector2 position;
        private bool isLeft;
        public PlayerAttack(Player kirby, string attackType)
        {
            //InitializeAttackDictionary();
            position = kirby.GetKirbyPosition();
            isLeft = kirby.IsLeft();
            // Set the attack based on the string
            currentAttack = AttackFactory(kirby, attackType);
        }

        // THIS METHOD SUCKS AND IS A BAND-AID REPAIR FOR THE METHOD BELOW
        private IProjectile AttackFactory(Player kirby, string attackType)
        {
            switch (attackType)
            {
                case ("Beam"):
                    return new KirbyBeam(kirby, !isLeft);
                case ("Fire"):
                    return new KirbyFlamethrower(kirby, !isLeft);
                case ("Puff"):
                    return new KirbyPuff(position, !isLeft);
                case ("Normal"):
                    return new Inhale(position, isLeft, kirby);
                case ("Spark"):
                    return new ElectricAttack(position, isLeft);
                case ("Slide"):
                    return new Slide(position, isLeft, kirby);
                case ("Star"):
                    return new KirbyStar(position, !isLeft);
                case ("BouncingStar"):
                    return new KirbyBouncingStar(position, !isLeft, kirby.GetPowerUp());
                default:
                    Debug.WriteLine(" [ERROR] PlayerAttack: No attack for string \"" + attackType +"\"");
                    return null;
            }
        }

        public void EndAttack()
        {
            currentAttack.EndAttack();
            currentAttack = null;
        }

        public bool IsDone()
        {
            return currentAttack.IsDone();
        }

        //public void Update(GameTime gameTime, Player kirby)
        //{
        //    currentAttack.Update(); 
        //}
        //public void Draw(SpriteBatch spriteBatch, Player kirby)
        //{
        //   currentAttack.Draw(spriteBatch);
        //}


    }
}
