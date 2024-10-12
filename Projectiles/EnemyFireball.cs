using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyFireball : IProjectile
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;

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
        }

        public void Update()
        {
            Position += Velocity;

            projectileSprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            projectileSprite.Draw(Position, spriteBatch);
        }
        public void EndAttack()
        {
            //
        }
        public bool IsDone()
        {
            return true;
        }
    }
}
