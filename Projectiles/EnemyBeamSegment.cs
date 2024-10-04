using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyBeamSegment : IProjectile
    {
        private Vector2 position; // acts as pivot point
        private Vector2 velocity;
        private int frameCount = 0;
        private Sprite projectileSprite;
        public bool IsActive = true;

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
            projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_waddledoo_beam");
        }

        public void Update()
        {
            if (IsActive)
            {
                Position += Velocity;

                projectileSprite.Update();

                // Increment frame count and check if the segment should disappear
                frameCount++;
                if (frameCount >= Constants.WaddleDooBeam.MAX_FRAMES)
                {
                    IsActive = false; // Mark the segment as inactive
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                projectileSprite.LevelDraw(Position, spriteBatch);
            }
        }
    }
}
