using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Particles
{
    public interface IParticle
    {
        void Update();
        void Draw(SpriteBatch spriteBatch);

        bool IsDone();
    }
}
