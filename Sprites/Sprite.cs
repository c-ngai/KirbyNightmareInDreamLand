using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace KirbyNightmareInDreamLand.Sprites
{

    public class Sprite : ISprite
    {
        // stores a reference to the current game
        private Game1 _game;
        private Camera _camera;

        // the SpriteAnimation used
        private SpriteAnimation _spriteAnimation;

        // current frame of the animation
        private int currentFrame;
        // current number of game ticks since last frame advance
        private int tickCounter;

        public int Width
        {
            get => _spriteAnimation.FrameSourceRectangles[currentFrame].Width;
        }

        public int Height
        {
            get => _spriteAnimation.FrameSourceRectangles[currentFrame].Height;
        }

        /* Creates a new animation object from an animation file. Imports animation
         * data from a .csv file into the Animation object. */
        public Sprite(SpriteAnimation spriteAnimation)
        {
            _spriteAnimation = spriteAnimation;

            currentFrame = 0;
            tickCounter = -1;
            _game = Game1.Instance;
        }

        // updates the animation for the game tick.
        public void Update()
        {
            // advance the tick counter
            // if it's reached the frame time of the current frame, advance the frame and reset the tick counter
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

        // draws the sprite to the spriteBatch.
        public void Draw(Vector2 position, SpriteBatch spriteBatch, Color color, float layerDepth)
        {
            _camera = _game.cameras[_game.CurrentCamera];
            // Floor the position
            position.Floor();

            // Pull the texture, frame center, and source rectangle from data.
            Texture2D texture = _spriteAnimation.Texture;
            Vector2 frameCenter = _spriteAnimation.FrameCenters[currentFrame];
            Rectangle sourceRectangle = _spriteAnimation.FrameSourceRectangles[currentFrame];

            // Mirror the frame center's X position within the source rectangle if flipping the sprite horizontally. (Really not sure why it doesn't do this itself...)
            if (_spriteAnimation.SpriteEffects.Equals(SpriteEffects.FlipHorizontally))
            {
                frameCenter.X = sourceRectangle.Width - frameCenter.X;
            }

            Rectangle destinationRectangle = new Rectangle((position - frameCenter).ToPoint(), sourceRectangle.Size);

            // cull the sprite if it is not within the camera rectangle and if culling is enabled.
            bool cull = false;
            if (_game.CULLING_ENABLED && !_camera.GetBounds().Intersects(destinationRectangle))
            {
                cull = true;
            }

            // draw the sprite if it has not been culled
            if (!cull)
            {
                // draw the sprite to the spriteBatch.
                spriteBatch.Draw(texture, position, sourceRectangle, color, 0, frameCenter, 1, _spriteAnimation.SpriteEffects, layerDepth);
                GameDebug.Instance.NumOfSpriteBatchDrawCalls++;

                // DEBUG VISUALS
                if (_game.DEBUG_SPRITE_MODE == true)
                {
                    GameDebug.Instance.DrawRectangle(spriteBatch, destinationRectangle, Color.Blue, Constants.Graphics.BLUE_ALPHA);
                    GameDebug.Instance.DrawPoint(spriteBatch, position, Color.Red, Constants.Graphics.RED_ALPHA);
                }
            }
            GameDebug.Instance.NumOfSpriteDrawCalls++;
        }

        // draws the sprite to the spriteBatch. With unspecified layer depth, uses 0.
        public void Draw(Vector2 position, SpriteBatch spriteBatch, Color color)
        {
            Draw(position, spriteBatch, color, 0);
        }

        //dDraws the sprite to the spriteBatch. With unspecified color mask and layer depth, uses white (no change to source image) and 0.
        public void Draw(Vector2 position, SpriteBatch spriteBatch)
        {
            Draw(position, spriteBatch, Color.White, 0);
        }

        // resets the animation to the start
        // should be desirable to call any time an entity's sprite is switched
        public void ResetAnimation()
        {
            currentFrame = 0;
            tickCounter = -1;
        }
    }
}
