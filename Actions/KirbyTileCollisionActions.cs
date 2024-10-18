
namespace KirbyNightmareInDreamLand.Actions
{
    public static class KirbyTileCollisionActions
    {
        public static void KirbyBottomBlockCollision(ICollidable object1)
        {
            Tile tile = (Tile) object1;
            //Game1.Instance.players[0].BottomCollisionWithBlock(tile);
        }

        public static void KirbyRightBlockCollision(ICollidable object1)
        {
            Tile tile = (Tile)object1;
            //Game1.Instance.players[0].RightCollisionWithBlock(tile);
        }

        public static void KirbyLeftBlockCollision(ICollidable object1)
        {
            //Tile tile = (Tile)object1.ObjectComponent();
           // Game1.Instance.players[0].LeftCollisionWithBlock(tile);
        }
    }
}
