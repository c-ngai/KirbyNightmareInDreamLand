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

        public PlayerCollisionHandler(int x, int y, Player player)
        {
            BoundingBox = new Rectangle(x, y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
            CollisionManager.Instance.RegisterDynamicObject(this);
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
        // Update the bounding box based on the player's current position and size
        public void UpdateBoundingBox(Vector2 pos)
        {
            BoundingBox = new Rectangle((int)pos.X, (int)pos.Y, Constants.HitBoxes.ENTITY_WIDTH, Constants.HitBoxes.ENTITY_HEIGHT);
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
            if (!CanCollide(other)) return;
            if (other.IsDynamic)
            {
                Player.TakeDamage();
            }
        }
    }

}