using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Controllers;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Collision;
using System;
using System.Diagnostics;
using System.Linq;
using KirbyNightmareInDreamLand.UI;

namespace KirbyNightmareInDreamLand
{
    public class Game1 : Game
    {
        public SpriteBatch _spriteBatch;
        public ObjectManager manager;
        
        public GraphicsDeviceManager Graphics { get; private set; }
        public KeyboardController Keyboard { get; private set; }
        public MouseController MouseController { get; private set; }

        private HUD hud;

        // Camera instance for the game
        public Camera Camera { get; private set; }

        public Level Level { get; private set; }

        // Sets up single reference for game time for things such as commands which cannot get current time elsewise
        // Note this is program time and not game time 
        public GameTime time { get; set; }

        public static GameTime GameTime { get; private set; }

        public Stopwatch TickStopwatch { get; private set; } = new Stopwatch();


        // Graphics settings modifiable at runtime
        public bool DEBUG_TEXT_ENABLED { get; set; }
        public bool DEBUG_SPRITE_MODE { get; set; }
        public bool DEBUG_LEVEL_MODE { get; set; }
        public bool CULLING_ENABLED { get; set; }
        public bool DEBUG_COLLISION_MODE { get; set; }
        public bool IS_FULLSCREEN { get; set; }
        public int WINDOW_WIDTH { get; set; }
        public int WINDOW_HEIGHT { get; set; }
        public int WINDOW_XOFFSET { get; set; }
        public int WINDOW_YOFFSET { get; set; }
        public int MAX_WINDOW_WIDTH { get; set; }
        public int TARGET_FRAMERATE { get; set; }
        public bool PAUSED = false;

        private static Game1 _instance;
        public static Game1 Instance
        {
            get
            {
                return _instance;
            }
        }
        public Game1()
        {
            _instance = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        protected override void Initialize()
        {
            DEBUG_TEXT_ENABLED = true;
            DEBUG_SPRITE_MODE = false;
            DEBUG_LEVEL_MODE = false; // TODO: Change to false by default later, currently no normal level draw behavior
            CULLING_ENABLED = true;
            DEBUG_COLLISION_MODE = false;
            IS_FULLSCREEN = false;
            WINDOW_WIDTH = Constants.Graphics.GAME_WIDTH * 3;
            WINDOW_HEIGHT = Constants.Graphics.GAME_HEIGHT * 3;
            WINDOW_XOFFSET = 0;
            WINDOW_YOFFSET = 0;
            TARGET_FRAMERATE = 60;

            TargetElapsedTime = TimeSpan.FromMilliseconds(1000f / TARGET_FRAMERATE);

            #region set max window width
            int displayWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int displayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            float displayAspectRatio = (float)displayWidth / displayHeight;
            float gameAspectRatio = (float)Constants.Graphics.GAME_WIDTH / Constants.Graphics.GAME_HEIGHT;
            // Set max width such that the window can be increased to as large as it can fit in the display, but no further.
            if (displayAspectRatio > gameAspectRatio) // If display aspect ratio is wider than game aspect ratio (3:2)
            {
                // Base max width on display height
                MAX_WINDOW_WIDTH = (displayHeight / Constants.Graphics.GAME_HEIGHT * Constants.Graphics.GAME_HEIGHT * Constants.Graphics.GAME_WIDTH / Constants.Graphics.GAME_HEIGHT);
            }
            else
            {
                // Base max width on display width
                MAX_WINDOW_WIDTH = displayWidth / Constants.Graphics.GAME_WIDTH * Constants.Graphics.GAME_WIDTH;
            }
            #endregion

            // true = exclusive fullscreen, false = borderless fullscreen
            Graphics.HardwareModeSwitch = true;
            Graphics.IsFullScreen = IS_FULLSCREEN;
            Graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            Graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Graphics.ApplyChanges();

            Keyboard = new KeyboardController();
            MouseController = new MouseController();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            System.Diagnostics.Debug.WriteLine("Debug from content load");

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            manager = ObjectManager.Instance;
            // Create camera instance
            Camera = new Camera();

            // Load all content through LevelLoader
            LevelLoader.Instance.LoadAllContent();

            // Load all objects
            manager.LoadKirby();

            // Create level instance and load initial room
            Level = new Level();
            Level.LoadRoom("room1");

            // Load the desired keymap by name
            LevelLoader.Instance.LoadKeymap("keymap1");

            hud = new HUD();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if(!PAUSED)
            {
                base.Update(gameTime);
                time = gameTime;

                // Reset timer for calculating max fps
                TickStopwatch.Restart();

                Keyboard.Update();
                MouseController.Update();

                GameTime = gameTime;

                foreach(IPlayer player in manager.Players) player.Update(time);

                Level.UpdateLevel();

                ObjectManager.Instance.OrganizeList();

                CollisionDetection.Instance.CheckCollisions();

                Camera.Update();
            } else {
                Keyboard.PausedUpdate();
            }
           
        }



        protected override void Draw(GameTime gameTime)
        {
            if(!PAUSED) {
                GraphicsDevice.Clear(Color.White);
                base.Draw(gameTime);

                // Level spritebatch
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.LevelMatrix);
                // Draw level
                Level.Draw(_spriteBatch);

                // Draw kirby
                foreach(IPlayer player in manager.Players) player.Draw(_spriteBatch);

                // Not currently using item
                // item.Draw(new Vector2(200, 150), spriteBatch);
                if (DEBUG_COLLISION_MODE)
                {
                    CollisionDetection.Instance.DebugDraw(_spriteBatch);
                }

                // Draws the debug position log
                GameDebug.Instance.DrawPositionLog(_spriteBatch, Color.Red, 1.0f);

                _spriteBatch.End();
                
                // Static spritebatch
                _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.ScreenMatrix);
                hud.Draw(_spriteBatch);
                _spriteBatch.End();

                // Stop timer for calculating max fps
                TickStopwatch.Stop();
                
                // Draw Debug Text
                if (DEBUG_TEXT_ENABLED)
                {
                    GameDebug.Instance.DrawDebugText(_spriteBatch);
                    manager.ResetDebugStaticObjects();
                }
                // Draw borders (should only be visible in fullscreen for letterboxing)
                GameDebug.Instance.DrawBorders(_spriteBatch);

                //manager.UpdateObjectLists();
            } else {
                _spriteBatch.Begin();
                GraphicsDevice.Clear(Color.CornflowerBlue);
                base.Draw(gameTime);
                _spriteBatch.End();
            }
        }

    }

}
