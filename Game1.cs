using KirbyNightmareInDreamLand.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Controllers;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Diagnostics;
using KirbyNightmareInDreamLand.Collision;
using KirbyNightmareInDreamLand.Entities;
using KirbyNightmareInDreamLand.Actions;

namespace KirbyNightmareInDreamLand
{
    public class Game1 : Game
    {
        private SpriteBatch spriteBatch;
        
        public GraphicsDeviceManager graphics { get; private set; }
        public KeyboardController keyboard;
        private MouseController mouseController;

        // Camera instance for the game
        public Camera camera { get; private set; }

        public Level level { get; private set; }


        // Sets up single reference for game time for things such as commands which cannot get current time elsewise
        // Note this is program time and not game time 
        public GameTime time { get; set; }

        public static GameTime GameTime { get; private set; }

        public Stopwatch TickStopwatch { get; private set; } = new Stopwatch();

        public ObjectManager objectManager { get; private set; }


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

        private static Game1 instance;
        public static Game1 Instance
        {
            get
            {
                return instance;
            }
        }
        public Game1()
        {
            instance = this;
            graphics = new GraphicsDeviceManager(this);
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
            graphics.HardwareModeSwitch = true;
            graphics.IsFullScreen = IS_FULLSCREEN;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.ApplyChanges();

            keyboard = new KeyboardController();
            mouseController = new MouseController();

            objectManager = ObjectManager.Instance;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            System.Diagnostics.Debug.WriteLine("Debug from content load");




            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create camera instance
            camera = new Camera();

            // Load all content through LevelLoader
            LevelLoader.Instance.LoadAllContent();

            // Load all objects
            ObjectManager.Instance.LoadObjects();

            // Create level instance and load initial room
            level = new Level();
            level.LoadRoom("room1");

            

            // Load the desired keymap by name
            LevelLoader.Instance.LoadKeymap("keymap1");
        }



        protected override void UnloadContent()
        {

        }

        List<IEnemy> enemyList2 = new List<IEnemy>(); // FOR PERFORMANCE TESTING
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            time = gameTime;

            // Reset timer for calculating max fps
            TickStopwatch.Restart();

            keyboard.Update();
            mouseController.Update();

            GameTime = gameTime;

            CollisionDetection.Instance.CheckCollisions();

            foreach (IPlayer player in objectManager.players) player.Update(time);
            objectManager.enemyList[objectManager.currentEnemyIndex].Update(time);

            // Commented out since we currently do not need item
            // item.Update();

            //enemyList2.Add(new Hothead(new Vector2(170, 100))); // FOR PERFORMANCE TESTING
            foreach (IEnemy enemy in enemyList2) enemy.Update(time); // FOR PERFORMANCE TESTING

            level.UpdateLevel();

            camera.Update();
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);

            // Level spritebatch
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.LevelMatrix);
            // Draw level
            level.Draw(spriteBatch);
            // Draw only selected enemy
            // enemyList[currentEnemyIndex].Draw(spriteBatch);
            foreach (IEnemy enemy in enemyList2) enemy.Draw(spriteBatch); // FOR PERFORMANCE TESTING

            // Draw kirby
            foreach(IPlayer player in objectManager.players) player.Draw(spriteBatch);

            // Not currently using item
            // item.Draw(new Vector2(200, 150), spriteBatch);
            if (DEBUG_COLLISION_MODE)
            {
                CollisionDetection.Instance.DebugDraw(spriteBatch);
            }
            spriteBatch.End();

            // Static spritebatch
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.ScreenMatrix);
            spriteBatch.End();

            // Stop timer for calculating max fps
            TickStopwatch.Stop();
            
            // Draw Debug Text
            if (DEBUG_TEXT_ENABLED)
            {
                GameDebug.Instance.DrawDebugText(spriteBatch);
            }
            // Draw borders (should only be visible in fullscreen for letterboxing)
            GameDebug.Instance.DrawBorders(spriteBatch);
        }

    }
}
