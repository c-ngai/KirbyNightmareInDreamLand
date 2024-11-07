using System;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace KirbyNightmareInDreamLand.GameState
{
    public class GameDebugState : BaseGameState
    {
        Game GAME;
        Camera _camera;
        SpriteBatch spriteBatch;
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

        public GameDebugState()
        {
            GAME = Game1.Instance;
            _camera = Game1.Instance.Camera;
            spriteBatch = Game1.Instance._spriteBatch;
            TileSprites = LoadTileSprites(Constants.Filepaths.TileSpriteList);

        }

        public override void Draw()
        {
            DrawBackground(spriteBatch);
            DrawTiles(spriteBatch);
            DrawDoorStars(spriteBatch);
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
                textPos.Floor();

                GameDebug.Instance.DrawSolidRectangle(spriteBatch, door.Bounds, Color.Red, 0.5f);
                spriteBatch.DrawString(LevelLoader.Instance.Font, door.DestinationRoom, textPos, Color.Red);
            }
        }

        Color translucent = new Color(127, 127, 127, 127);
        // Draws static, transparent sprites of the corresponding enemy for each enemy spawn point in the level.
        private void DrawSpawnPoints(SpriteBatch spriteBatch)
        {
            // Temporarily disable sprite debug mode if it's on.
            bool old_DEBUG_SPRITE_MODE = Game1.Instance.DEBUG_SPRITE_MODE;
            Game1.Instance.DEBUG_SPRITE_MODE = false;

            Vector2 kirbyPos = CurrentRoom.SpawnPoint;
            SpawnSprites["Kirby"].Draw(kirbyPos, spriteBatch, translucent);

            // Draw each enemy spawn point
            foreach (EnemyData enemy in CurrentRoom.Enemies)
            {
                Vector2 enemyPos = enemy.SpawnPoint;
                SpawnSprites[enemy.EnemyType].Draw(enemyPos, spriteBatch, translucent);
            }

            // Restore old sprite debug mode state.
            Game1.Instance.DEBUG_SPRITE_MODE = old_DEBUG_SPRITE_MODE;
        }

        // Draw visualizations of all the usually-invisible collision tiles.
        private void DrawTiles(SpriteBatch spriteBatch)
        {
            // Temporarily disable sprite debug mode if it's on. Sprite debug with debug tiles makes the screen look very messy, it's not useful information. This feels like a sloppy solution but it works for now.
            bool old_DEBUG_SPRITE_MODE = Game1.Instance.DEBUG_SPRITE_MODE;
            Game1.Instance.DEBUG_SPRITE_MODE = false;

            // Set bounds on the TileMap to iterate from
            int TopY, BottomY, LeftX, RightX;
            if (Game1.Instance.CULLING_ENABLED)
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
            bool old_CULLING_ENABLED = Game1.Instance.CULLING_ENABLED;
            Game1.Instance.CULLING_ENABLED = false;

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
            Game1.Instance.DEBUG_SPRITE_MODE = old_DEBUG_SPRITE_MODE;
            Game1.Instance.CULLING_ENABLED = old_CULLING_ENABLED;
        }

        // Draw a single tile.
        private void DrawTile(SpriteBatch spriteBatch, int tileID, Vector2 position)
        {
            TileSprites[tileID].Draw(position, spriteBatch);
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


    }

}