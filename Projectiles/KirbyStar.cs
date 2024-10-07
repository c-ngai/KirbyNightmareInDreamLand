using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyStar : IProjectile
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;

        public Vector2 Position
        {
            get => position;            // Return position of star
            set => position = value;    // Set the position of the star to the given value
        }

        public Vector2 Velocity
        {
            get => velocity;            // Return the current velocity of the star
            set => velocity = value;    // Set the velocity of the star to the given value
        }

        public KirbyStar(Vector2 kirbyPosition, bool isFacingRight)
        {
            Position = kirbyPosition;

            // Set the initial velocity based on the direction Kirby is facing
            Velocity = isFacingRight
                ? new Vector2(Constants.Star.SPEED, 0)
                : new Vector2(-Constants.Star.SPEED, 0);

            // Assign the appropriate sprite based on the direction
            projectileSprite = isFacingRight
                ? SpriteFactory.Instance.CreateSprite("projectile_kirby_star_right")
                : SpriteFactory.Instance.CreateSprite("projectile_kirby_star_left");
        }

        public void Update()
        {
            Position += Velocity;
            projectileSprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            projectileSprite.LevelDraw(Position, spriteBatch);
        }
    }
}
