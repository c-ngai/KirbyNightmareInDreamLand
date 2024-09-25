using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
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
        // The sprite effects. Simply holds a flag of whether or not to horizontally flip the texture.
        public SpriteEffects spriteEffects { get; set; }
        // List of frame source rectangles. Composed of positions imported from animation spreadsheet file.
        public List<Rectangle> frameSourceRectangles { get; set; }
        // List of frame centers, as Vectors.
        public List<Vector2> frameCenters { get; set; }
        // List of frame durations, in game cycles.
        public List<int> frameTimes { get; set; }
        // The number of frames in the animation.
        public int frameCount { get; set; }

        /* Creates a new sprite animation object from a sprite animation file. */
        public SpriteAnimation(SpriteJsonData spriteJsonData, Dictionary<string, Texture2D> textures)
        {
            frameSourceRectangles = new List<Rectangle>();
            frameCenters = new List<Vector2>();
            frameTimes = new List<int>();

            ImportAnimation(spriteJsonData, textures);
        }

        // Imports animation data from a .csv file into the proper fields of this object.
        private void ImportAnimation(SpriteJsonData spriteJsonData, Dictionary<string, Texture2D> textures)
        {
            texture = textures[spriteJsonData.texture];
            loopPoint = spriteJsonData.loopPoint;
            if (spriteJsonData.flip == true)
                spriteEffects = SpriteEffects.FlipHorizontally;
            else
                spriteEffects = SpriteEffects.None;
            // Set the total frame count.
            frameCount = spriteJsonData.frames.Count;

            // For each row in the table, separate it into columns by commas
            for (int i = 0; i < frameCount; i++)
            {
                Frame frame = spriteJsonData.frames[i];
                // Use the x, y, width, and height values to create a source rectangle for the current frame, and add it to its list.
                frameSourceRectangles.Add(new Rectangle(frame.X, frame.Y, frame.Width, frame.Height));
                // Use the center x and y values to create a vector for the frame's center, and add it to its list.
                frameCenters.Add(new Vector2(frame.CenterX, frame.CenterY));
                // Add the frame time to its list.
                frameTimes.Add(frame.Time);
            }
        }
    }
}
