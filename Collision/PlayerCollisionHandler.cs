using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
namespace KirbyNightmareInDreamLand
{
    public class PlayerCollisionHandler : ICollidable
    {
        public Rectangle BoundingBox { get; private set; }
        public bool IsDynamic { get; private set; } = true;
        public bool IsActive { get; private set; } = true;
        private IPlayer Player;
        //im changing collision manager to collision detection
        //then change handler to response --on your side
        //
        public PlayerCollisionHandler(int x, int y, Player player)
        {
            BoundingBox = new Rectangle(x, y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
            CollisionDetection.Instance.RegisterDynamicObject(this);
            Player = player;
        }
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
        // Method to toggle colliding with dynamic objects (invincibility against dynamic objects)
        public void SetCollidableWithDynamic(bool collidable)
        {
            IsActive = collidable;
        }
        public bool CanCollide(ICollidable other)
        {
            if (!IsActive && other.IsDynamic)
            {
                // If the player is invincible and the other object is dynamic, ignore collision
                return false;
            }
            return true; // Always collide with static objects
        }

        public void OnCollision(ICollidable other)
        {
            if (!IsActive) return;
            if (other.IsDynamic)
            {
                Player.TakeDamage();
            }
        }
        public void TakeDamage()
        {
            if (!IsActive) return;
            Player.TakeDamage();
        }

    }

}