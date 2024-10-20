using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyFireball : IProjectile, ICollidable, IExplodable
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        public bool IsActive = true;

        public Vector2 Position
        {
            get => position;            // Return position of fireball
            set => position = value;    // Set the position of the fireball to the given value
        }

        public Vector2 Velocity
        {
            get => velocity;            // Return the current velocity of the fireball
            set => velocity = value;    // Set the velocity of the fireball to the given value
        }

        public EnemyFireball(Vector2 startPosition, Vector2 fireballDirection)
        {
            Position = startPosition;

            // Normalize the direction vector and multiply by the constant speed
            if (fireballDirection != Vector2.Zero)
            {
                fireballDirection.Normalize(); // Ensures the vector has a length of 1
            }
            
            Velocity = fireballDirection * Constants.EnemyFire.SPEED;

            projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_hothead_fireball");
            ObjectManager.Instance.RegisterDynamicObject(this);
        }
        public string GetObjectType()
        {
            return "EnemyAttack";
        }

        public void Update()
        {
            if (IsActive)
            {
                Position += Velocity;

                projectileSprite.Update();
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
            else
            {
                ObjectManager.Instance.RemoveDynamicObject(this); // Deregister if dead
            }
        }
        public void EndAttack()
        {
            IsActive = false;
            CollisionActive = false;
        }
        public bool IsDone()
        {
            return true;
        }

        public bool CollisionActive { get; set; } = true;

        public virtual Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.FIREBALL_WIDTH / 2;
            float y = pos.Y - Constants.HitBoxes.FIREBALL_HEIGHT + Constants.HitBoxes.FIREBALL_OFFSET;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint;
        }
        public virtual Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.FIREBALL_WIDTH, Constants.HitBoxes.FIREBALL_HEIGHT);
        }

        public virtual void BottomBlockCollision(Rectangle intersection)
        {
            ObjectManager.Instance.RemoveDynamicObject(this);
            IsActive = false;
        }

        public virtual void RightBlockCollision(Rectangle intersection)
        {
            ObjectManager.Instance.RemoveDynamicObject(this);
            IsActive = false;
        }

        public virtual void LeftBlockCollision(Rectangle intersection)
        {
            ObjectManager.Instance.RemoveDynamicObject(this);
            IsActive = false;
        }
    }
}
