using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;

namespace KirbyNightmareInDreamLand.Particles
{
    public class Cloud : IParticle
    {
        private Sprite sprite;
        private Vector2 position;
        private int frameCount = 0;
        private Vector2 offset;
        private bool completed;

        public Cloud(Vector2 kirbyPosition, bool isLeft)
        {
            // assign the appropriate sprite based on direction
            sprite = isLeft
                ? SpriteFactory.Instance.CreateSprite("projectile_kirby_airpuff_right")
                : SpriteFactory.Instance.CreateSprite("projectile_kirby_airpuff_left");

            // assign appropriate animation location and direction 
            offset = isLeft
                ? Constants.Particle.CLOUD_OFFSET_RIGHT
                : Constants.Particle.CLOUD_OFFSET_LEFT;

            position = kirbyPosition + offset;
            ObjectManager.Instance.AddParticle(this);
            completed = false;
        }

        // TODO: change it to cloud max frames when that is determined
        public void Update()
        {
            if (frameCount < Constants.Particle.CLOUD_MAX_FRAMES)
            {
                sprite.Update();
                position += offset;
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
