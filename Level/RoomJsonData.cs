using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KirbyNightmareInDreamLand.Sprites
{
    // Class for deserializing each room data in the .json file.
    public class RoomJsonData
    {
        [JsonPropertyName("Foreground sprite")]
        public string ForegroundSpriteName { get; set; }

        [JsonPropertyName("Background sprite")]
        public string BackgroundSpriteName { get; set; }

        [JsonPropertyName("Tilemap")]
        public string TilemapName { get; set; }

        [JsonPropertyName("Spawn point X")]
        public int SpawnPointX { get; set; }

        [JsonPropertyName("Spawn point Y")]
        public int SpawnPointY { get; set; }


        [JsonPropertyName("Lock camera X?")]
        public bool LockCameraX { get; set; }

        [JsonPropertyName("Locked camera X")]
        public int LockedCameraX { get; set; }

        [JsonPropertyName("Lock camera Y?")]
        public bool LockCameraY { get; set; }

        [JsonPropertyName("Locked camera Y")]
        public int LockedCameraY { get; set; }


        [JsonPropertyName("Doors")]
        public List<DoorJsonData> Doors { get; set; } = new List<DoorJsonData>();

        [JsonPropertyName("Enemies")]
        public List<EnemyJsonData> Enemies { get; set; } = new List<EnemyJsonData>();

        [JsonPropertyName("Tomatoes")]
        public List<TomatoJsonData> Tomatoes { get; set; } = new List<TomatoJsonData>();

    }
        // Class for deserializing each door in RoomJsonData.
        public class DoorJsonData
    {
        public int TileX { get; set; }
        public int TileY { get; set; }
        public string DestinationRoom { get; set; }
    }

    // Class for deserializing each enemy in RoomJsonData.
    public class EnemyJsonData
    {
        public int SpawnTileX { get; set; }
        public int SpawnTileY { get; set; }
        public string EnemyType { get; set; }
    }

    // Class for deserializing each tomato in RoomJsonData.
    public class TomatoJsonData
    {
        public int SpawnTileX { get; set; }
        public int SpawnTileY { get; set; }
    }
}
