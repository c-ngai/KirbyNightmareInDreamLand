using KirbyNightmareInDreamLand.Entities;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.PowerUps;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using static KirbyNightmareInDreamLand.Constants;

namespace KirbyNightmareInDreamLand
{
    public class Level
    {

        private readonly Game1 _game;
        private readonly Camera _camera;

        public float BackgroundParallaxFactor = Constants.Graphics.PARALLAX_FACTOR;

        public string EnemyNamespace = Constants.Namespaces.ENEMY_NAMESPACE;
        public string PowerUpNamespace = Constants.Namespaces.POWERUP_NAMESPACE;

        public Room CurrentRoom { get; private set; }

        private List<Enemy> enemyList;

        private List<PowerUp> powerUpList;

        private List<Sprite> TileSprites;

        // Holds a sprite for kirby and each enemy type to draw at their spawn points in level debug mode.
        private Dictionary<string, Sprite> SpawnSprites = new Dictionary<string, Sprite>()
        {
            { "Kirby" , SpriteFactory.Instance.CreateSprite("kirby_normal_standing_right") },
            { "WaddleDee" , SpriteFactory.Instance.CreateSprite("waddledee_walking_left") },
            { "WaddleDoo" , SpriteFactory.Instance.CreateSprite("waddledoo_walking_left") },
            { "BrontoBurt" , SpriteFactory.Instance.CreateSprite("brontoburt_standing_left") },
            { "PoppyBrosJr" , SpriteFactory.Instance.CreateSprite("poppybrosjr_hop_left") },
            { "Sparky" , SpriteFactory.Instance.CreateSprite("sparky_standing_left") },
            { "Hothead" , SpriteFactory.Instance.CreateSprite("hothead_walking_left") },
        };

        public Level()
        {
            _game = Game1.Instance;
            _camera = _game.Camera;

            TileSprites = LoadTileSprites(Constants.Filepaths.TileSpriteList);
        }

        // Loads a room into the level by name.
        public void LoadRoom(string RoomName)
        {
            if (LevelLoader.Instance.Rooms.ContainsKey(RoomName))
            {
                CollisionDetection.Instance.ResetDynamicCollisionBoxes();
                CurrentRoom = LevelLoader.Instance.Rooms[RoomName];
                LoadLevelObjects();
                foreach (IPlayer player in _game.Players)
                {
                    player.GoToRoomSpawn();
                }
            }
            else
            {
                Debug.WriteLine("ERROR: \"" + RoomName + "\" is not a valid room name and cannot be loaded.");
            }
        }

        private List<Sprite> LoadTileSprites(string filepath)
        {
            List<Sprite> TileSprites = new List<Sprite>();

            List<string> lines = new(File.ReadLines(filepath));
            foreach (string line in lines)
            {
                Sprite sprite = SpriteFactory.Instance.CreateSprite(line);
                TileSprites.Add(sprite);
            }

            return TileSprites;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_game.DEBUG_LEVEL_MODE)
            {
                DrawDebug(spriteBatch);
            }
            else
            {
                DrawLevel(spriteBatch);
            }
            
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            if (CurrentRoom.BackgroundSprite != null)
            {
                Vector2 backgroundPosition = new Vector2(
                    _camera.GetPosition().X * BackgroundParallaxFactor,
                    _camera.GetPosition().Y * BackgroundParallaxFactor
                );

                CurrentRoom.BackgroundSprite.Draw(backgroundPosition, spriteBatch); 
            }
        }

        private void DrawForeground(SpriteBatch spriteBatch)
        {
            if (CurrentRoom.ForegroundSprite != null)
            {
                CurrentRoom.ForegroundSprite.Draw(Vector2.Zero, spriteBatch); 
            }
        }

        // Draws the level normally; background and foreground.
        public void DrawLevel(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            DrawForeground(spriteBatch);
            DrawLevelObjects(spriteBatch);
        }

