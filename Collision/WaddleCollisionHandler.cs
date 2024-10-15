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
            CollisionDetection.Instance.RegisterDynamicObject(this);
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
        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.ENTITY_WIDTH/2;
            float y = pos.Y - Constants.HitBoxes.ENTITY_HEIGHT;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint; 
        }
        // Update the bounding box based on the player's current position and size
        public void UpdateBoundingBox(Vector2 pos)
        {
            Vector2 rectPoint = CalculateRectanglePoint(pos);
            BoundingBox = new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
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