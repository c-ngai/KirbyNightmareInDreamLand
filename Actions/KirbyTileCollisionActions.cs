using KirbyNightmareInDreamLand.Collision;
namespace KirbyNightmareInDreamLand.Actions
{
    public class KirbyTileCollisionActions
    {
        public static ObjectManager manager { get; private set; } = ObjectManager.Instance;
        public static void KirbyBottomBlockCollision(ICollidable object1)
        {
            Tile tile = (Tile)object1;
            manager.players[0].BottomCollisionWithBlock(tile);
        }

        public static void KirbyRightBlockCollision(ICollidable object1)
        {
            Tile tile = (Tile)object1;
            manager.players[0].RightCollisionWithBlock(tile);
        }

        public static void KirbyLeftBlockCollision(ICollidable object1)
        {
            Tile tile = (Tile)object1;
            manager.players[0].LeftCollisionWithBlock(tile);
        }

        public static void KirbyBottomPlatformCollision(ICollidable object1)
        {
            Tile tile = (Tile)object1;
            manager.players[0].BottomCollisionWithPlatform(tile);
        }

    }
}
