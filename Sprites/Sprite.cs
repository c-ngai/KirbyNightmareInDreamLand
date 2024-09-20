using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Collections.Generic;

namespace MasterGame
{
    public interface ISprite
    {
        public void Update();
        public void Draw(Vector2 position);
    }

    public class Sprite : ISprite
    {

        // To store a reference to the global spritebatch.
        private SpriteBatch spriteBatch;
        private int gameWidth;
        private int gameHeight;

        // The spritesheet used.
        private Texture2D texture;
        // The frame number to loop back to after reaching the end.
        private int loopPoint;
        // The sprite effects. Simply holds a flag of whether or not to horizontally flip the texture.
        private SpriteEffects spriteEffects;
        // List of frame source rectangles. Composed of positions imported from animation spreadsheet file.
        private List<Rectangle> frameSourceRectangles;
        // List of frame centers, as Vectors.
        private List<Vector2> frameCenters;
        // List of frame durations, in game cycles.
        private List<int> frameTimes;
        // The number of frames in the animation.
        private int frameCount;

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

            texture = spriteAnimation.texture;
            loopPoint = spriteAnimation.loopPoint;
            spriteEffects = spriteAnimation.spriteEffects;

            frameCount = spriteAnimation.frameCount;
            frameSourceRectangles = spriteAnimation.frameSourceRectangles;
            frameCenters = spriteAnimation.frameCenters;
            frameTimes = spriteAnimation.frameTimes;

            currentFrame = 0;
            tickCounter = 0;
        }



        // Updates the animation for the game tick.
        public void Update()
        {
            // Advance the tick counter. If it's reached the frame time of the current frame, advance the frame and reset the tick counter.
            tickCounter++;
            if (tickCounter == frameTimes[currentFrame])
            {
                tickCounter = 0;
                currentFrame++;
                // If the current frame has hit the end, cycle back to the loop point.
                if (currentFrame == frameCount)
                {
                    currentFrame = loopPoint;
                }
            }
        }



        // Draws the sprite to the spriteBatch.
        public void Draw(Vector2 position)
        {
            // Get window width and height from Game1 for scaling.
            int windowWidth = Game1.self.windowWidth;
            int windowHeight = Game1.self.windowHeight;
            // Scale by height
            float scale = windowHeight / gameHeight;

            // Scale the position
            position *= scale;
            // Pull the frame center and source rectangle from data.
            Vector2 frameCenter = frameCenters[currentFrame];
            Rectangle sourceRectangle = frameSourceRectangles[currentFrame];

            // Draw the sprite to the spriteBatch.
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0, frameCenter, scale, spriteEffects, 0);
        }



        // Resets the animation to the start. Should be desirable to call any time an entity's sprite is switched.
        public void ResetAnimation()
        {
            currentFrame = 0;
            tickCounter = 0;
        }



    }
}
