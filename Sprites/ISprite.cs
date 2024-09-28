using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MasterGame.Sprites
{
    public interface ISprite
    {
        public void Update();
        public void Draw(Vector2 position, SpriteBatch spriteBatch);
        public void Draw(Vector2 position, SpriteBatch spriteBatch, Color color);
        public void ResetAnimation();
    }
}