﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace MasterGame
{
    public class AnimatedMovingHorizontallySprite : Sprite
    {
        // Texture atlas
        public Texture2D texture { get; set; }
        // Number of rows in the texture atlas
        public int rows { get; set; }
        // Number of columns in the texture atlas
        public int columns { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        private int currentFrame;
        private int totalFrames;


        public AnimatedMovingHorizontallySprite(Texture2D texture, int rows, int columns, Vector2 location)
        {
            this.texture = texture;
            this.rows = rows;
            this.columns = columns;
            currentFrame = 0;
            totalFrames = this.rows * this.columns;
            y = (int)location.Y;
        }

        public void Update()
        {
            // sets up frames for animation
            currentFrame++;
            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
            }

            // sets the horizontal movement to wrap around the screen
            if (x < Game1.self.windowWidth)
            {
                x += 5;
            }
            else if (x >= Game1.self.windowWidth)
            {
                x = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = texture.Width / columns;
            int height = texture.Height / rows;
            int row = currentFrame / columns;
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle(x, (int)location.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
