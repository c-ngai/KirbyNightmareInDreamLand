using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class EnemyFireball : IProjectile
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        private const float Speed = 2f; // Speed of the fireball

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

        public EnemyFireball(Vector2 startPosition)
        {
            Position = startPosition;
            Velocity = new Vector2(Speed, 0); // Moving to the right (if needs to be moving to left, make speed negative)
            projectileSprite = SpriteFactory.Instance.createSprite("projectile_hothead_fireball");
        }

        public void Update()
        {
            // Update position based on velocity
            Position += Velocity;

            projectileSprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            projectileSprite.Draw(Position, spriteBatch);
        }
    }
}
