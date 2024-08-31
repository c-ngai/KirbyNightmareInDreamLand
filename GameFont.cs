using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprint0
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
            string text = "Credits\nProgram Made By: Carman Ngai\nSprites from: https://www.spriters-resource.com/snes/kirbysuperstarkirbysfunpak/";
            spriteBatch.DrawString(font, text, new Microsoft.Xna.Framework.Vector2(500, 500), Color.Black);
        }

    }
}
