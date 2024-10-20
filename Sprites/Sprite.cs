﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Sprites
{

    public class Sprite : ISprite
    {
        // To store a reference to the current game.
        private Game1 _game;
        private Camera _camera;

        // The SpriteAnimation used.
        private SpriteAnimation _spriteAnimation;

        // The current frame of the animation.
        private int currentFrame;
        // The current number of game ticks since last frame advance.
        private int tickCounter;
        private int counter = 0;

        /* Creates a new animation object from an animation file. Imports animation
         * data from a .csv file into the Animation object. */
        public Sprite(SpriteAnimation spriteAnimation)
        {
            _spriteAnimation = spriteAnimation;

            currentFrame = 0;
            tickCounter = -1;
            _game = Game1.Instance;
            _camera = _game.Camera;
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
        public void Draw(Vector2 position, SpriteBatch spriteBatch, Color color)
        {
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

            // Cull the sprite if it is not within the camera rectangle and if culling is enabled.
            bool cull = false;
            if (_game.CULLING_ENABLED && !_camera.GetBounds().Intersects(destinationRectangle))
            {
                cull = true;
            }

            // Draw the sprite if it has not been culled.
            if (!cull)
            {
                // Draw the sprite to the spriteBatch.
                spriteBatch.Draw(texture, position, sourceRectangle, color, 0, frameCenter, 1, _spriteAnimation.SpriteEffects, 0);
                // DEBUG VISUALS, TIDY UP LATER
                if (_game.DEBUG_SPRITE_MODE == true)
                {
                    GameDebug.Instance.DrawRectangle(spriteBatch, destinationRectangle, Color.Blue);
                    GameDebug.Instance.DrawPoint(spriteBatch, position, Color.Red);

                    // Draws purple borders around all tiles intersecting with the sprite boundaries
                    /*
                    List<Tile> tiles = _game.level.IntersectingTiles(spriteBatch, destinationRectangle);
                    foreach (Tile tile in tiles)
                    {
                        Debug.Instance.DrawRectangle(spriteBatch, tile.rectangle, Color.Purple);
                    }
                    */
                }
            }

        }

        // Draws the sprite to the spriteBatch. With unspecified color mask, uses white (no change to source image).
        public void Draw(Vector2 position, SpriteBatch spriteBatch)
        {
            Draw(position, spriteBatch, Color.White);
        }


        public void DamageDraw(Vector2 position, SpriteBatch spriteBatch)
        {
            counter ++;
            if (counter < 10)
            {
                Draw(position, spriteBatch);
            } else{
                if(counter == 20)
                    counter = 0;
            }
        }

        // Resets the animation to the start. Should be desirable to call any time an entity's sprite is switched.
        public void ResetAnimation()
        {
            currentFrame = 0;
            tickCounter = -1;
        }
    }
}
