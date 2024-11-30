using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Particles
{
    public class CollisionStar : IParticle
    {
        private Sprite sprite;
        private Vector2 position;
        private int randomIndex;
        private int frameCount = 0;
        private Random randomGenerator;
        private bool completed;

        public CollisionStar(Vector2 kirbyPosition)
        {
            randomGenerator = new Random();

            // Randomly generates the star at one of the 8 possible offsets 
            randomIndex = randomGenerator.Next(Constants.Particle.OFFSET1, Constants.Particle.OFFSET8);

            position = kirbyPosition + Constants.Particle.startingLocations[randomIndex];
            sprite = SpriteFactory.Instance.CreateSprite("particle_bumpstar");
            ObjectManager.Instance.AddParticle(this);
            completed = false;
        }

        public void Update()
        {
            if (frameCount < Constants.Particle.STAR_MAX_FRAMES)
            {
                sprite.Update();
                position += Constants.Particle.offsets[randomIndex];
                //Debug.WriteLine(Constants.Particle.offsets[randomIndex]);
                frameCount++;
            }
            else
            {
                sprite = null;
                completed = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(position, spriteBatch);
        }

        public bool IsDone()
        {
            return completed;
        }
    }
}
