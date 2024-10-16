
//using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Projectiles;
using System.Collections.Generic;
using KirbyNightmareInDreamLand;
using System;

namespace KirbyNightmareInDreamLand.Entities.Players
{
    public class PlayerAttack
    {
        private IProjectile currentAttack;

        private Dictionary<string, Func<Player, IProjectile>> attackFactories;
        public PlayerAttack(Player kirby, String attackType)
        {
            InitializeAttackDictionary();
            // Set the attack based on the string
            currentAttack = attackFactories[attackType](kirby);
        }
        public void InitializeAttackDictionary()
        {
            attackFactories  = new Dictionary<string, Func<Player, IProjectile>>
            {
                { "Beam", (k) => new KirbyBeam(k.GetKirbyPosition(), !k.IsLeft()) },
                { "Fire", (k) => new KirbyFlamethrower(k.GetKirbyPosition(), !k.IsLeft()) },
                { "Puff", (k) => new KirbyPuff(k.GetKirbyPosition(), !k.IsLeft()) },
                { "Normal", (k) => new Inhale(k.GetKirbyPosition(), k.IsLeft()) },
                { "Spark", (k) => new ElectricAttack(k.GetKirbyPosition(), k.IsLeft()) },
                { "Slide", (k) => new Slide(k.GetKirbyPosition(), k.IsLeft()) }
            };
        }
        public void EndAttack(Player kirby)
        {
            currentAttack.EndAttack();
        }
        public bool IsDone()
        {
            return currentAttack.IsDone();
        }
        public void Update(GameTime gameTime, Player kirby)
        {
            if(currentAttack is Slide attack)
            {
                attack.Update(kirby);
            } else {
                currentAttack.Update();
            }
            
        }
        public void Draw(SpriteBatch spriteBatch, Player kirby)
        {
           currentAttack.Draw(spriteBatch);
        }


    }
}
