using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Sprites
{
    public interface ISpriteAnimation
    {

    }

    public class SpriteAnimation : ISpriteAnimation
    {
        // The spritesheet used.
        public Texture2D Texture { get; set; }
        // The frame number to loop back to after reaching the end.
        public int LoopPoint { get; set; }
        // The sprite effects. Simply holds a flag of whether or not to horizontally flip the texture.
        public SpriteEffects SpriteEffects { get; set; }
        // List of frame source rectangles. Composed of positions imported from animation spreadsheet file.
        public List<Rectangle> FrameSourceRectangles { get; set; }
        // List of frame centers, as Vectors.
        public List<Vector2> FrameCenters { get; set; }
        // List of frame durations, in game cycles.
        public List<int> FrameTimes { get; set; }
        // The number of frames in the animation.
        public int FrameCount { get; set; }

        /* Creates a new sprite animation object from a sprite animation file. */
        public SpriteAnimation(SpriteJsonData spriteJsonData, Dictionary<string, Texture2D> textures)
        {
            FrameSourceRectangles = new List<Rectangle>();
            FrameCenters = new List<Vector2>();
            FrameTimes = new List<int>();

            ImportAnimation(spriteJsonData, textures);
        }

        // Imports animation data from a .csv file into the proper fields of this object.
        private void ImportAnimation(SpriteJsonData spriteJsonData, Dictionary<string, Texture2D> textures)
        {
            Texture = textures[spriteJsonData.Textures];
            LoopPoint = spriteJsonData.LoopPoint;
            if (spriteJsonData.Flip == true)
                SpriteEffects = SpriteEffects.FlipHorizontally;
            else
                SpriteEffects = SpriteEffects.None;
            // Set the total frame count.
            FrameCount = spriteJsonData.Frames.Count;

            // For each row in the table, separate it into columns by commas
            for (int i = 0; i < FrameCount; i++)
            {
                Frame frame = spriteJsonData.Frames[i];
                // Use the x, y, width, and height values to create a source rectangle for the current frame, and add it to its list.
                FrameSourceRectangles.Add(new Rectangle(frame.X, frame.Y, frame.Width, frame.Height));
                // Use the center x and y values to create a vector for the frame's center, and add it to its list.
                FrameCenters.Add(new Vector2(frame.CenterX, frame.CenterY));
                // Add the frame time to its list.
                FrameTimes.Add(frame.Time);
            }
        }
    }
}
