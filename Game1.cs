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

namespace KirbyNightmareInDreamLand
{
    public class Game1 : Game
    {
        private SpriteBatch spriteBatch;
        
        public GraphicsDeviceManager graphics { get; private set; }
        private KeyboardController keyboard;
        private MouseController mouseController;

        // Camera instance for the game
        public Camera camera { get; private set; }

        public Level level { get; private set; }

        // Single-player but can later be updated to an array of kirbys for multiplayer
        private IPlayer kirby;

        // Get enemies (currently one of each but can change to an array of each enemy type)
        private IEnemy waddledeeTest;
        private IEnemy waddledooTest;
        private IEnemy brontoburtTest;
        private IEnemy hotheadTest;
        private IEnemy poppybrosjrTest;
        private IEnemy sparkyTest;

        // List of all enemies
        public IEnemy[] enemyList { get; set; }

        public Sprite item { get; set; }

        // TODO: Decoupling: move this out later
        public int currentEnemyIndex { get; set; }

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


        //why is game time public?? take out set in the game time make anybody that is not that commmand not be anle to set it
        //
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

            base.Initialize();
        }



        // Everything in this region will move into eventual loader file
        #region LoaderDetails
        // Will later be changed to read in keyboard control input 
        public void SetKeyboardControls(KeyboardController keyboard)
        {
            keyboard.RegisterCommand(Keys.Right, ExecutionType.Pressed, new KirbyMoveRightCommand(kirby, Keys.Right, keyboard));
            keyboard.RegisterCommand(Keys.Right, ExecutionType.StoppingPress, new KirbyRunningRightCommand(keyboard, Keys.Right, kirby));
            keyboard.RegisterCommand(Keys.Left, ExecutionType.Pressed, new KirbyMoveLeftCommand(kirby, Keys.Left, keyboard));
            keyboard.RegisterCommand(Keys.Left, ExecutionType.StoppingPress, new KirbyRunningLeftCommand(keyboard, Keys.Left, kirby));

            // this is hard-coded bc it needs to know the keybind to attack to check if it needs to slide
            keyboard.RegisterCommand(Keys.Down, ExecutionType.Pressed, new KirbyMoveCrouchedCommand(kirby, Keys.Down, Keys.Z, keyboard));
            keyboard.RegisterCommand(Keys.Down, ExecutionType.StoppingPress, new KirbyCrouchAndSlideCommand(kirby, Keys.Down, Keys.Z, keyboard));
            keyboard.RegisterCommand(Keys.Up, ExecutionType.Pressed, new KirbyFloatCommand(kirby));
            keyboard.RegisterCommand(Keys.X, ExecutionType.Pressed, new KirbyJumpCommand(kirby));

            keyboard.RegisterCommand(Keys.D, ExecutionType.StartingPress, new KirbyFaceRightCommand(kirby));
            keyboard.RegisterCommand(Keys.A, ExecutionType.StartingPress, new KirbyFaceLeftCommand(kirby));


            keyboard.RegisterCommand(Keys.Z, ExecutionType.StartingPress, new KirbyAttackCommand(kirby));
            keyboard.RegisterCommand(Keys.Z, ExecutionType.StoppingPress, new KirbyStopAttackingCommand(kirby));
            keyboard.RegisterCommand(Keys.Z, ExecutionType.Pressed, new KirbyAttackPressedCommand(kirby));

            keyboard.RegisterCommand(Keys.D1, ExecutionType.StartingPress, new KirbyChangeNormalCommand(kirby));
            keyboard.RegisterCommand(Keys.D2, ExecutionType.StartingPress, new KirbyChangeBeamCommand(kirby));
            keyboard.RegisterCommand(Keys.D3, ExecutionType.StartingPress, new KirbyChangeFireCommand(kirby));
            keyboard.RegisterCommand(Keys.D4, ExecutionType.StartingPress, new KirbyChangeSparkCommand(kirby));

            keyboard.RegisterCommand(Keys.O, ExecutionType.StartingPress, new PreviousEnemyCommand());
            keyboard.RegisterCommand(Keys.P, ExecutionType.StartingPress, new NextEnemyCommand());

            keyboard.RegisterCommand(Keys.Q, ExecutionType.StartingPress, new QuitCommand());
            keyboard.RegisterCommand(Keys.R, ExecutionType.StartingPress, new ResetCommand());

            keyboard.RegisterCommand(Keys.F1, ExecutionType.StartingPress, new GraphicsToggleDebugTextCommand());
            keyboard.RegisterCommand(Keys.F2, ExecutionType.StartingPress, new GraphicsToggleDebugSpriteCommand());
            keyboard.RegisterCommand(Keys.F3, ExecutionType.StartingPress, new GraphicsToggleDebugLevelCommand());
            keyboard.RegisterCommand(Keys.F4, ExecutionType.StartingPress, new GraphicsToggleCullingCommand());
            keyboard.RegisterCommand(Keys.F5, ExecutionType.StartingPress, new GraphicsToggleDebugCollisionCommand());
            keyboard.RegisterCommand(Keys.F, ExecutionType.StartingPress, new GraphicsToggleFullscreenCommand());
            keyboard.RegisterCommand(Keys.OemPlus, ExecutionType.StartingPress, new GraphicsIncreaseWindowSizeCommand());
            keyboard.RegisterCommand(Keys.OemMinus, ExecutionType.StartingPress, new GraphicsDecreaseWindowSizeCommand());
            keyboard.RegisterCommand(Keys.OemCloseBrackets, ExecutionType.StartingPress, new GraphicsIncreaseTargetFramerateCommand());
            keyboard.RegisterCommand(Keys.OemOpenBrackets, ExecutionType.StartingPress, new GraphicsDecreaseTargetFramerateCommand());
        }

