using KirbyNightmareInDreamLand.Controllers;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand
{

    public struct Door
    {
        Vector2 TilePosition;
        string DestinationRoom;

        public Door(Vector2 tilePosition, string destinationRoom)
        {
            TilePosition = tilePosition;
            DestinationRoom = destinationRoom;
        }
    }
    public struct Enemy
    {
        string EnemyType;
        Vector2 TileSpawnPoint;

        public Enemy(string enemyType, Vector2 tileSpawnPoint)
        {
            EnemyType = enemyType;
            TileSpawnPoint = tileSpawnPoint;
        }
    }

    public class Room
    {
        public Sprite BackgroundSprite { get; private set; }
        public Sprite ForegroundSprite { get; private set; }

        // 2D array of physics tile IDs for the room.
        public int[][] TileMap { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Vector2 SpawnPoint { get; private set; }

        public bool CameraXLock { get; private set; }
        public int LockedCameraX { get; private set; }
        public bool CameraYLock { get; private set; }
        public int LockedCameraY { get; private set; }

        public List<Door> Doors { get; private set; }
        public List<Enemy> Enemies { get; private set; }

        // Creates a new room object from a room json data object.
        public Room(RoomJsonData roomJsonData)
        {
            ForegroundSprite = SpriteFactory.Instance.CreateSprite(roomJsonData.ForegroundSpriteName);
            BackgroundSprite = SpriteFactory.Instance.CreateSprite(roomJsonData.BackgroundSpriteName);
            TileMap = LevelLoader.Instance.Tilemaps[roomJsonData.TilemapName];

            TileWidth = TileMap[0].Length;
            TileHeight = TileMap.Length;
            Width = TileWidth * Constants.Level.TILE_SIZE;
            Height = TileHeight * Constants.Level.TILE_SIZE;

            SpawnPoint = new Vector2(roomJsonData.SpawnPointX, roomJsonData.SpawnPointY);

            CameraXLock = roomJsonData.LockCameraX;
            LockedCameraX = roomJsonData.LockedCameraX;
            CameraYLock = roomJsonData.LockCameraY;
            LockedCameraY = roomJsonData.LockedCameraY;

            Doors = new List<Door>();
            foreach (DoorJsonData doorJsonData in roomJsonData.Doors)
            {
                Vector2 TilePosition = new Vector2(doorJsonData.TileX, doorJsonData.TileY);
                string DestinationRoom = doorJsonData.DestinationRoom;
                Door door = new Door(TilePosition, DestinationRoom);
                Doors.Add(door);
            }
            Enemies = new List<Enemy>();
            foreach (EnemyJsonData enemyJsonData in roomJsonData.Enemies)
            {
                string EnemyType = enemyJsonData.DestinationRoom;
                Vector2 TileSpawnPoint = new Vector2(enemyJsonData.SpawnTileX, enemyJsonData.SpawnTileY);
                Enemy enemy = new Enemy(EnemyType, TileSpawnPoint);
                Enemies.Add(enemy);
            }
        }

    }
}
