using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using static KirbyNightmareInDreamLand.Constants;

namespace KirbyNightmareInDreamLand
{
    public class Level
    {

        private Game1 _game;
        private Camera _camera;

        public float BackgroundParallaxFactor { get; set; } = 0.85f; // fix magic number 

        // Make private later probably, coupling issues
        public Room CurrentRoom;

        private List<Enemy> enemyList;

        private List<Sprite> TileSprites;

        public Level()
        {
            _game = Game1.Instance;
            _camera = _game.camera;

            TileSprites = LoadTileSprites("Content/Images/TileSprites.txt");
        }

        public void LoadRoom(string RoomName)
        {
            CurrentRoom = LevelLoader.Instance.Rooms[RoomName];
            LoadLevelObjects();
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

        public void LoadLevelObjects()
        {
            enemyList = new List<Enemy>();
            foreach (EnemyData enemy in CurrentRoom.Enemies)
            {
                Vector2 enemySpawnPoint = convertTileToPixel(enemy.TileSpawnPoint);
                Type type = Type.GetType("KirbyNightmareInDreamLand.Entities.Enemies." + enemy.EnemyType);

                if (type != null)
                {
                    // Get the constructor that takes a Vector2 parameter
                    var constructor = type.GetConstructor(new[] { typeof(Vector2) });

                    if (constructor != null)
                    {
                        // Create an instance of the enemy
                        Enemy enemyObject = (Enemy)constructor.Invoke(new object[] { enemySpawnPoint });
                        enemyList.Add(enemyObject);
                    }
                }
            }
        }

        // draws enemies and tomatoes
        public void DrawLevelObjects(SpriteBatch spriteBatch)
        {
            foreach(Enemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
            }

        }

        // tells player if they are at a door or not 
        public bool atDoor(Vector2 playerPosition)
        {
            bool result = false;
            foreach(Door door in CurrentRoom.Doors)
            {
                Vector2 doorPos = convertTileToPixel(door.TilePosition);
                Rectangle door_rec = new Rectangle(
                    (int)doorPos.X,
                    (int)doorPos.Y,
                    Constants.Level.TILE_SIZE,
                    Constants.Level.TILE_SIZE);
                if(door_rec.Contains(playerPosition))
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
                Vector2 doorPos = convertTileToPixel(door.TilePosition);
                Rectangle door_rec = new Rectangle(
                    (int)doorPos.X,
                    (int)doorPos.Y,
                    Constants.Level.TILE_SIZE,
                    Constants.Level.TILE_SIZE);

                if (door_rec.Contains(playerPos))
                {
                    LoadRoom(door.DestinationRoom);
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
        }

        // Following methods authored by Mark 

        // Debug mode (toggle F2), draws the usually-invisible collision tiles, doors, and enemy spawn locations.
        private void DrawDebug(SpriteBatch spriteBatch)
        {
            DrawTiles(spriteBatch);
            DrawDoors(spriteBatch);
            //DrawEnemySpawns(spriteBatch);
            DrawLevelObjects(spriteBatch);
        }

        // Draws a rectangle at every door with its destination room written above
        private void DrawDoors(SpriteBatch spriteBatch)
        {
            foreach (Door door in CurrentRoom.Doors)
            {
                Vector2 doorPos = door.TilePosition * Constants.Level.TILE_SIZE;
                Rectangle doorBounds = new Rectangle(doorPos.ToPoint(), new Point(16, 16));
                Vector2 textSize = LevelLoader.Instance.font.MeasureString(door.DestinationRoom);
                Vector2 textPos = doorPos - new Vector2(-9 + textSize.X / 2, -1 + textSize.Y);

                GameDebug.Instance.DrawSolidRectangle(spriteBatch, doorBounds, Color.Red);
                spriteBatch.DrawString(LevelLoader.Instance.font, door.DestinationRoom, textPos, Color.Red);
            }
        }

        // 
        private void DrawEnemySpawns(SpriteBatch spriteBatch)
        {
            foreach (EnemyData enemy in CurrentRoom.Enemies)
            {
                Vector2 doorPos = enemy.TileSpawnPoint * Constants.Level.TILE_SIZE;
                Rectangle doorBounds = new Rectangle(doorPos.ToPoint(), new Point(16, 16));
                GameDebug.Instance.DrawSolidRectangle(spriteBatch, doorBounds, Color.Red);
            }
        }

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
