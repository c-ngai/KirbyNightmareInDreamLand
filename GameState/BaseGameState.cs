
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.PowerUps;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace KirbyNightmareInDreamLand.GameState
{
    public abstract class BaseGameState : IGameState
    {
        public readonly Game1 _game;
        private readonly ObjectManager _manager;
        public Level level;
        private readonly Sprite DoorStarsSprite;
        public Vector2 SpawnPoint { get; set; }
        private List<Sprite> TileSprites;

        private static Dictionary<int, string> HubDoors = new Dictionary<int, string> 
        {
            { 0 , "hub_door_1_and_2"},
            { 2, "hub_door_1_and_2"},
            { 4, "hub_door3"},
            { 6, "hub_door4" }
        };

        private static Dictionary<int, ISprite> HubDoorAnimations = new Dictionary<int, ISprite>
        {
            { 1 , SpriteFactory.Instance.CreateSprite("hub_door_1_and_2_animation")},
            { 2 , SpriteFactory.Instance.CreateSprite("hub_door_1_and_2_animation")},
            { 3 , SpriteFactory.Instance.CreateSprite("hub_door3_animation")},
            { 4 , SpriteFactory.Instance.CreateSprite("hub_door4_animation")}
        };

        private static Vector2 drawHubDoorOffset = new Vector2(0, -8);

        private static Vector2 drawHubSignOffset = new Vector2(2, -24);

        // Holds a sprite for kirby and each enemy type to draw at their spawn points in level debug mode.
        private Dictionary<string, Sprite> SpawnSprites = new Dictionary<string, Sprite>()
        {
            { "Kirby" , SpriteFactory.Instance.CreateSprite("kirby0_normal_standing_right") },
            { "WaddleDee" , SpriteFactory.Instance.CreateSprite("waddledee_walking_left") },
            { "WaddleDoo" , SpriteFactory.Instance.CreateSprite("waddledoo_walking_left") },
            { "BrontoBurt" , SpriteFactory.Instance.CreateSprite("brontoburt_standing_left") },
            { "PoppyBrosJr" , SpriteFactory.Instance.CreateSprite("poppybrosjr_hop_left") },
            { "Sparky" , SpriteFactory.Instance.CreateSprite("sparky_standing_left") },
            { "Hothead" , SpriteFactory.Instance.CreateSprite("hothead_walking_left") },
        };

        public BaseGameState(Level _level)
        {
            _game = Game1.Instance;
            _manager = Game1.Instance.manager;
            level = _level;
            DoorStarsSprite = SpriteFactory.Instance.CreateSprite("doorstars");
            TileSprites = LoadTileSprites(Constants.Filepaths.TileSpriteList);
            // LevelLoader.Instance.LoadKeymap("keymap1");

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Camera camera = _game.cameras[_game.CurrentCamera];
            if (Game1.Instance.DEBUG_LEVEL_MODE)
            {
                DebugDraw(spriteBatch, camera);
            }
            else
            {
                DrawBackground(spriteBatch, camera);
                DrawForeground(spriteBatch);
                DrawDoorStars(spriteBatch);
                DrawLevelObjects(spriteBatch);
                foreach (IPlayer player in _manager.Players) player.Draw(spriteBatch);
            }
        }

        public virtual void Update()
        {
            level.CurrentRoom.ForegroundSprite.Update();
            DoorStarsSprite.Update();
            foreach (Enemy enemy in _manager.Enemies)
            {
                enemy.Update(_game.time);
            }
            foreach (PowerUp powerUp in Game1.Instance.Level.powerUpList)
            {
                powerUp.Update();
            }
            foreach (IPlayer player in _manager.Players) player.Update(Game1.Instance.time);
        }

        public void DrawBackground(SpriteBatch spriteBatch, Camera _camera)
        {
            if (level.CurrentRoom.BackgroundSprite != null)
            {
                Vector2 cameraPosition = new Vector2(
                    _camera.GetPosition().X * (1),
                    _camera.GetPosition().Y * (1)
                );

                Vector2 backgroundScreenPosition = new Vector2(
                    _camera.GetPosition().X * ((float)(_camera.bounds.Width - level.CurrentRoom.BackgroundSprite.Width) / (level.CurrentRoom.Width - _camera.bounds.Width)),
                    _camera.GetPosition().Y * ((float)(_camera.bounds.Height - level.CurrentRoom.BackgroundSprite.Height) / (level.CurrentRoom.Height - _camera.bounds.Height))
                );

                Vector2 backgroundPosition = cameraPosition + backgroundScreenPosition;
                level.CurrentRoom.BackgroundSprite.Draw(backgroundPosition, spriteBatch);
            }
        }

        public virtual void SelectQuitButton()
        {

        }

        public virtual void SelectContinueButton()
        {

        }

        public virtual void SelectButton()
        {

        }

        // only draw foreground is not implemented in the base class 
        public void DrawForeground(SpriteBatch spriteBatch)
        {
            if (level.CurrentRoom.ForegroundSprite != null)
            {
                level.CurrentRoom.ForegroundSprite.Draw(Vector2.Zero, spriteBatch);
            }
        }

        // draws enemies and tomatoes
        public void DrawLevelObjects(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in _manager.Enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (PowerUp powerUp in Game1.Instance.Level.powerUpList)
            {
                powerUp.Draw(spriteBatch);
            }
        }

        // Draws the stars around each door
        public void DrawDoorStars(SpriteBatch spriteBatch)
        {
            int door_num = 0;
            foreach (Door door in level.CurrentRoom.Doors)
            {
                Vector2 doorPos = door.Bounds.Location.ToVector2();
                if (door.DrawDoorStars)
                {
                    DoorStarsSprite.Draw(doorPos, spriteBatch);
                }
                else
                {
                    // add behavior for drawing hub doors
                    if(door_num % 2 == 0)
                    {
                        DrawHubDoor(doorPos, door_num, spriteBatch);
                        DrawDoorSign(doorPos, door_num, spriteBatch);
                    }
                }
                door_num++;
            }
        }

        private void DrawDoorSign(Vector2 position, int door_number, SpriteBatch spriteBatch)
        {
            ISprite signSprite = SpriteFactory.Instance.CreateSprite("door_sign_number" + door_number);
            signSprite.Draw(position + drawHubSignOffset, spriteBatch);
        }

        public void DrawHubDoor(Vector2 position, int door_num, SpriteBatch spriteBatch)
        {
            ISprite doorSprite = SpriteFactory.Instance.CreateSprite(HubDoors[door_num]);
            doorSprite.Draw(position + drawHubDoorOffset, spriteBatch);
        }

        public void UpdateHubDoor()
        {
            
        }

        public void DebugDraw(SpriteBatch spriteBatch, Camera camera)
        {
            DrawBackground(spriteBatch, camera);
            DrawTiles(spriteBatch, camera);
            DrawDoorStars(spriteBatch);
            DrawDoors(spriteBatch);
            DrawSpawnPoints(spriteBatch);
            DrawLevelObjects(spriteBatch);
            foreach (IPlayer player in _manager.Players) player.Draw(spriteBatch);
        }

        // Draws a rectangle at every door with its destination room written above
        private void DrawDoors(SpriteBatch spriteBatch)
        {
            foreach (Door door in level.CurrentRoom.Doors)
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

            Vector2 kirbyPos = level.CurrentRoom.SpawnPoint;
            SpawnSprites["Kirby"].Draw(kirbyPos, spriteBatch, translucent);

            // Draw each enemy spawn point
            foreach (EnemyData enemy in level.CurrentRoom.Enemies)
            {
                Vector2 enemyPos = enemy.SpawnPoint;
                SpawnSprites[enemy.EnemyType].Draw(enemyPos, spriteBatch, translucent);
            }

            // Restore old sprite debug mode state.
            Game1.Instance.DEBUG_SPRITE_MODE = old_DEBUG_SPRITE_MODE;
        }

        // Draw visualizations of all the usually-invisible collision tiles.
        private void DrawTiles(SpriteBatch spriteBatch, Camera _camera)
        {
            // Temporarily disable sprite debug mode if it's on. Sprite debug with debug tiles makes the screen look very messy, it's not useful information. This feels like a sloppy solution but it works for now.
            bool old_DEBUG_SPRITE_MODE = Game1.Instance.DEBUG_SPRITE_MODE;
            Game1.Instance.DEBUG_SPRITE_MODE = false;

            Game1.Instance.DEBUG_LEVEL_MODE = true;

            // Set bounds on the TileMap to iterate from
            int TopY, BottomY, LeftX, RightX;
            if (Game1.Instance.CULLING_ENABLED)
            {
                TopY = Math.Max(_camera.GetBounds().Top / Constants.Level.TILE_SIZE, 0);
                BottomY = Math.Min(_camera.GetBounds().Bottom / Constants.Level.TILE_SIZE + 1, level.CurrentRoom.TileHeight);
                LeftX = Math.Max(_camera.GetBounds().Left / Constants.Level.TILE_SIZE, 0);
                RightX = Math.Min(_camera.GetBounds().Right / Constants.Level.TILE_SIZE + 1, level.CurrentRoom.TileWidth);
            }
            else
            {
                TopY = 0;
                BottomY = level.CurrentRoom.TileHeight;
                LeftX = 0;
                RightX = level.CurrentRoom.TileWidth;
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
                    DrawTile(spriteBatch, level.CurrentRoom.TileMap[y][x], new Vector2(x * Constants.Level.TILE_SIZE, y * Constants.Level.TILE_SIZE));
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

