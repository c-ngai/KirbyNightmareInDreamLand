using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using System.Collections.Generic;
using System;
using KirbyNightmareInDreamLand.Projectiles;
using System.Runtime.Serialization;
using KirbyNightmareInDreamLand.Entities.PowerUps;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Actions
{
    public class DynamicCollisionActions
    {
        //kirby and enemy collide with each other
        public static void KirbyEnemyCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            CollisionType type1 = object1.GetCollisionType();
            CollisionType type2 = object2.GetCollisionType();
            if (type1 == CollisionType.Enemy && type2 == CollisionType.Player)
            {
                Enemy enemy = (Enemy)object1;
                Player player = (Player)object2;
                
                if (enemy.IsBeingInhaled)
                {
                    enemy.GetSwallowed(intersection);
                    player.SwallowEnemy(enemy.PowerType());
                }
                else
                {
                    enemy.TakeDamage(intersection, player.GetHitBox().Center.ToVector2());
                    player.TakeDamage(intersection, enemy.GetHitBox().Center.ToVector2());
                }
            }
        }

        //kirby collides with an enemy attack 
        public static void KirbyEnemyAttackCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player player = (Player)object2;
            player.TakeDamage(intersection, object1.GetPosition());

            if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            }
        }

        //enemy collides with kirby attack --check for inhale
        public static void EnemyKirbyAttackCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object2 is IExplodable projectile)
            {
                projectile.EndAttack();
            }

            CollisionType type = object1.GetCollisionType();
            if (type == CollisionType.Enemy)
            {
                Enemy enemy = (Enemy)object1;

                if (object2 is Inhale)
                {
                    Inhale attack = (Inhale)object2;
                    //attack.OnCollide(enemy.PowerType()); //change skirby to mouthful
                    enemy.GetInhaled(intersection, attack.player);
                }
                else
                {
                    enemy.TakeDamage(intersection, object2.GetHitBox().Center.ToVector2());
                }
            }

            
        }

        //boincing star collides with inhale attack
        public static void PlayerAttackBouncingStarCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if(object1 is KirbyBouncingStar star)
            {
                if (object2 is Inhale attack)
                {
                    star.GetInhaled(intersection, attack.player);
                }
            }
        }
        public static void KirbyBouncingStarCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is KirbyBouncingStar star && object2 is Player player)
            {
                if (star.isBeingInhaled)
                {
                    star.GetSwallowed(intersection);
                    player.SwallowEnemy(star.PowerUp());
                }
            }
        }

        //kirby intercats with item
        public static void KirbyItemCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player player = (Player)object1;
            player.FillHealth();

            PowerUp pu = (PowerUp)object2;
            pu.UsePowerUp();
        }

        //kirby interacts with powerupstar 
        public static void KirbyProjectileCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player player = (Player)object1;
            player.FillHealth();

            if(object2 is KirbyStar)
            {
                KirbyStar star = (KirbyStar)object2;
                star.EndAttack(); //change skirby to mouthful
            }
        }
    }
}
