using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.Levels;
namespace KirbyNightmareInDreamLand.Actions
{
    public class TileCollisionActions
    {
        /** ICollidable objects colliding with blocks from different directions **/
        #region blockCollisions
        // bottom side of a ICollidable object colliding with a block
        public static void BottomBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player)
            {
                player.BottomCollisionWithBlock(intersection);
            }
            else if (object1 is IEnemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.BottomCollisionWithBlock(intersection);
            }
            else if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.FloorBounce();
            }
        }

        // right side of a ICollidable object colliding with a block
        public static void RightBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player)
            {
                player.RightCollisionWithBlock(intersection);
            }
            else if (object1 is IEnemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.RightCollisionWithBlock(intersection);
            }
            else if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.WallRightBounce();
            }
        }

        // left side of a ICollidable object colliding with a block
        public static void LeftBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player)
            {
                player.LeftCollisionWithBlock(intersection);
            }
            else if (object1 is IEnemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.LeftCollisionWithBlock(intersection);
            }
            else if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.WallLeftBounce();
            }
            // TODO: extra visual effect for demo day (stretch goal)
            //else if (object1 is KirbyBriefcase sc)
            //{
            //    sc.Explode();
            //}
        }

        // top side of a ICollidable object colliding with a block
        public static void TopBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player)
            {
                player.TopCollisionWithBlock(intersection);
            }
            else if (object1 is IEnemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.TopCollisionWithBlock(intersection);
            }
            else if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.CeilingBounce();
            }
        }
        #endregion

        // bottom side of a ICollidable object colliding with a platform
        public static void BottomPlatformCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player)
            {
                player.BottomCollisionWithPlatform(intersection);
            }
            else if (object1 is IEnemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.BottomCollisionWithPlatform(intersection);
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.FloorBounce();
            }
            else if (object1 is KirbyBriefcase briefcase)
            {
                briefcase.EndAttack();
            }
            else if (object1 is EnemyBriefcase enemyBriefcase)
            {
                enemyBriefcase.EndAttack();
            }
        }

        // enemy colliding with water (dies)
        public static void WaterCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Enemy enemy = (Enemy)object1;
            enemy.TakeDamage(intersection, intersection.Center.ToVector2());
        }

        /** ICollidable objects colliding with the different slope types **/
        #region slopeCollisions
        public static void GentleLeftSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            CollisionType type = object1.GetCollisionType();
            Tile tile = (Tile)object2;
            if (type == CollisionType.Player)
            {
                Player player = (Player)object1;

                player.CollisionWithGentle1LeftSlope(tile);
            }
            else if (object1 is Enemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.CollisionWithGentle1LeftSlope(tile);
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.FloorBounce();
            }
            else if (object1 is KirbyBriefcase sc)
            {
                sc.EndAttack();
            }
            else if (object1 is EnemyBriefcase en)
            {
                en.EndAttack();
            }
        }

        public static void GentleRightSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            CollisionType type = object1.GetCollisionType();
            Tile tile = (Tile)object2;
            if (type == CollisionType.Player)
            {
                Player player = (Player)object1;

                player.CollisionWithGentle1RightSlope(tile);
            }
            else if (object1 is Enemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.CollisionWithGentle1RightSlope(tile);
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.FloorBounce();
            }
            else if (object1 is KirbyBriefcase sc)
            {
                sc.EndAttack();
            }
            else if (object1 is EnemyBriefcase en)
            {
                en.EndAttack();
            }
        }

        public static void MediumLeftSlopeCollison(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            CollisionType type = object1.GetCollisionType();
            Tile tile = (Tile)object2;
            if (type == CollisionType.Player)
            {
                Player player = (Player)object1;

                player.CollisionWithGentle2LeftSlope(tile);
            }
            else if (object1 is Enemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.CollisionWithGentle2LeftSlope(tile);
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.FloorBounce();
            }
        }

        public static void MediumRightSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            CollisionType type = object1.GetCollisionType();
            Tile tile = (Tile)object2;
            if (type == CollisionType.Player)
            {
                Player player = (Player)object1;

                player.CollisionWithGentle2RightSlope(tile);
            }
            else if (object1 is Enemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.CollisionWithGentle2RightSlope(tile);
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.FloorBounce();
            }
            else if (object1 is KirbyBriefcase sc)
            {
                sc.EndAttack();
            }
            else if (object1 is EnemyBriefcase en)
            {
                en.EndAttack();
            }
        }

        public static void SteepLeftSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            CollisionType type = object1.GetCollisionType();
            Tile tile = (Tile)object2;
            if (type == CollisionType.Player)
            {
                Player player = (Player)object1;

                player.CollisionWithSteepLeftSlope(tile);
            }
            else if (object1 is Enemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.CollisionWithSteepLeftSlope(tile);
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.FloorBounce();
            }
            else if (object1 is KirbyBriefcase sc)
            {
                sc.EndAttack();
            }
            else if (object1 is EnemyBriefcase en)
            {
                en.EndAttack();
            }
        }

        public static void SteepRightSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            CollisionType type = object1.GetCollisionType();
            Tile tile = (Tile)object2;
            if (type == CollisionType.Player)
            {
                Player player = (Player)object1;

                player.CollisionWithSteepRightSlope(tile);
            }
            else if (object1 is Enemy enemy && !enemy.IsBeingInhaled)
            {
                enemy.CollisionWithSteepRightSlope(tile);
            }
            else if (object1 is KirbyBouncingStar star && !star.isBeingInhaled)
            {
                star.FloorBounce();
            }
            else if (object1 is KirbyBriefcase sc)
            {
                sc.EndAttack();
            }
            else if (object1 is EnemyBriefcase en)
            {
                en.EndAttack();
            }
        }
        #endregion
    }
}