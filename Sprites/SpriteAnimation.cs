using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Collections.Generic;

namespace MasterGame
{
    public interface ISpriteAnimation
    {

    }

    public class SpriteAnimation : ISpriteAnimation
    {


        // The spritesheet used.
        public Texture2D texture { get; set; }
        // The frame number to loop back to after reaching the end.
        public int loopPoint { get; set; }
        // List of frame source rectangles. Composed of positions imported from animation spreadsheet file.
        public List<Rectangle> frameSourceRectangles { get; set; }
        // List of frame centers, as Vectors.
        public List<Vector2> frameCenters { get; set; }
        // List of frame durations, in game cycles.
        public List<int> frameTimes { get; set; }
        // The number of frames in the animation.
        public int frameCount { get; set; }



        /* Creates a new sprite animation object from a sprite animation file. */
        public SpriteAnimation(string SpriteFilepath, Dictionary<string, Texture2D> textures)
        {

            frameSourceRectangles = new List<Rectangle>();
            frameCenters = new List<Vector2>();
            frameTimes = new List<int>();

            ImportAnimationFile(SpriteFilepath, textures);
            
        }



        // Imports animation data from a .csv file into the proper fields of this object.
        private void ImportAnimationFile(string SpriteFilepath, Dictionary<string, Texture2D> textures)
        {
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



    }
}
