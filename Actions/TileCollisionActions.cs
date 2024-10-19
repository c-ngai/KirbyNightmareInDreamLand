using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Entities.Players;
namespace KirbyNightmareInDreamLand.Actions
{
    public class TileCollisionActions
    {
        public static ObjectManager manager { get; private set; } = ObjectManager.Instance;
        public static void BottomBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            //if (object1.GetType().Equals("Player"))
            //{
            Player player = (Player)object1;
            player.BottomCollisionWithBlock(intersection);
            //}
        }

        public static void RightBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            //if (object1.GetType().Equals("Player"))
            //{
            Player player = (Player)object1;
            player.RightCollisionWithBlock(intersection);
            //}
        }

        public static void LeftBlockCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            //if (object1.GetType().Equals("Player"))
            //{
            Player player = (Player)object1;
            player.LeftCollisionWithBlock(intersection);
            //}
        }

        public static void BottomPlatformCollision(ICollidable object1, ICollidable object2, Rectangle intersection)
        {
            //if (object1.GetType().Equals("Player"))
            //{
            Player player = (Player)object1;
            player.BottomCollisionWithPlatform(intersection);
            //}
        }

        public static void BottomAirCollision(ICollidable player, ICollidable object2, Rectangle intersection)
        {
            Player currentPlayer = (Player)player;
            currentPlayer.BottomCollisionWithAir(intersection);
        }

    }
}