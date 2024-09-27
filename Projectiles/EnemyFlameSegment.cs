using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MasterGame
{
    public class EnemyFlameSegment : IProjectile
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        private float speed;
        private float delay; // Delay before this segment becomes active
        private bool isActive; 
        private static Random random = new Random(); // Random instance for sprite selection
        private int frameCount;

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

        public EnemyFlameSegment(Vector2 startPosition, Vector2 flameDirection, float speed, float delay)
        {
            Position = startPosition;
            this.speed = speed;
            this.delay = delay;
            isActive = false;

            if (flameDirection != Vector2.Zero)
            {
                flameDirection.Normalize();
            }

            Velocity = flameDirection * speed;

            // Randomly select either the first or second set of sprites based on direction
            bool useSecondSprite = random.Next(2) == 0; // 50% chance to use the second sprite

            projectileSprite = SpriteFactory.Instance.createSprite("projectile_hothead_fire");

        }

        public void Update()
        {
            // Reduce delay over time
            if (delay > 0)
            {
                delay -= SecondsPerFrame;
            }
            else
            {
                isActive = true;
            }

            // Only update position if the flame segment is active
            if (isActive)
            {
                Position += Velocity; 

                frameCount++;

                // Mark the segment as inactive after a certain number of frames
                if (frameCount >= MaxFrames)
                {
                    isActive = false;
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
            if (isActive)
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
        }

    }
}
