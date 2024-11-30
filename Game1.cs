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
        private SpriteBatch _spriteBatch;
        public ObjectManager manager;
        
        public GraphicsDeviceManager Graphics { get; private set; }
        public KeyboardController Keyboard { get; private set; }
        public GamepadController Gamepad { get; private set; }
        public MouseController MouseController { get; private set; }

        private HUD[] huds;

        public Camera[] cameras;
        public int ActiveCameraCount;
        public int CurrentCamera;

        public Level Level { get; private set; }

        public GameOverLay gameOverLay; 

        public GamePlayingState GameState;

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
        public bool DEBUG_COLLISION_MODE { get; set; }
        public bool DEBUG_ZOOM_MODE { get; set; }
        public bool CULLING_ENABLED { get; set; }
        public bool SPLITSCREEN_MODE { get; set; }
        public bool IS_FULLSCREEN { get; set; }
        public int WINDOW_WIDTH { get; set; }
        public int WINDOW_HEIGHT { get; set; }
        public int WINDOW_XOFFSET { get; set; }
        public int WINDOW_YOFFSET { get; set; }
        public int MAX_WINDOW_WIDTH { get; set; }
        public int MAX_WINDOW_WIDTH_SPLITSCREEN { get; set; }
        public bool SPLITSCREEN_AVAILABLE { get; set; }
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
            DEBUG_COLLISION_MODE = false;
            DEBUG_ZOOM_MODE = false;
            CULLING_ENABLED = true;
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
            huds = new HUD[Constants.Game.MAXIMUM_PLAYER_COUNT];
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
                huds[i] = new HUD(i);
            }

            // Create level instance and load initial room
            Level = new Level();
            Level.LoadRoom("hub");

            // Load the desired keymap by name
            LevelLoader.Instance.LoadKeymap("main");
            LevelLoader.Instance.LoadButtonmap("main");
            
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
                huds[i].Update();
            }
            
            SoundManager.Update();
            //_transitioning.Update();

            //Debug.WriteLine("CurrentCamera game state = " + Level._currentState);

            UpdateCounter++;
        }

        private RasterizerState[] rasterizerStates = { new RasterizerState { ScissorTestEnable = false } , new RasterizerState { ScissorTestEnable = true } };
        private void DrawView(Rectangle bounds)
        {
            // Level spritebatch
            GraphicsDevice.ScissorRectangle = bounds;
            RasterizerState rasterizerState = DEBUG_ZOOM_MODE && !SPLITSCREEN_MODE ? rasterizerStates[0] : rasterizerStates[1];
            Matrix viewMatrix = Matrix.CreateScale((float)bounds.Width / Constants.Graphics.GAME_WIDTH) * Matrix.CreateTranslation(bounds.X, bounds.Y, 0);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, rasterizerState, null, cameras[CurrentCamera].LevelMatrix * viewMatrix);
            
            Level.Draw(_spriteBatch);

            if (Level.IsCurrentState("KirbyNightmareInDreamLand.GameState.GameTransitioningState"))
            {
                gameOverLay.DrawFade(_spriteBatch, Level.FadeAlpha);
            }

            if (DEBUG_COLLISION_MODE)
            {
                CollisionDetection.Instance.DebugDraw(_spriteBatch);
            }

            // Draws the debug position log
            GameDebug.Instance.DrawPositionLog(_spriteBatch, Color.Red, 1.0f);

            if (DEBUG_ZOOM_MODE)
            {
                for (int i = 0; i < ActiveCameraCount; i++)
                {
                    GameDebug.Instance.DrawRectangle(_spriteBatch, cameras[i].GetBounds(), Color.Lime, 1f);
                    GameDebug.Instance.DrawRectangle(_spriteBatch, cameras[i].GetEnemyBounds(), Color.Red, 1f);
                }
                #region Debug code to draw enemy respawn bounds
                //for (int x = cameras[CurrentCamera].bounds.Left - 120; x < cameras[CurrentCamera].bounds.Right + 120; x += 2)
                //{
                //    for (int y = cameras[CurrentCamera].bounds.Top - 80; y < cameras[CurrentCamera].bounds.Bottom + 80; y += 2)
                //    {
                //        Vector2 position = new Vector2(x, y);
                //        bool test = Camera.InAnyEnemyRespawnBounds(position);
                //        Color color = test ? Color.LawnGreen : Color.Red;
                //        float alpha = test ? 1f : 0.25f;
                //        GameDebug.Instance.DrawPoint(_spriteBatch, position, color, alpha);
                //    }
                //}
                #endregion
            }
            _spriteBatch.End();

            // Static spritebatch
            // Temporarily disable culling for the static spritebatch, LAZY FIX, WILL IMPLEMENT PROPER FIX LATER -Mark
            bool old_CULLING_ENABLED = CULLING_ENABLED;
            CULLING_ENABLED = false;
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, rasterizerState, null, viewMatrix);

            huds[CurrentCamera].Draw(_spriteBatch);
            
            _spriteBatch.End();
            // Restore old culling mode
            CULLING_ENABLED = old_CULLING_ENABLED;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);


            if (SPLITSCREEN_MODE && SPLITSCREEN_AVAILABLE)
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
                        DrawView(bounds);
                    }
                    else
                    {
                        GameDebug.Instance.DrawEmptyPlayerScreen(_spriteBatch, bounds);
                    }
                }
            }
            else if (DEBUG_ZOOM_MODE)
            {
                Rectangle bounds = new Rectangle(
                    WINDOW_XOFFSET + WINDOW_WIDTH / 4,
                    WINDOW_YOFFSET + WINDOW_HEIGHT / 4,
                    WINDOW_WIDTH / 2,
                    WINDOW_HEIGHT / 2);
                CurrentCamera = 0;
                DrawView(bounds);
            }
            else
            {
                Rectangle bounds = new Rectangle(WINDOW_XOFFSET, WINDOW_YOFFSET, WINDOW_WIDTH, WINDOW_HEIGHT);
                CurrentCamera = 0;
                DrawView(bounds);
            }


            //Rectangle bounds1 = new Rectangle(
            //        WINDOW_XOFFSET,
            //        WINDOW_YOFFSET,
            //        WINDOW_WIDTH,
            //        WINDOW_HEIGHT);
            //int x = Mouse.GetState().X * 255 / WINDOW_WIDTH;

            //BlendState blendState = new BlendState
            //{
            //    ColorSourceBlend = Blend.One,
            //    AlphaSourceBlend = Blend.One,
                
            //    ColorDestinationBlend = Blend.InverseSourceAlpha,
            //    AlphaDestinationBlend = Blend.InverseSourceAlpha,

            //    ColorBlendFunction = BlendFunction.ReverseSubtract,
            //    AlphaBlendFunction = BlendFunction.Add
            //};

            //_spriteBatch.Begin(SpriteSortMode.Deferred, blendState, SamplerState.PointClamp, null, null, null, null);
            ////GameDebug.Instance.DrawSolidRectangle(_spriteBatch, bounds1, new Color(x, x, x, 0), 1f);
            //GameDebug.Instance.DrawSolidRectangle(_spriteBatch, bounds1, new Color(x, x, x, 0), 1f);
            //_spriteBatch.End();

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