using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Sprint0
{
    public class AnimatedSpriteTest : Sprite
    {
        // Texture atlas
        public Texture2D texture { get; set; }
        // Animation object
        public Animation animation { get; set; }
        public int x { get; set; }
        public int y { get; set; }


        public AnimatedSpriteTest(Texture2D texture, Animation animation, Vector2 location)
        {
            this.texture = texture;
            this.animation = animation;
            y = (int)location.Y;
        }

        public void Update()
        {
            animation.Update();

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

            Rectangle sourceRectangle = animation.getSourceRectangle();
            Rectangle destinationRectangle = new Rectangle(x, (int)location.Y, animation.getWidth(), animation.getHeight());

            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
