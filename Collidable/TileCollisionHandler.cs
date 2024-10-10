using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public class TileCollisionHandler : ICollidable
    {
        public Rectangle BoundingBox { get; private set; }
        public bool IsDynamic { get; private set; } = true;
        public TileCollisionHandler()
        {

        }

       
        public void OnCollision(ICollidable other)
        {
            //
        }

        public void UpdateBoundingBox(Vector2 pos)
        {
            //does nothing, tile is static
        }
    }
}