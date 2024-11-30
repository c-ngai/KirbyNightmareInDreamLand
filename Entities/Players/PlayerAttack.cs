
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

        public string attackType;
        public IProjectile currentAttack {get; private set;}
        private Vector2 position;
        private bool isLeft;
        public PlayerAttack(Player kirby, string _attackType)
        {
            //InitializeAttackDictionary();
            position = kirby.GetKirbyPosition();
            isLeft = kirby.IsLeft();
            // Set the attack based on the string
            currentAttack = AttackFactory(kirby, _attackType);
            attackType = _attackType;
        }

        // THIS METHOD SUCKS AND IS A BAND-AID REPAIR FOR THE METHOD BELOW
        private IProjectile AttackFactory(Player kirby, string attackType)
        {
            switch (attackType)
            {
                case ("Normal"):
                    return new Inhale(position, isLeft, kirby);
                case ("Beam"):
                    return new KirbyBeam(kirby, !isLeft);
                case ("Spark"):
                    return new ElectricAttack(kirby, position, isLeft);
                case ("Fire"):
                    return new KirbyFlamethrower(kirby, !isLeft);
                case ("Professor"):
                    return new KirbyBriefcase(kirby, position, isLeft);
                case ("Star"):
                    return new KirbyStar(kirby, position, !isLeft);
                case ("Puff"):
                    return new KirbyPuff(kirby, position, !isLeft);
                case ("Slide"):
                    return new Slide(position, isLeft, kirby);
                default:
                    Debug.WriteLine(" [ERROR] PlayerAttack: No attack for string \"" + attackType +"\"");
                    return null;
            }
        }

        public void EndAttack()
        {
            if (attackType != "Star" && attackType != "Professor") // this is dumb fix later if time -mark at 1am
            {
                currentAttack.EndAttack();
            }
        }

        public bool IsDone()
        {
            return currentAttack.IsDone();
        }

    }
}
