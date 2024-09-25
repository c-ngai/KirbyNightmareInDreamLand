using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class EnemyBeamSegment : IProjectile
    {
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 pivotPosition; // Beam's origin point
        private int frameCount = 0;
        private int maxFrames = 6; // Segment disappears after 6 frames
        private ISprite sprite;

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

        public EnemyBeamSegment(Vector2 startPosition, Vector2 beamVelocity, Vector2 pivotPosition)
        {
            Position = startPosition;
            Velocity = beamVelocity;
            this.pivotPosition = pivotPosition;
            this.sprite = SpriteFactory.Instance.createSprite("projectile_waddledoo_beam");
        }

        public void Update()
        {
            // Update position based on velocity
            Position += Velocity;

            // Update animation (handled internally by sprite)
            sprite.Update();

            // Increment frame count and check if the segment should disappear
            frameCount++;
            if (frameCount > maxFrames)
            {
                // Mark the segment as inactive or remove it from the list in EnemyBeam
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(Position, spriteBatch);
        }
    }
}