        //level 
        //instantiate on demand
        // this needs to move to level loader or object manager 
        public void LoadLevelObjects()
        {
            enemyList = new List<Enemy>();
            foreach (EnemyData enemy in CurrentRoom.Enemies)
            {
                Type type = Type.GetType(EnemyNamespace + enemy.EnemyType);

                if (type != null)
                {
                    System.Diagnostics.Debug.WriteLine("This is the type name for the enemy: " + type);

                    // Get the constructor that takes a Vector2 parameter
                    ConstructorInfo constructor = type.GetConstructor(new[] { typeof(Vector2) });
                    System.Diagnostics.Debug.WriteLine("this is the enemy constructor" + constructor);

                    if (constructor != null)
                    {
                        // Create an instance of the enemy
                        Enemy enemyObject = (Enemy)constructor.Invoke(new object[] { enemy.SpawnPoint });
                        enemyList.Add(enemyObject);
                    }
                }
            }

            // power ups currently do not require dynamic typing because they all use the same class. Will possibly need to chang ethis later on. 
            powerUpList = new List<PowerUp>();
            foreach(PowerUpData powerUp in CurrentRoom.PowerUps)
            {
                Type type = Type.GetType(PowerUpNamespace);
                PowerUp new_item = new PowerUp(powerUp.SpawnPoint, powerUp.PowerUpType);
                powerUpList.Add(new_item);
            }
        }

        // gets called when player defeats an enemy so that it doesn't get drawn anymore
        public void removeEnemyFromList(Enemy enemy)
        {
            enemyList.Remove(enemy);
        }

        // gets called when player uses a power up so it doesn't get drawn anymore
        public void removePowerUpFromList(PowerUp powerUp)
        {
            powerUpList.Remove(powerUp);
        }

