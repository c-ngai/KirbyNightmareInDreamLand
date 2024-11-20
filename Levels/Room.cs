using KirbyNightmareInDreamLand.Controllers;
using KirbyNightmareInDreamLand.Entities;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand.Levels
{

    public struct Door
    {
        public Rectangle Bounds;
        public string DestinationRoom;
        public Vector2 DestinationPoint;
        public bool DrawDoorStars;
        public bool IsBigDoor;

        public Door(Rectangle bounds, string destinationRoom, Vector2 destinationPoint, bool drawDoorStars, bool isBigDoor)
        {
            Bounds = bounds;
            DestinationRoom = destinationRoom;
            DestinationPoint = destinationPoint;
            DrawDoorStars = drawDoorStars;
            IsBigDoor = isBigDoor;
        }
    }
    public struct EnemyData
    {
        public string EnemyType;
        public Vector2 SpawnPoint;

        public EnemyData(string enemyType, Vector2 spawnPoint)
        {
            EnemyType = enemyType;
            SpawnPoint = spawnPoint;
        }
    }
    public struct PowerUpData
    {
        public string PowerUpType;
        public Vector2 SpawnPoint;

        public PowerUpData(string powerUpType, Vector2 spawnPoint)
        {
            SpawnPoint = spawnPoint;
            PowerUpType = powerUpType;
        }
    }

    public class Room
    {
        public string Name { get; private set; }
        
        public Sprite BackgroundSprite { get; private set; }
        public Sprite ForegroundSprite { get; private set; }

        // 2D array of physics tile IDs for the room.
        public int[][] TileMap { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Vector2 SpawnTile { get; private set; }
        public Vector2 SpawnPoint { get; private set; }

        public bool CameraXLock { get; private set; }
        public int LockedCameraX { get; private set; }
        public bool CameraYLock { get; private set; }
        public int LockedCameraY { get; private set; }

        public List<Door> Doors { get; private set; }
        public List<EnemyData> Enemies { get; private set; }
        public List<PowerUpData> PowerUps { get; private set; }

        // Creates a new room object from a room json data object.
        public Room(string roomName, RoomJsonData roomJsonData)
        {
            Name = roomName;
            
            ForegroundSprite = SpriteFactory.Instance.CreateSprite(roomJsonData.ForegroundSpriteName);
            BackgroundSprite = SpriteFactory.Instance.CreateSprite(roomJsonData.BackgroundSpriteName);
            TileMap = LevelLoader.Instance.Tilemaps[roomJsonData.TilemapName];

            TileWidth = TileMap[0].Length;
            TileHeight = TileMap.Length;
            Width = TileWidth * Constants.Level.TILE_SIZE;
            Height = TileHeight * Constants.Level.TILE_SIZE;

            SpawnTile = new Vector2(roomJsonData.SpawnTileX, roomJsonData.SpawnTileY);
            SpawnPoint = SpawnTile * Constants.Level.TILE_SIZE + Constants.Level.BOTTOM_MIDDLE_OF_TILE;

            CameraXLock = roomJsonData.LockCameraX;
            LockedCameraX = roomJsonData.LockedCameraX;
            CameraYLock = roomJsonData.LockCameraY;
            LockedCameraY = roomJsonData.LockedCameraY;

            Doors = new List<Door>();
            foreach (DoorJsonData doorJsonData in roomJsonData.Doors)
            {
                
                Rectangle Bounds = new Rectangle(
                    doorJsonData.TileX * Constants.Level.TILE_SIZE,
                    doorJsonData.TileY * Constants.Level.TILE_SIZE,
                    doorJsonData.IsBigDoor ? Constants.Level.TILE_SIZE * 2 : Constants.Level.TILE_SIZE,
                    Constants.Level.TILE_SIZE + 1); // Plus one for now because when standing on the ground, kirby's position is right at the bottom edge of the door rectangle normally, which isn't counted as "inside" it. Door hitbox goes 1 pixel into the ground.
                string DestinationRoom = doorJsonData.DestinationRoom;
                Vector2 DestinationPoint = new Vector2(
                    doorJsonData.DestinationTileX, doorJsonData.DestinationTileY)
                    * Constants.Level.TILE_SIZE
                    + Constants.Level.BOTTOM_MIDDLE_OF_TILE;
                bool DrawDoorStars = doorJsonData.DrawDoorStars;
                bool IsBigDoor = doorJsonData.IsBigDoor;
                Door door = new Door(Bounds, DestinationRoom, DestinationPoint, DrawDoorStars, IsBigDoor);
                Doors.Add(door);
            }

            Enemies = new List<EnemyData>();
            foreach (EnemyJsonData enemyJsonData in roomJsonData.Enemies)
            {
                string EnemyType = enemyJsonData.EnemyType;
                Vector2 TileSpawnPoint = new Vector2(enemyJsonData.SpawnTileX, enemyJsonData.SpawnTileY);
                Vector2 SpawnPoint = TileSpawnPoint * Constants.Level.TILE_SIZE + Constants.Level.BOTTOM_MIDDLE_OF_TILE;
                EnemyData enemy = new EnemyData(EnemyType, SpawnPoint);

                // Add enemy to list if it has a valid name
                if (Constants.ValidEnemyNames.Contains(enemyJsonData.EnemyType))
                {
                    Enemies.Add(enemy);
                }
                else
                {
                    Debug.WriteLine(" [ERROR] In room \"" + Name + "\", \"" + enemyJsonData.EnemyType + "\" is not a valid enemy name. (check capitalization?)");
                }
            }

            PowerUps = new List<PowerUpData>();
            foreach (PowerUpJsonData powerUpJsonData in roomJsonData.PowerUps)
            {
                string PowerUpType = powerUpJsonData.PowerUpType;
                Vector2 TileSpawnPoint = new Vector2(powerUpJsonData.SpawnTileX, powerUpJsonData.SpawnTileY);
                Vector2 SpawnPoint = TileSpawnPoint * Constants.Level.TILE_SIZE + Constants.Level.BOTTOM_MIDDLE_OF_TILE;
                PowerUpData powerUp = new PowerUpData(PowerUpType, SpawnPoint);
                PowerUps.Add(powerUp);
            }
        }

    }
}
