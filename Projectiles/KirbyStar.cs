using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class KirbyStar : IProjectile
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

        public KirbyStar(Vector2 startPosition, Vector2 starDirection)
        {
            Position = startPosition;

            // Normalize the direction vector and multiply by the constant speed
            if (starDirection != Vector2.Zero)
            {
                starDirection.Normalize(); // Ensures the vector has a length of 1
            }

            Velocity = starDirection * Speed; // Apply the constant speed

            // Check the direction and assign the appropriate sprite
            if (starDirection.X >= 0)
            {
                projectileSprite = SpriteFactory.Instance.createSprite("projectile_kirby_star_right");
            }
            else if (starDirection.X < 0)
            {
                projectileSprite = SpriteFactory.Instance.createSprite("projectile_kirby_star_left");
            }
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
    }
}
