using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MasterGame
{
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
