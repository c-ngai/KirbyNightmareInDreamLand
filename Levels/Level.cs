using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.PowerUps;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace KirbyNightmareInDreamLand.Levels
{

    public class Level
    {

        private readonly Game1 _game;
        private readonly Camera _camera;

        public float BackgroundParallaxFactor = Constants.Graphics.PARALLAX_FACTOR;

        public string EnemyNamespace = Constants.Namespaces.ENEMY_NAMESPACE;
        public string PowerUpNamespace = Constants.Namespaces.POWERUP_NAMESPACE;

        public Room CurrentRoom { get; private set; }

        public Vector2 SpawnPoint { get; private set; }
      
        private SpriteBatch spriteBatch;

        private List<Enemy> enemyList;

        private List<PowerUp> powerUpList;

        private List<Sprite> TileSprites;

        private Sprite _doorstarsSprite;

        private ObjectManager manager = ObjectManager.Instance;

        public bool LevelPaused;

        public struct RoomChangeData
        {
            public bool ChangeRoom;
            public string DestinationRoom;
            public Vector2 DestinationPoint;
            public bool FadeInComplete;
            public bool FadeOutComplete;
            public bool CurrentlyFadingOut;
            public bool CurrentlyFadingIn;
            public bool CurrentlyTransitioning;
            public float FadeSpeed;
            public float FadeAlpha;
        }

        public struct GameOverData
        {
            public bool GameOver;
            public bool CurrentlyTransitioning;
            public float FadeSpeed;
            public float FadeAlpha;
        }

        private RoomChangeData _roomChangeData;

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

            spriteBatch = Game1.Instance._spriteBatch;

            TileSprites = LoadTileSprites(Constants.Filepaths.TileSpriteList);
            _doorstarsSprite = SpriteFactory.Instance.CreateSprite("doorstars");

            LevelPaused = false;

            _roomChangeData = new RoomChangeData
            {
                ChangeRoom = false,
                DestinationRoom = null,
                DestinationPoint = Vector2.Zero,
                CurrentlyFadingIn = false,
                CurrentlyFadingOut= false,
                CurrentlyTransitioning = false,
                FadeSpeed = 0.05f,
                FadeAlpha = 0f
             };
        }

        public Vector2 convertTileToPixel(Vector2 tilePosition)
        {
            return new Vector2(tilePosition.X * Constants.Level.TILE_SIZE, tilePosition.Y * Constants.Level.TILE_SIZE);
        }

        #region Loading

        // Loads a room into the level by name, specifying a spawn point. (for entering from a door)
        public void LoadRoom(string RoomName, Vector2? _spawnPoint)
        {
            if (LevelLoader.Instance.Rooms.ContainsKey(RoomName))
            {
                // Sets it up so player will not be incorrectly removed during room changes
                manager.ResetDynamicCollisionBoxes();
                manager.ResetStaticObjects();
                CurrentRoom = LevelLoader.Instance.Rooms[RoomName];
                LoadLevelObjects();
                SpawnPoint = _spawnPoint ?? CurrentRoom.SpawnPoint;
                foreach (IPlayer player in manager.Players)
                {
                    player.GoToRoomSpawn();
                    manager.RegisterDynamicObject((Player)player);
                }
            }
            else
            {
                Debug.WriteLine("ERROR: \"" + RoomName + "\" is not a valid room name and cannot be loaded.");
            }
        }

        // Overflow method. If no spawn point is specified, the level will load the room's default. TODO: refactor this?? does this implementation suck?????
        public void LoadRoom(string RoomName)
        {
            LoadRoom(RoomName, null);
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

        #endregion

        #region Drawing
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_game.DEBUG_LEVEL_MODE || CurrentRoom.Name == "treasureroom")
            {
                DrawDebug(spriteBatch);
            }
            else
            {
                DrawLevel(spriteBatch);
            }
        }

        // Draws the level normally; background and foreground.
        public void DrawLevel(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            DrawForeground(spriteBatch);
            DrawDoorStars(spriteBatch);
            DrawLevelObjects(spriteBatch);
            if (_roomChangeData.CurrentlyFadingOut)
            {
                FadeOut();
            }
            if (_roomChangeData.CurrentlyFadingIn)
            {
                FadeIn();
            }
            if (LevelPaused)
            {
                DrawPauseScreen();
            }
        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            if (CurrentRoom.BackgroundSprite != null)
            {
                Vector2 cameraPosition = new Vector2(
                    _camera.GetPosition().X * (1),
                    _camera.GetPosition().Y * (1)
                );
                
                Vector2 backgroundScreenPosition = new Vector2(
                    _camera.GetPosition().X * ((float)(_camera.bounds.Width - CurrentRoom.BackgroundSprite.Width) / (CurrentRoom.Width - _camera.bounds.Width)),
                    _camera.GetPosition().Y * ((float)(_camera.bounds.Height - CurrentRoom.BackgroundSprite.Height) / (CurrentRoom.Height - _camera.bounds.Height))
                );

                Vector2 backgroundPosition = cameraPosition + backgroundScreenPosition;
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

        // draws enemies and tomatoes
        public void DrawLevelObjects(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemyList)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (PowerUp powerUp in powerUpList)
            {
                powerUp.Draw(spriteBatch);
            }
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

        // Draws the stars around each door
        private void DrawDoorStars(SpriteBatch spriteBatch)
        {
            foreach (Door door in CurrentRoom.Doors)
            {
                Vector2 doorPos = door.Bounds.Location.ToVector2();
                _doorstarsSprite.Draw(doorPos, spriteBatch);
            }
        }
        #endregion

        #region LocalObjectManagement

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

        // gets called when player uses a power up so it doesn't get drawn anymore
        public void removePowerUpFromList(PowerUp powerUp)
        {
            powerUpList.Remove(powerUp);
        }

        #endregion

        #region Go between rooms 

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
        public void EnterDoorAt(Vector2 playerPos)
        {
            foreach (Door door in CurrentRoom.Doors)
            {
                if (door.Bounds.Contains(playerPos))
                {
                    _roomChangeData.DestinationRoom = door.DestinationRoom;
                    _roomChangeData.DestinationPoint = door.DestinationPoint;
                    _roomChangeData.ChangeRoom = true;
                    _roomChangeData.CurrentlyFadingOut = true;
                    _roomChangeData.CurrentlyTransitioning = true;
                }
            }
        }

        public void FadeIn()
        {
             GameDebug.Instance.DrawSolidRectangle(spriteBatch, _camera.bounds, Color.White, _roomChangeData.FadeAlpha);
        }


        public void FadeOut()
        {
             GameDebug.Instance.DrawSolidRectangle(spriteBatch, _camera.bounds, Color.White, _roomChangeData.FadeAlpha);
        }

        #endregion

        #region Pause 

        public void DrawPauseScreen()
        {
            List<string> kirbyType = new List<string>();
            foreach(Player player in Game1.Instance.manager.Players)
            {
                kirbyType.Add(player.GetKirbyType());
            }
            Sprite pause_sprite = SpriteFactory.Instance.CreateSprite(kirbyType[0] + "_pause_screen");
            Sprite pause_background = SpriteFactory.Instance.CreateSprite("pause_screen_background");

            pause_background.Draw(Vector2.Zero, spriteBatch);
            pause_sprite.Draw(Vector2.Zero, spriteBatch);
        }

        public void PauseLevel()
        {
            LevelPaused = true;
        }

        public void UnpauseLevel()
        {
            LevelPaused = false;
        }

        #endregion

        #region UpdateLevel

        public void UpdateLevel()
        {

            // if we are currently fading out we want to keep fading out
            if (_roomChangeData.CurrentlyTransitioning && _roomChangeData.CurrentlyFadingOut)
            {
                _roomChangeData.FadeAlpha += _roomChangeData.FadeSpeed; // increment opacity 
                if (_roomChangeData.FadeAlpha >= 1.0f) // if we are opaque  
                {
                    _roomChangeData.FadeAlpha = 1f; // reset fadeAlpha so fade-in is ready 
                    _roomChangeData.CurrentlyFadingOut = false; // Fade-out complete
                }
            }

            // if we are transitioning and not fading out we want to use the opaque screen to load the new room 
            if (_roomChangeData.CurrentlyTransitioning && !_roomChangeData.CurrentlyFadingOut && !_roomChangeData.CurrentlyFadingIn)
            {
                LoadRoom(_roomChangeData.DestinationRoom, _roomChangeData.DestinationPoint);
                _roomChangeData.ChangeRoom = false; // we changed the room, so reset bool so we don't keep reloading the room
                _roomChangeData.CurrentlyFadingIn = true; //  Cue the fade it 
            }

            // if we are currently fading in we want to keep fading in
            if (_roomChangeData.CurrentlyTransitioning && _roomChangeData.CurrentlyFadingIn)
            {
                _roomChangeData.FadeAlpha -= _roomChangeData.FadeSpeed; // decrement opacity 
                if (_roomChangeData.FadeAlpha <= 0f) // if we are transparent 
                {
                    _roomChangeData.FadeAlpha = 0f; // reset fadeAlpha so fade-out is ready to go
                    _roomChangeData.CurrentlyFadingIn = false; // Fade-in complete
                    _roomChangeData.CurrentlyTransitioning = false; // We are done transitioning
                }

            }

            CurrentRoom.ForegroundSprite.Update();
            _doorstarsSprite.Update();
            foreach (Enemy enemy in enemyList)
            {
                enemy.Update(_game.time);
            }
            foreach (PowerUp powerUp in powerUpList)
            {
                powerUp.Update();
            }

        }

        #endregion

        #region Debug

        // Debug mode (toggle F2), draws the usually-invisible collision tiles, doors, and enemy spawn locations.
        private void DrawDebug(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
            DrawTiles(spriteBatch);
            DrawDoorStars(spriteBatch);
            DrawDoors(spriteBatch);
            DrawSpawnPoints(spriteBatch);
            DrawLevelObjects(spriteBatch);
            if (_roomChangeData.CurrentlyFadingOut)
            {
                FadeOut();
            }
            if (_roomChangeData.CurrentlyFadingIn)
            {
                FadeIn();
            }
        }

        Color translucent = new Color(127, 127, 127, 127);
        // Draws static, transparent sprites of the corresponding enemy for each enemy spawn point in the level.
        private void DrawSpawnPoints(SpriteBatch spriteBatch)
        {
            // Temporarily disable sprite debug mode if it's on.
            bool old_DEBUG_SPRITE_MODE = _game.DEBUG_SPRITE_MODE;
            _game.DEBUG_SPRITE_MODE = false;

            Vector2 kirbyPos = CurrentRoom.SpawnPoint;
            SpawnSprites["Kirby"].Draw(kirbyPos, spriteBatch, translucent);

            // Draw each enemy spawn point
            foreach (EnemyData enemy in CurrentRoom.Enemies)
            {
                Vector2 enemyPos = enemy.SpawnPoint;
                SpawnSprites[enemy.EnemyType].Draw(enemyPos, spriteBatch, translucent);
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
    }
}
        #endregion