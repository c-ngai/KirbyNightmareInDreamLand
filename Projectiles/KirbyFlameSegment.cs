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
        private const int MaxFrames = 14; // Number of frames before the flame segment disappears
        private int frameCount; // Track the frames since activation

        public bool IsActive { get; private set; } // Expose IsActive for external checks

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
            IsActive = false; // Start as inactive
            frameCount = 0;

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
                delay -= 0.016f; // Assuming 60fps, 1/60 = ~0.016 seconds per frame
            }
            else
            {
                IsActive = true;
            }

            // Only update position if the flame segment is active
            if (IsActive)
            {
                Position += Velocity; // Update position based on velocity

                frameCount++;

                // Mark the segment as inactive after a certain number of frames
                if (frameCount >= MaxFrames)
                {
                    IsActive = false;
                    projectileSprite = null; // Set sprite to null to avoid further drawing
                }
                else
                {
                    projectileSprite?.Update();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive && projectileSprite != null) // Only draw when active
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
        }
    }
}