        // Do we need thisthis?
        public void LoadItem()
        {
            item = SpriteFactory.Instance.CreateSprite("item_maximtomato");
        }

        public void LoadObjects()
        {
            CollisionManager.Instance.ResetDynamicCollisionBoxes();
            // Creates kirby object
            //make it a list from the get go to make it multiplayer asap
            kirby = new Player(new Vector2(30, Constants.Graphics.FLOOR));
            kirby.PlayerSprite = SpriteFactory.Instance.CreateSprite("kirby_normal_standing_right");
            // Target the camera on Kirby
            camera.TargetPlayer(kirby);


            // Currently commented out since we don't need the item
            //LoadItem();

            // Creates enemies
            waddledeeTest = new WaddleDee(new Vector2(80, Constants.Graphics.FLOOR));
            waddledooTest = new WaddleDoo(new Vector2(170, 100));
            brontoburtTest = new BrontoBurt(new Vector2(170, 100));
            hotheadTest = new Hothead(new Vector2(170, 100));
            poppybrosjrTest = new PoppyBrosJr(new Vector2(170, 100));
            sparkyTest = new Sparky(new Vector2(170, 100));

            enemyList = new IEnemy[] { waddledeeTest, waddledooTest, brontoburtTest, hotheadTest, poppybrosjrTest, sparkyTest };
            currentEnemyIndex = 0;
            
            // Remapping keyboard to new Kirby 
            keyboard = new KeyboardController();
            SetKeyboardControls(keyboard);
        }
        #endregion

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create camera and level instances
            camera = new Camera();

            // Load all content through LevelLoader
            LevelLoader.Instance.LoadAllContent();

            level = new Level();
            level.LoadRoom("room1");

            // Load all objects
            LoadObjects();

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

            kirby.Update(time);
            enemyList[currentEnemyIndex].Update(time);

            // Commented out since we currently do not need item
            //item.Update();

            //enemyList2.Add(new Hothead(new Vector2(170, 100))); // FOR PERFORMANCE TESTING
            foreach (IEnemy enemy in enemyList2) enemy.Update(time); // FOR PERFORMANCE TESTING

            CollisionManager.Instance.CheckCollisions();

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
            kirby.Draw(spriteBatch);

            // Not currently using item
            //item.Draw(new Vector2(200, 150), spriteBatch);
            if (DEBUG_COLLISION_MODE)
            {
                CollisionManager.Instance.DebugDraw(spriteBatch);
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
