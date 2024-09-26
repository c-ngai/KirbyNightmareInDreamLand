using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class KirbyBeamSegment : IProjectile
    {
        private Vector2 position;
        private Vector2 velocity;
        private int frameCount = 0;
        private int maxFrames = 6; // Segment disappears after 6 frames
        private bool IsActive = true;
        private ISprite sprite1;
        private ISprite sprite2;


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

        public KirbyBeamSegment(Vector2 startPosition, Vector2 beamVelocity)
        {
            Position = startPosition;
            Velocity = beamVelocity;
            this.sprite1 = SpriteFactory.Instance.createSprite("projectile_kirby_beam1");
            this.sprite2 = SpriteFactory.Instance.createSprite("projectile_kirby_beam2");

        }

        public void Update()
        {
            if (IsActive)
            {
                // Update position based on velocity
                Position += Velocity;

            }

            // Update animation Chandled internally by sprite)
            if (frameCount % 2 == 0)
            {
                sprite2.Update();

            }
            else
            {
                sprite1.Update();
            }

            // Increment frame count and check if the segment should disappear
            frameCount++;
            if (frameCount >= maxFrames)
            {
                IsActive = false; // Mark the segment as inactive

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (frameCount % 2 == 0 && IsActive)
            {
                sprite2.Draw(Position, spriteBatch);

            }
            else if (IsActive)
            {
                sprite1.Draw(Position, spriteBatch);
            }
        }
    }
}
