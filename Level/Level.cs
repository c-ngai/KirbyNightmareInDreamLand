using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace KirbyNightmareInDreamLand
{
    public class Level
    {

        private Game1 _game;
        private Camera _camera;

        public float BackgroundParallaxFactor { get; set; } = 0.85f;

        // Make private later probably, coupling issues
        public Room room;

        private List<Sprite> TileSprites;

        public Level()
        {
            _game = Game1.Instance;
            _camera = _game.camera;

            TileSprites = LoadTileSprites("Content/Images/TileSprites.txt");
        }


        public void LoadRoom(string RoomName)
        {
            room = LevelLoader.Instance.Rooms[RoomName];
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
            if (room.BackgroundSprite != null)
            {
                Vector2 backgroundPosition = new Vector2(
                    _camera.GetPosition().X * BackgroundParallaxFactor,
                    _camera.GetPosition().Y * BackgroundParallaxFactor
                );

                room.BackgroundSprite.Draw(backgroundPosition, spriteBatch); // Draw at origin or wherever it should be
            }
        }

        private void DrawForeground(SpriteBatch spriteBatch)
        {
            if (room.ForegroundSprite != null)
            {
                room.ForegroundSprite.Draw(Vector2.Zero, spriteBatch); // Draw at origin or wherever it should be
            }
        }

        // Draws the level normally; background and foreground.
        public void DrawLevel(SpriteBatch spriteBatch)
        {

            // Draw background 
            DrawBackground(spriteBatch);

            // Draw the room's foreground
            DrawForeground(spriteBatch);
        }

        public void UpdateLevel(SpriteBatch spriteBatch)
        {
            room.ForegroundSprite.Update();
            
        }

        // Debug mode (toggle F2), draws the usually-invisible collision tiles.
        public void DrawDebug(SpriteBatch spriteBatch)
        {
            // Temporarily disable sprite debug mode if it's on. Sprite debug with debug tiles makes the screen look very messy, it's not useful information. This feels like a sloppy solution but it works for now.
            bool old_DEBUG_SPRITE_MODE = _game.DEBUG_SPRITE_MODE;
            _game.DEBUG_SPRITE_MODE = false;

            // Set bounds on the TileMap to iterate from
            int TopY, BottomY, LeftX, RightX;
            if (_game.CULLING_ENABLED)
            {
                TopY = Math.Max(_camera.GetBounds().Top / Constants.Level.TILE_SIZE, 0);
                BottomY = Math.Min(_camera.GetBounds().Bottom / Constants.Level.TILE_SIZE + 1, room.TileMap.Length);
                LeftX = Math.Max(_camera.GetBounds().Left / Constants.Level.TILE_SIZE, 0);
                RightX = Math.Min(_camera.GetBounds().Right / Constants.Level.TILE_SIZE + 1, room.TileMap[0].Length);
            }
            else
            {
                TopY = 0;
                BottomY = room.TileMap.Length;
                LeftX = 0;
                RightX = room.TileMap[0].Length;
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
                    DrawTile(spriteBatch, room.TileMap[y][x], new Vector2(x * Constants.Level.TILE_SIZE, y * Constants.Level.TILE_SIZE));
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
            int BottomY = Math.Min(collisionRectangle.Bottom / Constants.Level.TILE_SIZE + 1, room.TileHeight);
            int LeftX = Math.Max(collisionRectangle.Left / Constants.Level.TILE_SIZE, 0);
            int RightX = Math.Min(collisionRectangle.Right / Constants.Level.TILE_SIZE + 1, room.TileWidth);

            // Iterate across all the rows of the TileMap visible within the frame of the camera
            for (int y = TopY; y < BottomY; y++)
            {
                // Iterate across all the columns of the TileMap visible within the frame of the camera
                for (int x = LeftX; x < RightX; x++)
                {
                    Tile tile = new Tile();
                    tile.type = (TileCollisionType)room.TileMap[y][x];
                    tile.rectangle = new Rectangle(x * Constants.Level.TILE_SIZE, y * Constants.Level.TILE_SIZE, Constants.Level.TILE_SIZE, Constants.Level.TILE_SIZE);
                    tiles.Add(tile);
                }
            }

            return tiles;
        }

    }
}
