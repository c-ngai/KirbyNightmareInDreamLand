using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;

namespace KirbyNightmareInDreamLand.Particles
{
    public class Star : IParticle
    {
        private Sprite sprite;
        private Vector2 position;
        private int frameCount = 0;
        private Random randomGenerator;
        private Vector2[] SpawnPoints = new[] { Constants.Particle.STAR_OFFSET_TOPLEFT, Constants.Particle.STAR_OFFSET_TOP, Constants.Particle.STAR_OFFSET_TOPRIGHT,
            Constants.Particle.STAR_OFFSET_LEFT, Constants.Particle.STAR_OFFSET_RIGHT, Constants.Particle.STAR_OFFSET_BOTTOMLEFT, Constants.Particle.STAR_OFFSET_BOTTOM,
            Constants.Particle.STAR_OFFSET_BOTTOMRIGHT};
        private Vector2 spawnPoint;
        private bool completed;

        public Star(Vector2 kirbyPosition)
        {
            randomGenerator = new Random();
            spawnPoint = SpawnPoints[randomGenerator.Next(0, 8)];

            position = kirbyPosition + spawnPoint;
            sprite = SpriteFactory.Instance.CreateSprite("projectile_kirby_airpuff_right");
            completed = false;
        }

        public void Update()
        {
            if (frameCount < Constants.Puff.MAX_FRAMES)
            {
                sprite.Update();
                position += spawnPoint;
                frameCount++;
            }
            else
            {
                completed = true;
                sprite = null;
            }
        }

        public bool isDone()
        {
            return completed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!completed && sprite != null)
            {
                sprite.Draw(position, spriteBatch);
            }
        }
    }
}
