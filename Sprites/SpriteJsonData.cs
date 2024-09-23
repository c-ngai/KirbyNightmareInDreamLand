using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MasterGame
{
    // Class for deserializing each sprite animation data in the .json file.
    public class SpriteJsonData
    {
        [JsonPropertyName("Source texture")]
        public string texture { get; set; }

        [JsonPropertyName("Loop point")]
        public int loopPoint { get; set; }

        [JsonPropertyName("Flip horizontally?")]
        public bool flip { get; set; }

        [JsonPropertyName("Frames")]
        public List<Frame> frames { get; set; } = new List<Frame>();
    }

    // Class for deserializing each frame in SpriteJsonData.
    public class Frame
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public int Time { get; set; }
    }
}
