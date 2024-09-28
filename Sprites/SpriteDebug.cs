using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MasterGame
{

    public class SpriteDebug
    {

        private Color translucent = new(255, 255, 255, 63);
        private Texture2D red;
        private Texture2D blue;



        private static SpriteDebug instance = new();

        public static SpriteDebug Instance
        {
            get
            {
                return instance;
            }
        }

        public SpriteDebug()
        {
        }


        // Create textures for drawing boxes.
        public void Load(GraphicsDevice graphicsDevice)
        {
            // TEMPORARY, FOR DEBUG SPRITE VISUALS
            red = new Texture2D(graphicsDevice, 1, 1);
            red.SetData(new Color[] { Color.Red });
            
            blue = new Texture2D(graphicsDevice, 1, 1);
            blue.SetData(new Color[] { Color.Blue });
        }



        // Draws a rectangle around the sprite bounds and a point at its center. Pretty messy, should probably tidy up, but also it's debug, so not a priority
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Vector2 frameCenter, Rectangle sourceRectangle, float scale)
        {

            // Draw box around sprite
            // top side
            spriteBatch.Draw(blue, new Rectangle((int)(position.X - frameCenter.X * scale), (int)(position.Y - frameCenter.Y * scale), (int)(sourceRectangle.Width * scale), (int)scale), translucent);
            // bottom side
            spriteBatch.Draw(blue, new Rectangle((int)(position.X - frameCenter.X * scale), (int)(position.Y + (sourceRectangle.Height - frameCenter.Y - 1) * scale), (int)(sourceRectangle.Width * scale), (int)scale), translucent);
            // left side
            spriteBatch.Draw(blue, new Rectangle((int)(position.X - frameCenter.X * scale), (int)(position.Y - (frameCenter.Y - 1) * scale), (int)scale, (int)((sourceRectangle.Height - 2) * scale)), translucent);
            // right side
            spriteBatch.Draw(blue, new Rectangle((int)(position.X + (sourceRectangle.Width - frameCenter.X - 1) * scale), (int)(position.Y - (frameCenter.Y - 1) * scale), (int)scale, (int)((sourceRectangle.Height - 2) * scale)), translucent);

            // Draw dot at center of sprite
            spriteBatch.Draw(red, new Rectangle((int)(position.X - scale), (int)(position.Y - scale), (int)(scale * 2), (int)(scale * 2)), translucent);
            
        }

    }
}
