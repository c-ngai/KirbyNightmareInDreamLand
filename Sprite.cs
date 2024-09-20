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
        public void Draw(SpriteBatch spriteBatch, Vector2 position);
    }

    public class Sprite : ISprite
    {

        // The spritesheet used.
        private Texture2D texture;
        // The frame number to loop back to after reaching the end.
        private int loopPoint;
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
        public Sprite(string SpriteFilepath, Dictionary<string, Texture2D> textures)
        {
            currentFrame = 0;
            tickCounter = 0;
            frameSourceRectangles = new List<Rectangle>();
            frameCenters = new List<Vector2>();
            frameTimes = new List<int>();

            // Read spreadsheet rows into list of strings.
            List<string> rows = new(File.ReadLines(SpriteFilepath));
            // Read spreadsheet into 2D array of strings.
            string[][] spreadsheet = new string[rows.Count][];
            for (int i = 0; i < rows.Count; i++)
            {
                spreadsheet[i] = rows[i].Split(',');
            }
            
            // Find the source texture from spreadsheet header.
            string TextureName = spreadsheet[0][1];
            texture = textures[TextureName];
            // Find loop point from spreadsheet header.
            loopPoint = int.Parse(spreadsheet[1][1]);
            // Set the total frame count.
            frameCount = rows.Count - 3;

            // For each row in the table, separate it into columns by commas
            for (int i = 3; i < rows.Count; i++)
            {
                // Pull data from each column of the current frame's row in the spreadsheet.
                int frameX = int.Parse(spreadsheet[i][0]);
                int frameY = int.Parse(spreadsheet[i][1]);
                int frameWidth = int.Parse(spreadsheet[i][2]);
                int frameHeight = int.Parse(spreadsheet[i][3]);
                int frameCenterX = int.Parse(spreadsheet[i][4]);
                int frameCenterY = int.Parse(spreadsheet[i][5]);
                int frameTime = int.Parse(spreadsheet[i][6]);
                // Use the x, y, width, and height values to create a source rectangle for the current frame, and add it to its list.
                frameSourceRectangles.Add(new Rectangle(frameX, frameY, frameWidth, frameHeight));
                // Use the center x and y values to create a vector for the frame's center, and add it to its list.
                frameCenters.Add(new Vector2(frameCenterX, frameCenterY));
                // Add the frame time to its list.
                frameTimes.Add(int.Parse(spreadsheet[i][6]));
            }

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

        // TODO: stuff
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Vector2 frameCenter = frameCenters[currentFrame];
            Rectangle sourceRectangle = frameSourceRectangles[currentFrame];
            int destX = (int)(position.X - frameCenter.X);
            int destY = (int)(position.Y - frameCenter.Y);
            Rectangle destinationRectangle = new Rectangle(destX, destY, sourceRectangle.Width, sourceRectangle.Height);

            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, Color.White);
        }

        // Resets the animation to the start. Should be desirable to call any time an entity's sprite is switched.
        public void ResetAnimation()
        {
            currentFrame = 0;
            tickCounter = 0;
        }

    }
}
