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
            string type = object2.GetObjectType();
            if (type.Equals("Player"))
            {
                Player player = (Player)object2;
                player.TakeDamage(intersection);
            }
            type = object1.GetObjectType();
            if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.TakeDamage(intersection);
            }
        }

        //kirby collides with an enemy attack 
        public static void KirbyEnemyAttackCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player player = (Player)object1;
            player.TakeDamage(intersection);

            if(object2 is EnemyFireball)
            {
                EnemyFireball fire = (EnemyFireball)object2;
                fire.EndAttack();
            }
        }

        //enemy collides with kirby attack --check for inhale
        public static void EnemyKirbyAttackCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.TakeDamage(intersection);
            }

            if(object2 is Inhale)
            {
                Inhale attack = (Inhale)object2;
                attack.OnCollide(); //change skirby to mouthful
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

        //kirby intercats with powerupstar 
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
     
        public static void EntityCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player player = (Player)object2;
            Enemy enemy = (Enemy)object1;
            player.TakeDamage();
            enemy.TakeDamage();
        }
    }
}
