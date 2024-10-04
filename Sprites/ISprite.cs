using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Sprites
{
    public interface ISprite
    {
        public void Update();
        public void LevelDraw(Vector2 position, SpriteBatch spriteBatch);
        public void ScreenDraw(Vector2 position, SpriteBatch spriteBatch, Color color);
        public void ResetAnimation();
    }
}