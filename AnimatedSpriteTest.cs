using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace MasterGame
{
    public class AnimatedSpriteTest : SpriteTemplate
    {
        // Texture atlas
        public Texture2D texture { get; set; }
        // Animation object
        public Sprite sprite { get; set; }
        public int x { get; set; }
        public int y { get; set; }


        public AnimatedSpriteTest(Texture2D texture, Sprite animation, Vector2 location)
        {
            this.texture = texture;
            this.sprite = animation;
            y = (int)location.Y;
        }

        public void Update()
        {
            sprite.Update();

            // sets the horizontal movement to wrap around the screen
            if (x < Game1.self.windowWidth)
            {
                x += 1;
            }
            else if (x >= Game1.self.windowWidth)
            {
                x = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 location)
        {

            Rectangle sourceRectangle = sprite.getSourceRectangle();
            Rectangle destinationRectangle = new Rectangle(x, (int)location.Y, sprite.getWidth(), sprite.getHeight());

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
