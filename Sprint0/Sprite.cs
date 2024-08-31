using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Sprint0
{
    public abstract class Sprite 
    {
        public void InitializeDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
        }

        public void EndDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }
        public abstract void Update(SpriteBatch spriteBatch, Vector2 location);
    }
}
