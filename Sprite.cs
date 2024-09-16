using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Collections.Generic;

// THIS IS MESSY, EARLY TEST!!!!

namespace MasterGame
{
    public class Sprite
    {

        // The filepath of the source image for the animation's spritesheet.
        private string sourceImageFilePath;
        /* The frame to loop back to after reaching the end. Usually this is 0 if an
         * animation loops in its entirety, but sometimes it is desirable to set it
         * further if you want a certain section at the start of an animation to only
         * play the first time. */
        private int loopPoint;
        // List of frame times. Imported from animation spreadsheet file.
        private List<int> frameTime;
        // List of source rectangles. Composed of positions imported from animation spreadsheet file.
        private List<Rectangle> frameSourceRectangles;
        // The number of frames in the animation.
        private int frameCount;

        // The current frame of the animation.
        private int currentFrame;
        // The current number of game ticks since last frame advance.
        private int tickCounter;

        /* Creates a new animation object from an animation file. Imports animation
         * data from a .csv file into the Animation object. */
        public Sprite(string filepath)
        {
            currentFrame = 0;
            tickCounter = 0;
            frameTime = new List<int>();
            frameSourceRectangles = new List<Rectangle>();

            List<string> rows = new(File.ReadLines(filepath));
            // Set the total frame count.
            frameCount = rows.Count - 3;
            // Set the loop point.
            string[] columns = rows[1].Split(',');
            loopPoint = int.Parse(columns[1]);
            // For each row in the table, separate it into columns by commas
            for (int i = 3; i < rows.Count; i++)
            {
                columns = rows[i].Split(',');
                // Add each column's value to its respective list. x, y, width, height, time - in that order.
                int frameX = int.Parse(columns[0]);
                int frameY = int.Parse(columns[1]);
                int frameWidth = int.Parse(columns[2]);
                int frameHeight = int.Parse(columns[3]);
                frameTime.Add(int.Parse(columns[4]));
                // Use the new positional values to create a rectangle, and add
                //Debug.WriteLine(frameX, frameY, frameWidth, frameHeight);
                frameSourceRectangles.Add(new Rectangle(frameX, frameY, frameWidth, frameHeight));
            }

        }

        // Updates the animation for the game tick.
        public void Update()
        {
            // Advance the tick counter. If it's reached the frame time of the current frame, advance the frame and reset the tick counter.
            tickCounter++;
            if (tickCounter == frameTime[currentFrame])
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

        // Returns the source rectangle of the current frame.
        public Rectangle getSourceRectangle()
        {
            return frameSourceRectangles[currentFrame];
        }

        // Returns the width of the current frame.
        public int getWidth()
        {
            return frameSourceRectangles[currentFrame].Width;
        }

        // Returns the height of the current frame.
        public int getHeight()
        {
            return frameSourceRectangles[currentFrame].Height;
        }

    }
}
