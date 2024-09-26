﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class EnemyBeamSegment : IProjectile
    {
        private Vector2 position; // acts as pivot point
        private Vector2 velocity;
        private int frameCount = 0;
        private int maxFrames = 6; // Segment disappears after 6 frames
        private Sprite projectileSprite;
        public bool IsActive = true; // Track whether the segment is active or not

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

        public EnemyBeamSegment(Vector2 startPosition, Vector2 beamVelocity)
        {
            Position = startPosition;
            Velocity = beamVelocity;
            projectileSprite = SpriteFactory.Instance.createSprite("projectile_waddledoo_beam");
        }

        public void Update()
        {
            if (IsActive)
            {
                // Update position based on velocity
                Position += Velocity;

                // Update animation
                projectileSprite.Update();

                // Increment frame count and check if the segment should disappear
                frameCount++;
                if (frameCount >= maxFrames)
                {
                    IsActive = false; // Mark the segment as inactive
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
    }
}
