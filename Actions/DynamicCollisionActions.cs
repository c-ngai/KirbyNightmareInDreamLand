using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.Entities.PowerUps;

namespace KirbyNightmareInDreamLand.Actions
{
    public class DynamicCollisionActions
    {
        // Kirby and enemy collide with each other
        public static void KirbyEnemyCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            CollisionType type1 = object1.GetCollisionType();
            CollisionType type2 = object2.GetCollisionType();

            // handles collision for each entity
            if (type1 == CollisionType.Enemy && type2 == CollisionType.Player)
            {
                Enemy enemy = (Enemy)object1;
                Player player = (Player)object2;
                
                // differentiates between collision from kirby inhale vs regular physical contact
                if (enemy.IsBeingInhaled)
                {
                    enemy.GetSwallowed(intersection);
                    player.EatEnemy(enemy.PowerType());
                }
                else
                {
                    enemy.TakeDamage(null, intersection, player.GetHitBox().Center.ToVector2());
                    player.TakeDamage(object1, intersection, enemy.GetHitBox().Center.ToVector2());
                }
            }
        }

        #region attackCollisions
        // Kirby collides with an enemy attack 
        public static void KirbyEnemyAttackCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player player = (Player)object2;
            player.TakeDamage(object1, intersection, object1.GetPosition());

            if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            }
        }

        // enemy collides with Kirby attack - checks for inhale
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
                    enemy.GetInhaled(intersection, attack.player);
                }
                else
                {
                    enemy.TakeDamage(null, intersection, object2.GetHitBox().Center.ToVector2());
                }
            }
        }

        // bouncing star collides with inhale attack
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

        // re-swallowing of a Kirby power-up star 
        public static void KirbyBouncingStarCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is KirbyBouncingStar star && object2 is Player player)
            {
                if (star.isBeingInhaled)
                {
                    star.GetSwallowed(intersection);
                    player.EatEnemy(star.PowerUp());
                }
            }
        }
        #endregion

        // Kirby interacts with item, currently its just the powerup tomato so kirby simply regains health
        public static void KirbyItemCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player player = (Player)object1;
            player.FillHealth();

            PowerUp pu = (PowerUp)object2;
            pu.UsePowerUp();
        }
    }
}
