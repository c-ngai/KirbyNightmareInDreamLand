using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public class TileCollisionHandler : ICollidable
    {
        public Rectangle BoundingBox { get; private set; }
        public bool IsDynamic { get; private set; } = true;
        public bool IsActive { get; private set; } = true;
        public TileCollisionHandler()
        {

        }

       public void DestroyHitBox()
        {
            //IsActive = false;  // Mark enemy as inactive
        }
        public void EnableHitBox()
        {
            //IsActive = true;  // Mark enemy as inactive
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