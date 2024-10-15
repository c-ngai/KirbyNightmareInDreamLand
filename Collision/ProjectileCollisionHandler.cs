using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public class ProjectileCollisionHandler : ICollidable
    {
        public Rectangle BoundingBox { get; private set; }

        public bool IsDynamic { get; private set; } = true;
        public bool IsActive { get; private set; } = true;
        public ProjectileCollisionHandler(int x, int y)
        {
            BoundingBox = new Rectangle(x, y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
            CollisionDetection.Instance.RegisterDynamicObject(this);
        }
        public void DestroyHitBox()
        {
            IsActive = false;  // Mark enemy as inactive
        }
        public void EnableHitBox()
        {
            IsActive = true;  // Mark enemy as inactive
        }

        public void OnCollision(ICollidable other)
        {
            //does nothing
        }  
        public void UpdateBoundingBox(Vector2 pos)
        {
            BoundingBox = new Rectangle((int)pos.X, (int)pos.Y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
        }
    }
}