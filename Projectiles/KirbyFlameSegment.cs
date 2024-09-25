using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class KirbyFlameSegment : IProjectile
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        private float speed; // Speed of the flame segment
        private float delay; // Delay before this segment becomes active
        private bool isActive; // Whether the flame segment has started moving
        private static Random random = new Random(); // Random instance for sprite selection

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public KirbyFlameSegment(Vector2 startPosition, Vector2 flameDirection, float speed, float delay)
        {
            Position = startPosition;
            this.speed = speed; // Set speed from parameter
            this.delay = delay; // Set delay from parameter
            isActive = false; // Start as inactive

            // Normalize the direction vector and multiply by the speed
            if (flameDirection != Vector2.Zero)
            {
                flameDirection.Normalize();
            }

            Velocity = flameDirection * speed; // Apply the variable speed

            // Randomly select either the first or second set of sprites based on direction
            bool useSecondSprite = random.Next(2) == 0; // 50% chance to use the second sprite

            if (flameDirection.X >= 0)
            {
                // Right-facing direction
                if (useSecondSprite)
                {
                    projectileSprite = SpriteFactory.Instance.createSprite("projectile_kirby_fire2_right");
                }
                else
                {
                    projectileSprite = SpriteFactory.Instance.createSprite("projectile_kirby_fire1_right");
                }
            }
            else
            {
                // Left-facing direction
                if (useSecondSprite)
                {
                    projectileSprite = SpriteFactory.Instance.createSprite("projectile_kirby_fire2_left");
                }
                else
                {
                    projectileSprite = SpriteFactory.Instance.createSprite("projectile_kirby_fire1_left");
                }
            }
        }

        public void Update()
        {
            // Reduce delay over time
            if (delay > 0)
            {
                delay -= 0.016f; // TODO: Check. If 60fps, 1/60 = ~0.016 seconds per frame
            }
            else
            {
                isActive = true;
            }

            // Only update position if the flame segment is active
            if (isActive)
            {
                Position += Velocity; // Update position based on velocity
                projectileSprite.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive) // Only draw when active
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
        }
    }
}
