using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KirbyNightmareInDreamLand.Sprites
{
    // Class for deserializing each sprite animation data in the .json file.
    public class SpriteJsonData
    {
        [JsonPropertyName("Source texture")]
        public string Texture { get; set; }

        [JsonPropertyName("Loop point")]
        public int LoopPoint { get; set; }

        [JsonPropertyName("Flip horizontally?")]
        public bool Flip { get; set; }

        [JsonPropertyName("Frames")]
        public List<FrameJsonData> Frames { get; set; } = new List<FrameJsonData>();
    }

    // Class for deserializing each frame in SpriteJsonData.
    public class FrameJsonData
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
