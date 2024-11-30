using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyFireball : IProjectile, ICollidable, IExplodable
    {
        public IPlayer player { get => null; } // this projectile never originates from a player
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
            ObjectManager.Instance.AddProjectile(this);
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.EnemyAttack;
        }

        public void Update()
        {
            if (IsActive)
            {
                Position += Velocity;

                projectileSprite.Update();

                // Despawn if outside room
                if (position.X < -16 || position.X > Game1.Instance.Level.CurrentRoom.Width + 16)
                {
                    IsActive = false;
                    CollisionActive = false;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
        }
    
        public bool IsDone()
        {
            return !IsActive;
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
        public Vector2 GetPosition()
        {
            return Position;
        }
        public void EndAttack()
        {
            IsActive = false;
            CollisionActive = false;
        }
    }
}
