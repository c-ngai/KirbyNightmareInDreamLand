using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KirbyNightmareInDreamLand.Collision
{
    // Class for deserializing each hitbox data in the .json file.
    public class HitboxJsonData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
    }

}
