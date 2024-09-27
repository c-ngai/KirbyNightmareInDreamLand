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
        // To store a reference to the global spritebatch and current game.
        private SpriteBatch spriteBatch;
        private Game1 game;
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
        public Sprite(SpriteAnimation spriteAnimation, Game1 game)
        {
            gameWidth = Constants.Graphics.GAME_WIDTH;
            gameHeight = Constants.Graphics.GAME_HEIGHT;

            _spriteAnimation = spriteAnimation;

            currentFrame = 0;
            tickCounter = 0;
            this.game = game;
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
            int windowWidth = game.WINDOW_WIDTH;
            int windowHeight = game.WINDOW_HEIGHT;
            // Scale by height
            float scale = (float)windowHeight / gameHeight;

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
            if (game.DEBUG_SPRITE_MODE == true)
            {
                SpriteDebug.Instance.Draw(spriteBatch, position, frameCenter, sourceRectangle, scale);
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
