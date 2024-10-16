using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public class TileCollisionHandler : ICollidable
    {
        public Rectangle BoundingBox { get; private set; }

        // Tiles are always static
        public bool IsDynamic { get; private set; } = false;

        // Tiles are always active; aka collidable
        public bool IsActive { get; private set; } = true;
        private Tile tile;


        public TileCollisionHandler(Tile newTile)
        {
            BoundingBox = tile.rectangle;
            CollisionManager.Instance.RegisterStaticObject(newTile, this);
            tile = newTile;
        }

        public void AddToCollisionMapping()
        {

        }
       public void DestroyHitBox()
        {

        }
        public void EnableHitBox()
        {

        }
        public void OnCollision(ICollidable other)
        {
            // Does nothing, tiles don't react
        }

        public void UpdateBoundingBox(Vector2 pos)
        {
            // Does nothing, tile is static
        }
    }
}