using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.Sprites;

namespace KirbyNightmareInDreamLand.Projectiles
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

            projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_hothead_fire");

        }

        public void Update()
        {
            // Reduce delay over time
            if (delay > 0)
            {
                delay -= Constants.EnemyFire.SECONDS_PER_FRAME;
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
                if (frameCount >= Constants.EnemyFire.MAX_FRAMES)
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
                projectileSprite.LevelDraw(Position, spriteBatch);
            }
        }

    }
}
