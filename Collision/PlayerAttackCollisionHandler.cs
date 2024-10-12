using Microsoft.Xna.Framework;

namespace KirbyNightmareInDreamLand
{
    public class PlayerAttackCollisionHandler : ICollidable
    {
        public Rectangle BoundingBox { get; private set; }

        public bool IsDynamic { get; private set; } = true;
        public bool IsActive { get; private set; } = true;
        public PlayerAttackCollisionHandler(int x, int y)
        {
            BoundingBox = new Rectangle(x, y, Constants.HitBoxes.ATTACK_SIZE, Constants.HitBoxes.ATTACK_SIZE);
            CollisionManager.Instance.RegisterDynamicObject(this);
            //System.Console.WriteLine(y);
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
            BoundingBox = new Rectangle((int)pos.X, (int)pos.Y, Constants.HitBoxes.ATTACK_SIZE, Constants.HitBoxes.ATTACK_SIZE);
        }
    }
}