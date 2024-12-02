using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Particles
{
    public class Stardust : IParticle
    {
        private Sprite sprite;
        private Vector2 position;
        private int frameCount = 0;
        private bool completed;

        public Stardust(Vector2 position)
        {
            this.position = position;
            sprite = SpriteFactory.Instance.CreateSprite("particle_stardust");
            ObjectManager.Instance.AddParticle(this);
            completed = false;
        }

        public void Update()
        {
            if (frameCount < Constants.Particle.STARDUST_MAX_FRAMES)
            {
                sprite.Update();
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
