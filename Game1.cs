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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Xml.Linq;
using KirbyNightmareInDreamLand.Audio;
using Microsoft.Xna.Framework.Input;
using KirbyNightmareInDreamLand.GameState;
using KirbyNightmareInDreamLand.Particles;

namespace KirbyNightmareInDreamLand
{
    public sealed class Game1 : Game
    {
        public SpriteBatch _spriteBatch { get; private set; }
        public ObjectManager manager;
        
        public GraphicsDeviceManager Graphics { get; private set; }
        public KeyboardController Keyboard { get; private set; }
        public GamepadController Gamepad { get; private set; }
        public MouseController MouseController { get; private set; }

        private HUD hud;

        public Camera[] cameras;
        public int ActiveCameraCount;
        public int CurrentCamera;

        public Level Level { get; private set; }

        public GameOverLay gameOverLay; 

        public GamePlayingState GameState;

        public SoundInstance music;

        // Sets up single reference for game time for things such as commands which cannot get current time elsewise
        // Note this is program time and not game time 
        public GameTime time { get; set; }

        public static GameTime GameTime { get; private set; }

        public Stopwatch TickStopwatch { get; private set; } = new Stopwatch();

        public int UpdateCounter { get; private set; }


        // Graphics settings modifiable at runtime
        public bool DEBUG_TEXT_ENABLED { get; set; }
        public bool DEBUG_SPRITE_MODE { get; set; }
        public bool DEBUG_LEVEL_MODE { get; set; }
        public bool CULLING_ENABLED { get; set; }
        public bool DEBUG_COLLISION_MODE { get; set; }
        public bool SPLITSCREEN_MODE { get; set; }
        public bool IS_FULLSCREEN { get; set; }
        public int WINDOW_WIDTH { get; set; }
        public int WINDOW_HEIGHT { get; set; }
        public int WINDOW_XOFFSET { get; set; }
        public int WINDOW_YOFFSET { get; set; }
        public int MAX_WINDOW_WIDTH { get; set; }
        public int MAX_WINDOW_WIDTH_SPLITSCREEN { get; set; }
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
            DEBUG_TEXT_ENABLED = false;
            DEBUG_SPRITE_MODE = false;
            DEBUG_LEVEL_MODE = false; // TODO: Change to false by default later, currently no normal level draw behavior
            CULLING_ENABLED = true;
            DEBUG_COLLISION_MODE = false;
            SPLITSCREEN_MODE = false;
            IS_FULLSCREEN = false;
            WINDOW_WIDTH = Constants.Graphics.GAME_WIDTH * 4;
            WINDOW_HEIGHT = Constants.Graphics.GAME_HEIGHT * 4;
            WINDOW_XOFFSET = 0;
            WINDOW_YOFFSET = 0;
            TARGET_FRAMERATE = 60;

            UpdateCounter = 0;

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
            Gamepad = new GamepadController();
            MouseController = new MouseController();

            cameras = new Camera[Constants.Game.MAXIMUM_PLAYER_COUNT];
            CurrentCamera = 0;

            GamePad.InitDatabase();
            
            SoundEffect.Initialize();

            base.Initialize();

            gameOverLay = new GameOverLay();
        }

        protected override void LoadContent()
        {
            System.Diagnostics.Debug.WriteLine("Debug from content load");

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            manager = ObjectManager.Instance;

            // Load all content through LevelLoader
            LevelLoader.Instance.LoadAllContent();

            // Load player
            LevelLoader.Instance.LoadKirby();

            // Create camera instances
            for (int i = 0; i < Constants.Game.MAXIMUM_PLAYER_COUNT; i++)
            {
                cameras[i] = new Camera(i);
            }

            // Create level instance and load initial room
            Level = new Level();
            Level.LoadRoom("room1");

            // Load the desired keymap by name
            LevelLoader.Instance.LoadKeymap("keymap1");
            LevelLoader.Instance.LoadButtonmap("buttonmap1");

            music = SoundManager.CreateInstance("song_vegetablevalley");
            music?.Play();

            hud = new HUD(manager.kirby);
        }



        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            time = gameTime;

