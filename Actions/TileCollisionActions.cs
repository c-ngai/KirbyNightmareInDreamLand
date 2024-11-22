using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Projectiles;
using KirbyNightmareInDreamLand.Collision;
using System.Numerics;
using KirbyNightmareInDreamLand.Levels;
using System.Diagnostics;
using System;
namespace KirbyNightmareInDreamLand.Actions
{
    public class TileCollisionActions
    {
        public static void BottomBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player) 
            {
                player.BottomCollisionWithBlock(intersection);
            }
            else if (object1 is IEnemy enemy)
            {
                enemy.BottomCollisionWithBlock(intersection);
            }
            else if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            } 
            else if (object1 is KirbyBouncingStar star) 
            {
                star.FloorBounce();
            }
        }

        public static void RightBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player)
            {
                player.RightCollisionWithBlock(intersection);
            }
            else if (object1 is IEnemy enemy)
            {
                enemy.RightCollisionWithBlock(intersection);
            }
            else if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            }   
            else if (object1 is KirbyBouncingStar star) 
            {
                star.WallRightBounce();
            }
        }
        public static void LeftBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player)
            {
                player.LeftCollisionWithBlock(intersection);
            }
            else if (object1 is IEnemy enemy)
            {
                enemy.LeftCollisionWithBlock(intersection);
            }
            else if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            }
            else if (object1 is KirbyBouncingStar star) 
            {
                star.WallLeftBounce();
            }
        }

        public static void TopBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player)
            {
                player.TopCollisionWithBlock(intersection);
            }
            else if (object1 is IEnemy enemy)
            {
                enemy.TopCollisionWithBlock(intersection);
            }
            else if (object1 is IExplodable projectile)
            {
                projectile.EndAttack();
            }
        }

        public static void BottomPlatformCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            if (object1 is IPlayer player)
            {
                player.BottomCollisionWithPlatform(intersection);
            }
            else if (object1 is IEnemy enemy)
            {
                enemy.BottomCollisionWithPlatform(intersection);
            }
            else if (object1 is KirbyBouncingStar star) 
            {
                star.FloorBounce();
            }

        }

        public static void BottomAirCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            CollisionType type = object1.GetCollisionType();
            if (type == CollisionType.Enemy)
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
            CollisionType type = object1.GetCollisionType();
            Tile tile = (Tile)object2;
            if (type == CollisionType.Player)
            {
                Player player = (Player)object1;
            
                player.CollisionWithGentle1LeftSlope(tile);
            }
            else if (type == CollisionType.Enemy)
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustGentle1SlopeLeftCollision(tile);
            } 
            else if (object1 is KirbyBouncingStar star) 
            {
                star.FloorBounce();
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
            else if (type == CollisionType.Enemy)
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustGentle1SlopeRightCollision(tile);
            } 
            else if (object1 is KirbyBouncingStar star) 
            {
                star.FloorBounce();
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
            else if (type == CollisionType.Enemy)
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustGentle2SlopeLeftCollision(tile);
            }
            else if (object1 is KirbyBouncingStar star) 
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
            else if (type == CollisionType.Enemy)
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustGentle2SlopeRightCollision(tile);
            }
            else if (object1 is KirbyBouncingStar star) 
            {
                star.FloorBounce();
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
            else if (type == CollisionType.Enemy)
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustSteepSlopeLeftCollision(tile);
            }
            else if (object1 is KirbyBouncingStar star) 
            {
                star.FloorBounce();
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
            else if (type == CollisionType.Enemy)
            {
                Enemy enemy = (Enemy)object1;
                enemy.AdjustSteepSlopeRightCollision(tile);
            }
            else if (object1 is KirbyBouncingStar star) 
            {
                star.FloorBounce();
            }
        }
    }
}