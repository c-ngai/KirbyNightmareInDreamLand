using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.Enemies;
namespace KirbyNightmareInDreamLand.Actions
{
    public class TileCollisionActions
    {
        public static ObjectManager manager { get; private set; } = ObjectManager.Instance;

        // I don't think I can refactor these because I'm using type casting?
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

            }
        }

        public static void BottomPlatformCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {

            Player player = (Player)object1;
            player.BottomCollisionWithPlatform(intersection);
        }

        public static void BottomAirCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Player currentPlayer = (Player)object1;
            currentPlayer.BottomCollisionWithAir(intersection);
        }

        public static void WaterCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            Enemy enemy = (Enemy)object1;
            enemy.TakeDamage();
        }
    }
}