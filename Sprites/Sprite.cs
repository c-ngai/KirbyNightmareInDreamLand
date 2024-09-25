using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MasterGame
{
    public interface ISprite
    {
        public void Update();
        public void Draw(Vector2 position, SpriteBatch spriteBatch);
        public void ResetAnimation();
    }

    public class Sprite : ISprite
    {
        // To store a reference to the global spritebatch.
        private SpriteBatch spriteBatch;
        // The game's INTERNAL resolution. For placement on screen relative to the scale. Not to be confused with window resolution.
        private int gameWidth;
        private int gameHeight;

        // The SpriteAnimation used.
        private SpriteAnimation _spriteAnimation;

        // The current frame of the animation.
        private int currentFrame;
        // The current number of game ticks since last frame advance.
        private int tickCounter;

        /* Creates a new animation object from an animation file. Imports animation
         * data from a .csv file into the Animation object. */
        public Sprite(SpriteAnimation spriteAnimation)
        {
            gameWidth = Constants.Graphics.GAME_WIDTH;
            gameHeight = Constants.Graphics.GAME_HEIGHT;

            _spriteAnimation = spriteAnimation;

            currentFrame = 0;
            tickCounter = 0;
        }



        // Updates the animation for the game tick.
        public void Update()
        {
            // Advance the tick counter. If it's reached the frame time of the current frame, advance the frame and reset the tick counter.
            tickCounter++;
            if (tickCounter == _spriteAnimation.frameTimes[currentFrame])
            {
                tickCounter = 0;
                currentFrame++;
                // If the current frame has hit the end, cycle back to the loop point.
                if (currentFrame == _spriteAnimation.frameCount)
                {
                    currentFrame = _spriteAnimation.loopPoint;
                }
            }
        }

        // Draws the sprite to the spriteBatch.
        public void Draw(Vector2 position, SpriteBatch spriteBatch, Color color)
        {
            // Get window width and height from Game1 for scaling.
            int windowWidth = Constants.Graphics.WINDOW_WIDTH;
            int windowHeight = Constants.Graphics.WINDOW_HEIGHT;
            // Scale by height
            float scale = windowHeight / gameHeight;

            // Scale the position
            position.Floor();
            position *= scale;
            // Pull the frame center and source rectangle from data.
            Vector2 frameCenter = _spriteAnimation.frameCenters[currentFrame];
            Rectangle sourceRectangle = _spriteAnimation.frameSourceRectangles[currentFrame];

            if (_spriteAnimation.spriteEffects.Equals(SpriteEffects.FlipHorizontally))
            {
                frameCenter.X = sourceRectangle.Width - frameCenter.X;
            }

            // Draw the sprite to the spriteBatch.
            spriteBatch.Draw(_spriteAnimation.texture, position, sourceRectangle, color, 0, frameCenter, scale, _spriteAnimation.spriteEffects, 0);


            // DEBUG VISUALS, TIDY UP LATER
            if (Constants.Graphics.DEBUG_SPRITE_MODE == true)
            {
                Texture2D red;
                red = new Texture2D(Game1.self.GraphicsDevice, 1, 1);
                red.SetData(new Color[] { Color.Red });
                Texture2D blue;
                blue = new Texture2D(Game1.self.GraphicsDevice, 1, 1);
                blue.SetData(new Color[] { Color.Blue });

                Color translucent = new Color(255, 255, 255, 63);

                // Draw box around sprite
                // top side
                spriteBatch.Draw(blue, new Rectangle((int)(position.X - frameCenter.X*scale), (int)(position.Y - frameCenter.Y*scale), (int)(sourceRectangle.Width*scale), (int)scale), translucent);
                // bottom side
                spriteBatch.Draw(blue, new Rectangle((int)(position.X - frameCenter.X * scale), (int)(position.Y + (sourceRectangle.Height - frameCenter.Y - 1) * scale), (int)(sourceRectangle.Width * scale), (int)scale), translucent);
                // left side
                spriteBatch.Draw(blue, new Rectangle((int)(position.X - frameCenter.X * scale), (int)(position.Y - frameCenter.Y * scale), (int)scale, (int)(sourceRectangle.Height * scale)), translucent);
                // right side
                spriteBatch.Draw(blue, new Rectangle((int)(position.X + (sourceRectangle.Width - frameCenter.X - 1) * scale), (int)(position.Y - frameCenter.Y * scale), (int)scale, (int)(sourceRectangle.Height * scale)), translucent);

                // Draw dot at center of sprite
                spriteBatch.Draw(red, new Rectangle((int)(position.X - scale), (int)(position.Y - scale), (int)scale * 2, (int)scale * 2), translucent);
            }
            
        }

        // Draws the sprite to the spriteBatch. With unspecified color, uses white.
        public void Draw(Vector2 position, SpriteBatch spriteBatch)
        {
            Draw(position, spriteBatch, Color.White);
        }

        // Resets the animation to the start. Should be desirable to call any time an entity's sprite is switched.
        public void ResetAnimation()
        {
            currentFrame = 0;
            tickCounter = 0;
        }
    }
}
