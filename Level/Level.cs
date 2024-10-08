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
        // Make private later probably, coupling issues
        public Room room;

        private List<Sprite> TileSprites;

        public Level(Game1 game)
        {
            _game = game;
            _camera = game.camera;

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

        // Draws the level normally, background and foreground.
        public void DrawLevel(SpriteBatch spriteBatch)
        {
            // TODO: Vivian
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
        }

        private void DrawTile(SpriteBatch spriteBatch, int tileID, Vector2 position)
        {
            TileSprites[tileID].Draw(position, spriteBatch);
        }

    }
}
