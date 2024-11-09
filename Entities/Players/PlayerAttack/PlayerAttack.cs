
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
        public IProjectile currentAttack {get; private set;}

        private Dictionary<string, Func<Player, IProjectile>> attackFactories;
        private Vector2 position;
        private bool isLeft;
        public PlayerAttack(Player kirby, String attackType)
        {
            InitializeAttackDictionary();
            position = kirby.GetKirbyPosition();
            isLeft = kirby.IsLeft();
            // Set the attack based on the string
            currentAttack = attackFactories[attackType](kirby);
        }
        public void InitializeAttackDictionary()
        {
            attackFactories  = new Dictionary<string, Func<Player, IProjectile>>
            {
                { "Beam", (k) => new KirbyBeam(position, !isLeft) },
                { "Fire", (k) => new KirbyFlamethrower(position, !isLeft) },
                { "Puff", (k) => new KirbyPuff(position, !isLeft) },
                { "Normal", (k) => new Inhale(position, isLeft, k) },
                { "Spark", (k) => new ElectricAttack(position, isLeft) },
                { "Slide", (k) => new Slide(position, isLeft) },
                { "Star", (k) => new KirbyStar(position, !isLeft) }
            };
        }
        public void EndAttack()
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
