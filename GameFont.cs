using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MasterGame
{
    public class GameFont
    {
        public GameFont()
        {
            
        }

        public void InitializeDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
        }

        public void EndDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
        }

        public void ControlDraw(SpriteBatch spriteBatch, SpriteFont font)
        {
            InitializeDraw(spriteBatch);
            Draw(spriteBatch, font);
            EndDraw(spriteBatch);
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            string text = "Credits\nProgram Made By: Carman Ngai\nSprites from: \n http://rbwhitaker.wikidot.com/monogame-texture-atlases-1 \n http://rbwhitaker.wikidot.com/monogame-drawing-text-with-spritefonts";
            spriteBatch.DrawString(font, text, new Vector2(100, 300), Color.Black);
        }

    }
}