        // draws enemies and tomatoes
        public void DrawLevelObjects(SpriteBatch spriteBatch)
        {
            foreach(Enemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (PowerUp powerUp in powerUpList)
            {
                powerUp.Draw(spriteBatch);
            }
        }

        // tells player if they are at a door or not 
        public bool atDoor(Vector2 playerPosition)
        {
            bool result = false;
            foreach(Door door in CurrentRoom.Doors)
            {
                if(door.Bounds.Contains(playerPosition))
                {
                    result = true;
                }
            }

            return result;
        }

        // go to the next room, called because a player wants to go through a door 
        public void nextRoom(Vector2 playerPos)
        {
            foreach(Door door in CurrentRoom.Doors)
            {
                if (door.Bounds.Contains(playerPos))
                {
                    LoadRoom(door.DestinationRoom);
                    break;
                }
            }
        }

        public Vector2 convertTileToPixel(Vector2 tilePosition)
        {
            return new Vector2(tilePosition.X * Constants.Level.TILE_SIZE, tilePosition.Y * Constants.Level.TILE_SIZE);
        }

        public void UpdateLevel()
        {
            CurrentRoom.ForegroundSprite.Update();
            foreach(Enemy enemy in enemyList)
            {
                enemy.Update(_game.time);
            }
            foreach(PowerUp powerUp in powerUpList)
            {
                powerUp.Update();
            }
        }

        // Following methods authored by Mark 

        // Debug mode (toggle F2), draws the usually-invisible collision tiles, doors, and enemy spawn locations.
        private void DrawDebug(SpriteBatch spriteBatch)
        {
            DrawTiles(spriteBatch);
            DrawDoors(spriteBatch);
            DrawSpawnPoints(spriteBatch);
            DrawLevelObjects(spriteBatch);
        }

        // Draws a rectangle at every door with its destination room written above
        private void DrawDoors(SpriteBatch spriteBatch)
        {
            foreach (Door door in CurrentRoom.Doors)
            {
                Vector2 doorPos = door.Bounds.Location.ToVector2();
                Vector2 textSize = LevelLoader.Instance.Font.MeasureString(door.DestinationRoom);
                Vector2 textPos = doorPos - new Vector2(-9 + textSize.X / 2, -1 + textSize.Y);

                GameDebug.Instance.DrawSolidRectangle(spriteBatch, door.Bounds, Color.Red);
                spriteBatch.DrawString(LevelLoader.Instance.Font, door.DestinationRoom, textPos, Color.Red);
            }
        }

        // Draws static, transparent sprites of the corresponding enemy for each enemy spawn point in the level.
        private void DrawSpawnPoints(SpriteBatch spriteBatch)
        {
            // Temporarily disable sprite debug mode if it's on.
            bool old_DEBUG_SPRITE_MODE = _game.DEBUG_SPRITE_MODE;
            _game.DEBUG_SPRITE_MODE = false;

            Vector2 kirbyPos = CurrentRoom.SpawnPoint;
            SpawnSprites["Kirby"].Draw(kirbyPos, spriteBatch, new Color(255, 255, 255, 127));

            // Draw each enemy spawn point
            foreach (EnemyData enemy in CurrentRoom.Enemies)
            {
                Vector2 enemyPos = enemy.SpawnPoint;
                SpawnSprites[enemy.EnemyType].Draw(enemyPos, spriteBatch, new Color(255, 255, 255, 63));
            }

            // Restore old sprite debug mode state.
            _game.DEBUG_SPRITE_MODE = old_DEBUG_SPRITE_MODE;
        }

        // Draw visualizations of all the usually-invisible collision tiles.
        private void DrawTiles(SpriteBatch spriteBatch)
        {
            // Temporarily disable sprite debug mode if it's on. Sprite debug with debug tiles makes the screen look very messy, it's not useful information. This feels like a sloppy solution but it works for now.
            bool old_DEBUG_SPRITE_MODE = _game.DEBUG_SPRITE_MODE;
            _game.DEBUG_SPRITE_MODE = false;

            // Set bounds on the TileMap to iterate from
            int TopY, BottomY, LeftX, RightX;
            if (_game.CULLING_ENABLED)
            {
                TopY = Math.Max(_camera.GetBounds().Top / Constants.Level.TILE_SIZE, 0);
                BottomY = Math.Min(_camera.GetBounds().Bottom / Constants.Level.TILE_SIZE + 1, CurrentRoom.TileHeight);
                LeftX = Math.Max(_camera.GetBounds().Left / Constants.Level.TILE_SIZE, 0);
                RightX = Math.Min(_camera.GetBounds().Right / Constants.Level.TILE_SIZE + 1, CurrentRoom.TileWidth);
            }
            else
            {
                TopY = 0;
                BottomY = CurrentRoom.TileHeight;
                LeftX = 0;
                RightX = CurrentRoom.TileWidth;
            }

            // Temporarily disable sprite culling if it's on, because this function has its own culling that relies on the regularity of tiles that is much more efficient than the rectangle intersection-detecting method of the Sprite class.
            bool old_CULLING_ENABLED = _game.CULLING_ENABLED;
            _game.CULLING_ENABLED = false;

            // Iterate across all the rows of the TileMap visible within the frame of the camera
            for (int y = TopY; y < BottomY; y++)
            {
                // Iterate across all the columns of the TileMap visible within the frame of the camera
                for (int x = LeftX; x < RightX; x++)
                {
                    DrawTile(spriteBatch, CurrentRoom.TileMap[y][x], new Vector2(x * Constants.Level.TILE_SIZE, y * Constants.Level.TILE_SIZE));
                }
            }

            // Restore old sprite debug mode state.
            _game.DEBUG_SPRITE_MODE = old_DEBUG_SPRITE_MODE;
            _game.CULLING_ENABLED = old_CULLING_ENABLED;
        }

        // Draw a single tile.
        private void DrawTile(SpriteBatch spriteBatch, int tileID, Vector2 position)
        {
            TileSprites[tileID].Draw(position, spriteBatch);
        }

        // Given a rectangle in the world, returns a List of all Tiles in the level that intersect with that given rectangle.
        public List<Tile> IntersectingTiles(Rectangle collisionRectangle)
        {
            List<Tile> tiles = new List<Tile>();

            // Set bounds on the TileMap to iterate from
            int TopY = Math.Max(collisionRectangle.Top / Constants.Level.TILE_SIZE, 0);
            int BottomY = Math.Min(collisionRectangle.Bottom / Constants.Level.TILE_SIZE + 1, CurrentRoom.TileHeight);
            int LeftX = Math.Max(collisionRectangle.Left / Constants.Level.TILE_SIZE, 0);
            int RightX = Math.Min(collisionRectangle.Right / Constants.Level.TILE_SIZE + 1, CurrentRoom.TileWidth);

            // Iterate across all the rows of the TileMap visible within the frame of the camera
            for (int y = TopY; y < BottomY; y++)
            {
                // Iterate across all the columns of the TileMap visible within the frame of the camera
                for (int x = LeftX; x < RightX; x++)
                {
                    Tile tile = new Tile();
                    tile.type = (TileCollisionType)CurrentRoom.TileMap[y][x];
                    tile.rectangle = new Rectangle(x * Constants.Level.TILE_SIZE, y * Constants.Level.TILE_SIZE, Constants.Level.TILE_SIZE, Constants.Level.TILE_SIZE);
                    tiles.Add(tile);

                    // Registers each relevant tile into the collisionHandler
                    tile.RegisterTile();
                }
            }

            return tiles;
        }

    }
}
