using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

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


            // Set vertical bounds on the TileMap to iterate from
            int TopY = Math.Max(_camera.TopY / Constants.Level.TILE_SIZE, 0);
            int BottomY = Math.Min(_camera.BottomY / Constants.Level.TILE_SIZE + 1, room.TileMap.Length);
            // Iterate across all the rows of the TileMap visible within the frame of the camera
            for (int y = TopY; y < BottomY; y++)
            {
                // Set horizontal bounds on the TileMap to iterate from
                int LeftX = Math.Max(_camera.LeftX / Constants.Level.TILE_SIZE, 0);
                int RightX = Math.Min(_camera.RightX / Constants.Level.TILE_SIZE + 1, room.TileMap[y].Length);
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
            TileSprites[tileID].LevelDraw(position, spriteBatch);
        }

    }
}
