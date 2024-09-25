using Microsoft.Xna.Framework;

namespace MasterGame
{
    public class EnemyBeam : IProjectile
    {
        private Vector2 position;
        private Vector2 velocity;
        private Sprite projectileSprite; // TODO: change to projectileSprite

        public EnemyBeam(Vector2 startPosition, Vector2 direction)
        {
            position = startPosition;
            velocity = direction;
            projectileSprite = SpriteFactory.Instance.createSprite("waddledee_walking_right"); // TODO: delete and use line below instead
            // fireballSprite = SpriteFactory.Instance.createSprite("enemy_fireball");
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public void Update()
        {
            // Move the fireball based on its velocity
            position += velocity;

            // Update the fireball sprite animation
            enemySprite.Update(); // TODO: change to sprite for fireball

        }

        public void Draw()
        {
            enemySprite.Draw(position); // TODO: change to sprite for fireball
        }
    }
}
