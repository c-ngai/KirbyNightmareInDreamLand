using KirbyNightmareInDreamLand.Entities.Enemies;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public class WaddleDeeCollisionHandler : ICollidable
    {
        public Rectangle BoundingBox { get; private set; }
        public bool IsDynamic { get; private set; } = true;
        public bool IsActive { get; private set; } = true;
        private IEnemy Enemy;

        public WaddleDeeCollisionHandler(int x, int y, WaddleDee entity)
        {
            BoundingBox = new Rectangle(x, y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
            CollisionManager.Instance.RegisterDynamicObject(this);
            Enemy = entity;
        }
        // Update the bounding box based on the player's current position and size
        public void DestroyHitBox()
        {
            IsActive = false;  // Mark enemy as inactive
        }
        public void EnableHitBox()
        {
            IsActive = true;  // Mark enemy as inactive
        }
        public void UpdateBoundingBox(Vector2 pos)
        {
            BoundingBox = new Rectangle((int)pos.X, (int)pos.Y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
        }

        public void OnCollision(ICollidable other)
        {
            if (other.IsDynamic)
            {
                DestroyHitBox();
                Enemy.TakeDamage();
            }
        }
    }

}