            GameDebug.Instance.ResetCounters();

            // Reset timer for calculating max fps
            TickStopwatch.Restart();

            // can put in a list of controllers and update in foreach 
            Keyboard.Update();
            Gamepad.Update();
            MouseController.Update();

            ActiveCameraCount = manager.Players.Count;
            GameTime = gameTime;

            Level.UpdateLevel();
            
            manager.ResetDebugStaticObjects();
            manager.OrganizeList();


            CollisionDetection.Instance.CheckCollisions();

            // Update all active cameras
            for (int i = 0; i < Constants.Game.MAXIMUM_PLAYER_COUNT; i++)
            {
                cameras[i].Update();
            }

            foreach (IParticle particle in manager.Particles) particle.Update();

            manager.UpdateParticles();

            
            SoundManager.Update();
            //_transitioning.Update();

            UpdateCounter++;
        }

        

        private void DrawView(Rectangle bounds, Camera camera)
        {
            // Level spritebatch
            RasterizerState rasterizerState = new RasterizerState { ScissorTestEnable = true };
            GraphicsDevice.ScissorRectangle = bounds;
            
            Matrix viewMatrix = Matrix.CreateScale((float)bounds.Width / Constants.Graphics.GAME_WIDTH) * Matrix.CreateTranslation(bounds.X, bounds.Y, 0);
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, rasterizerState, null, camera.LevelMatrix * viewMatrix);
            // Draw level
            Level.Draw(_spriteBatch);

            // Draw kirby
            //foreach(IPlayer player in manager.Players) player.Draw(_spriteBatch);

            if (Level.IsCurrentState("KirbyNightmareInDreamLand.GameState.GameTransitioningState"))
            {
                gameOverLay.DrawFade(_spriteBatch, Level.FadeAlpha);
            }

            // Draw particles
            foreach (IParticle particle in manager.Particles) particle.Draw(_spriteBatch);

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
            // Temporarily disable culling for the static spritebatch, LAZY FIX, WILL IMPLEMENT PROPER FIX LATER -Mark
            bool old_CULLING_ENABLED = CULLING_ENABLED;
            CULLING_ENABLED = false;

            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, rasterizerState, null, viewMatrix);
            hud.Draw(_spriteBatch);
            _spriteBatch.End();
            // Restore old culling mode
            CULLING_ENABLED = old_CULLING_ENABLED;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);


            if (SPLITSCREEN_MODE)
            {
                for (int i = 0; i < Constants.Game.MAXIMUM_PLAYER_COUNT; i++)
                {
                    Rectangle bounds = new Rectangle(
                        WINDOW_XOFFSET + WINDOW_WIDTH / 2 * (i % 2),
                        WINDOW_YOFFSET + WINDOW_HEIGHT / 2 * (i / 2),
                        WINDOW_WIDTH / 2,
                        WINDOW_HEIGHT / 2
                    );
                    if (i < ActiveCameraCount)
                    {
                        CurrentCamera = i;
                        DrawView(bounds, cameras[i]);
                    }
                    else
                    {
                        GameDebug.Instance.DrawEmptyPlayerScreen(_spriteBatch, bounds);
                    }
                }
            }
            else
            {
                Rectangle bounds = new Rectangle(WINDOW_XOFFSET, WINDOW_YOFFSET, WINDOW_WIDTH, WINDOW_HEIGHT);
                CurrentCamera = 0;
                DrawView(bounds, cameras[0]);
            }

            
            // Draw Debug Text
            if (DEBUG_TEXT_ENABLED)
            {
                GameDebug.Instance.DrawDebugText(_spriteBatch);
            }

            // Draw borders (should only be visible in fullscreen for letterboxing)
            GameDebug.Instance.DrawBorders(_spriteBatch);

            // Stop timer for calculating max fps
            TickStopwatch.Stop();
        }

    }

}
