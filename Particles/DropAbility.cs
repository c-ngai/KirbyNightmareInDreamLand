using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Particles
{
    public class DropAbility : IParticle
    {
        private Sprite sprite;
        private Vector2 position;
        private int frameCount = 0;
        private bool completed;

        public DropAbility(Vector2 position)
        {
            this.position = position + new Vector2(0, -8);
            sprite = SpriteFactory.Instance.CreateSprite("particle_dropability");
            ObjectManager.Instance.AddParticle(this);
            completed = false;
        }

        public void Update()
        {
            if (frameCount < Constants.Particle.DROPABILITY_MAX_FRAMES)
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
