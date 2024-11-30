using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Particles
{
    public class DashCloud : IParticle
    {
        private IPlayer player;
        private Sprite sprite;
        private Vector2 position;
        private int frameCount = 0;
        private bool completed;

        public DashCloud(IPlayer player)
        {
            this.player = player;
            // assign the appropriate sprite based on direction
            sprite = player.IsLeft()
                ? SpriteFactory.Instance.CreateSprite("particle_dash_left")
                : SpriteFactory.Instance.CreateSprite("particle_dash_right");

            position = player.GetKirbyPosition();
            ObjectManager.Instance.AddParticle(this);
            completed = false;
        }

        public void Update()
        {
            if (frameCount < Constants.Particle.DASH_CLOUD_FRAMES)
            {
                sprite.Update();
                frameCount++;
            }
            else
            {
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
