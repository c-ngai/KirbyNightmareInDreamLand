using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KirbyNightmareInDreamLand.Sprites
{

    public class Sprite : ISprite
    {
        // To store a reference to the current game.
        private Game1 _game;
        private Camera _camera;
        // The game's INTERNAL resolution. For placement on screen relative to the scale. Not to be confused with window resolution.
        // NOTE: Currently does not need game width but will for future use
        //private int gameWidth;
        private int gameHeight;

        // The SpriteAnimation used.
        private SpriteAnimation _spriteAnimation;

        // The current frame of the animation.
        private int currentFrame;
        // The current number of game ticks since last frame advance.
        private int tickCounter;
        private int counter = 0;

        /* Creates a new animation object from an animation file. Imports animation
         * data from a .csv file into the Animation object. */
        public Sprite(SpriteAnimation spriteAnimation, Game1 game)
        {
            // NOTE: Currently does not need game width but will for future use
            //gameWidth = Constants.Graphics.GAME_WIDTH;
            gameHeight = Constants.Graphics.GAME_HEIGHT;

            _spriteAnimation = spriteAnimation;

            currentFrame = 0;
            tickCounter = 0;
            _game = game;
            _camera = game.camera;
        }



        // Updates the animation for the game tick.
        public void Update()
        {
            // Advance the tick counter. If it's reached the frame time of the current frame, advance the frame and reset the tick counter.
            tickCounter++;
            if (tickCounter == _spriteAnimation.FrameTimes[currentFrame])
            {
                tickCounter = 0;
                currentFrame++;
                // If the current frame has hit the end, cycle back to the loop point.
                if (currentFrame == _spriteAnimation.FrameCount)
                {
                    currentFrame = _spriteAnimation.LoopPoint;
                }
            }
        }

        // Draws the sprite to the spriteBatch.
        public void ScreenDraw(Vector2 position, SpriteBatch spriteBatch, Color color)
        {
            // Scale by height
            float scale = (float)_game.WINDOW_HEIGHT / gameHeight;

            // Scale the position
            position.Floor();
            position *= scale;
            // Adjust position by window offset
            position.X += _game.WINDOW_XOFFSET;
            position.Y += _game.WINDOW_YOFFSET;
            // Pull the texture, frame center, and source rectangle from data.
            Texture2D texture = _spriteAnimation.Texture;
            Vector2 frameCenter = _spriteAnimation.FrameCenters[currentFrame];
            Rectangle sourceRectangle = _spriteAnimation.FrameSourceRectangles[currentFrame];

            if (_spriteAnimation.SpriteEffects.Equals(SpriteEffects.FlipHorizontally))
            {
                frameCenter.X = sourceRectangle.Width - frameCenter.X;
            }

            // Draw the sprite to the spriteBatch.
            spriteBatch.Draw(texture, position, sourceRectangle, color, 0, frameCenter, scale, _spriteAnimation.SpriteEffects, 0);


            // DEBUG VISUALS, TIDY UP LATER
            if (_game.DEBUG_SPRITE_MODE == true)
            {
                SpriteDebug.Instance.Draw(spriteBatch, position, frameCenter, sourceRectangle, scale);
            }
            
        }

        // Draws the sprite to the spriteBatch. With unspecified color mask, uses white (no change to source image).
        public void ScreenDraw(Vector2 position, SpriteBatch spriteBatch)
        {
            ScreenDraw(position, spriteBatch, Color.White);
        }

        // Draws the sprite to the spriteBatch. With unspecified color mask, uses white (no change to source image).
        public void LevelDraw(Vector2 position, SpriteBatch spriteBatch)
        {
            position -= _camera.GetPosition();
            ScreenDraw(position, spriteBatch, Color.White);
        }

        public void DamageDraw(Vector2 position, SpriteBatch spriteBatch)
        {
            counter ++;
            if (counter < 10)
            {
                LevelDraw(position, spriteBatch);
            } else{
                if(counter == 20)
                    counter = 0;
            }
        }

        // Resets the animation to the start. Should be desirable to call any time an entity's sprite is switched.
        public void ResetAnimation()
        {
            currentFrame = 0;
            tickCounter = 0;
        }
    }
}
