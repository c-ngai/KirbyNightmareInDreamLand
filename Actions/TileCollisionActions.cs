using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.Collision;
using KirbyNightmareInDreamLand.Levels;
using System.Diagnostics;
namespace KirbyNightmareInDreamLand.Actions
{
    public class TileCollisionActions
    {
        public static void BottomBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            if (type.Equals("Player"))
            {
                Player player = (Player)object1;
                player.BottomCollisionWithBlock(intersection);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.BottomCollisionWithBlock(intersection);
            }
            else
            {
                IExplodable projectile = (IExplodable)object1;
                projectile.CollisionWithBlock(intersection);
            }
        }

        public static void RightBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            if (type.Equals("Player"))
            {
                Player player = (Player)object1;
                player.RightCollisionWithBlock(intersection);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.ChangeDirection();
            }
            else
            {
                IExplodable projectile = (IExplodable)object1;
                projectile.CollisionWithBlock(intersection);
            }
        }

        public static void LeftBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            if (type.Equals("Player"))
            {
                Player player = (Player)object1;
                player.LeftCollisionWithBlock(intersection);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.ChangeDirection();
            }
            else
            {
                IExplodable projectile = (IExplodable)object1;
                projectile.CollisionWithBlock(intersection);
            }
        }

        public static void BottomPlatformCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player player = (Player)object1;
            player.BottomCollisionWithPlatform(intersection);
        }

        public static void BottomAirCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            //Debug.WriteLine($"type: {type}");
            if (type.Equals("Player"))
            {
                Player currentPlayer = (Player)object1;
                currentPlayer.BottomCollisionWithAir(intersection);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.BottomCollisionWithAir(intersection);
            }
        }

        public static void WaterCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Enemy enemy = (Enemy)object1;
            enemy.TakeDamage(intersection);
        }

        public static void GentleLeftSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            Tile tile = (Tile)object2;
            if (type.Equals("Player"))
            {
                Player player = (Player)object1;
            
                player.CollisionWithGentle1LeftSlope(tile);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustGentle1SlopeLeftCollision(tile);
            }
        }

        public static void GentleRightSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            Tile tile = (Tile)object2;
            if (type.Equals("Player"))
            {
                Player player = (Player)object1;

                player.CollisionWithGentle1RightSlope(tile);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustGentle1SlopeRightCollision(tile);
            }
        }

        public static void MediumLeftSlopeCollison(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            Tile tile = (Tile)object2;
            if (type.Equals("Player"))
            {
                Player player = (Player)object1;

                player.CollisionWithGentle2LeftSlope(tile);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustGentle2SlopeLeftCollision(tile);
            }
        }

        public static void MediumRightSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            Tile tile = (Tile)object2;
            if (type.Equals("Player"))
            {
                Player player = (Player)object1;

                player.CollisionWithGentle2RightSlope(tile);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustGentle2SlopeRightCollision(tile);
            }
        }

        public static void SteepLeftSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            Tile tile = (Tile)object2;
            if (type.Equals("Player"))
            {
                Player player = (Player)object1;

                player.CollisionWithSteepLeftSlope(tile);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustSteepSlopeLeftCollision(tile);
            }
        }

        public static void SteepRightSlopeCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            string type = object1.GetObjectType();
            Tile tile = (Tile)object2;
            if (type.Equals("Player"))
            {
                Player player = (Player)object1;

                player.CollisionWithSteepRightSlope(tile);
            }
            else if (type.Equals("Enemy"))
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustSteepSlopeRightCollision(tile);
            }
        }
    }
}