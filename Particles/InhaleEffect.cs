using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Particles
{
    public class InhaleEffect : IParticle
    {
        private IPlayer player;
        private Sprite sprite;
        private Vector2 position;
        private int frameCount = 0;
        private bool completed;

        public InhaleEffect(IPlayer player)
        {
            this.player = player;
            // assign the appropriate sprite based on direction
            sprite = player.IsLeft()
                ? SpriteFactory.Instance.CreateSprite("particle_inhale_left")
                : SpriteFactory.Instance.CreateSprite("particle_inhale_right");

            ObjectManager.Instance.AddParticle(this);
            completed = false;
        }

        public void Update()
        {
            sprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            position = player.GetKirbyPosition(); // this is bad to be in draw but whatever
            sprite.Draw(position, spriteBatch);
        }

        public bool IsDone()
        {
            return completed;
        }

        public void End()
        {
            completed = true;
        }

    }
}
