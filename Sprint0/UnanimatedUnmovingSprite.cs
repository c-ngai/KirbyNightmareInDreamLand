using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Sprint0
{
    public class UnanimatedUnmovingSprite : Sprite
    {
        public Texture2D Texture { get; set; }

        private int width;

        private int height;

        public UnanimatedUnmovingSprite(Texture2D texture)
        {
            Texture = texture;
            width = 200;
            height = 200;
        }

        public override void Update(SpriteBatch spriteBatch, Vector2 location)
        {
            InitializeDraw(spriteBatch);
            Draw(spriteBatch, location);
            EndDraw(spriteBatch);
        }

        public void Draw(SpriteBatch spritebatch, Vector2 location)
        {
            Rectangle position = new Rectangle((int)location.X, (int)location.Y, width, height);
            spritebatch.Draw(Texture, position, Color.White);
        }
    }
}
