using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MasterGame
{
    public interface ISprite
    {
        public void Update();
        public void Draw(Vector2 position);
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
            spriteBatch = Game1.self.spriteBatch;
            gameWidth = Game1.self.gameWidth;
            gameHeight = Game1.self.gameHeight;

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



        // Draws the sprite to the spriteBatch. Color parameter for multiplying the image. (color multiplication)
        public void Draw(Vector2 position, Color color)
        {
            // Get window width and height from Game1 for scaling.
            int windowWidth = Game1.self.windowWidth;
            int windowHeight = Game1.self.windowHeight;
            // Scale by height
            float scale = windowHeight / gameHeight;

            // Scale the position
            position.Floor();
            position *= scale;
            // Pull the frame center and source rectangle from data.
            Vector2 frameCenter = _spriteAnimation.frameCenters[currentFrame];
            Rectangle sourceRectangle = _spriteAnimation.frameSourceRectangles[currentFrame];

            // Draw the sprite to the spriteBatch.
            spriteBatch.Draw(_spriteAnimation.texture, position, sourceRectangle, color, 0, frameCenter, scale, _spriteAnimation.spriteEffects, 0);
        }

        

        // Draws the sprite to the spriteBatch. With unspecified color, uses white.
        public void Draw(Vector2 position)
        {
            Draw(position, Color.White);
        }



        // Resets the animation to the start. Should be desirable to call any time an entity's sprite is switched.
        public void ResetAnimation()
        {
            currentFrame = 0;
            tickCounter = 0;
        }



    }
}
