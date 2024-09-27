using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class KirbyPuff : IProjectile
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        private const float InitialSpeed = 3.5f; 
        private const float DecelerationRate = 0.05f; 
        private const int MaxFrames = 20; // Puff disappears after 20 frames
        public bool isActive = true;

        public Vector2 Position
        {
            get => position;            // Return position of puff
            set => position = value;    // Set the position of the puff to the given value
        }

        public Vector2 Velocity
        {
            get => velocity;            // Return the current velocity of the puff
            set => velocity = value;    // Set the velocity of the puff to the given value
        }

        public KirbyPuff(Vector2 startPosition, Vector2 puffDirection)
        {
            Position = startPosition;

            // Normalize the direction vector and multiply by the initial speed
            if (puffDirection != Vector2.Zero)
            {
                puffDirection.Normalize(); // Ensures the vector has a length of 1
            }

            Velocity = puffDirection * InitialSpeed; // Set the initial velocity

            // Check the direction and assign the appropriate sprite
            if (puffDirection.X >= 0)
            {
                projectileSprite = SpriteFactory.Instance.createSprite("projectile_kirby_airpuff_right");
            }
            else if (puffDirection.X < 0)
            {
                projectileSprite = SpriteFactory.Instance.createSprite("projectile_kirby_airpuff_left");
            }
        }

        public void Update()
        {
            if (isActive)
            {
                // Decelerate the puff by reducing its velocity
                if (Velocity.Length() > 0)
                {
                    Vector2 deceleration = Vector2.Normalize(Velocity) * DecelerationRate;
                    Velocity -= deceleration;

                    // Make the velocity zero if it becomes negative or close to zero
                    if (Velocity.Length() < 0.01f)
                    {
                        Velocity = Vector2.Zero;
                    }
                }

                Position += Velocity;

                // Increment frame count and check if puff should disappear
                frameCount++;
                if (frameCount >= MaxFrames || Velocity == Vector2.Zero)
                {
                    isActive = false;
                    projectileSprite = null; // Remove the sprite to avoid memory leaks
                }

                // Update the puff's sprite animation only if it's still active
                if (isActive)
                {
                    projectileSprite?.Update();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw if the puff is active and the sprite exists
            if (isActive && projectileSprite != null)
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
        }
    }
}
