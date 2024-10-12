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
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }



        protected override void Initialize()
        {
            DEBUG_TEXT_ENABLED = true;
            DEBUG_SPRITE_MODE = false;
            DEBUG_LEVEL_MODE = true; // TODO: Change to false by default later, currently no normal level draw behavior
            CULLING_ENABLED = true;
            DEBUG_COLLISION_MODE = false;
            IS_FULLSCREEN = false;
            WINDOW_WIDTH = 720;
            WINDOW_HEIGHT = 480;
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

            base.Initialize();
        }



        // Everything in this region will move into eventual loader file
        #region LoaderDetails
        // Will later be changed to read in keyboard control input 
        public void SetKeyboardControls(KeyboardController keyboard)
        {
            keyboard.RegisterCommand(Keys.Right, new KirbyMoveRightCommand(kirby, Keys.Right, keyboard), new KirbyRunningRightCommand(this, keyboard, Keys.Right, kirby), ExecutionType.Pressed);
            keyboard.RegisterCommand(Keys.Left, new KirbyMoveLeftCommand(kirby, Keys.Left, keyboard), new KirbyRunningLeftCommand(this, keyboard, Keys.Left, kirby), ExecutionType.Pressed);

            // this is hard-coded bc it needs to know the keybind to attack to check if it needs to slide
            keyboard.RegisterCommand(Keys.Down, new KirbyCrouchAndSlideCommand(kirby, Keys.Z, keyboard, this), new KirbyStopCrouchCommand(kirby), ExecutionType.Pressed);
            keyboard.RegisterCommand(Keys.Up, new KirbyFloatCommand(kirby), null, ExecutionType.Pressed);
            keyboard.RegisterCommand(Keys.X, new KirbyJumpCommand(kirby), null, ExecutionType.Pressed);

            keyboard.RegisterCommand(Keys.D, new KirbyFaceRightCommand(kirby), null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.Z, new KirbyAttackCommand(kirby),null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.D1, new KirbyChangeNormalCommand(kirby), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D2, new KirbyChangeBeamCommand(kirby), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D3, new KirbyChangeFireCommand(kirby), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D4, new KirbyChangeSparkCommand(kirby), null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.E, new KirbyTakeDamageCommand(kirby), new KirbyStopMovingCommand(kirby), ExecutionType.Pressed);

            //keyboard.RegisterCommand(Keys.U, new HideItemCommand(this), null, ExecutionType.StartingPress);
            //keyboard.RegisterCommand(Keys.I, new ShowItemCommand(this), null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.O, new PreviousEnemyCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.P, new NextEnemyCommand(this), null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.Q, new QuitCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.R, new ResetCommand(this), null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.F1, new GraphicsToggleDebugTextCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.F2, new GraphicsToggleDebugSpriteCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.F3, new GraphicsToggleDebugLevelCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.F4, new GraphicsToggleCullingCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.F5, new GraphicsToggleDebugCollisionCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.F, new GraphicsToggleFullscreenCommand(this, graphics), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.OemPlus, new GraphicsIncreaseWindowSizeCommand(this, graphics), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.OemMinus, new GraphicsDecreaseWindowSizeCommand(this, graphics), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.OemCloseBrackets, new GraphicsIncreaseTargetFramerateCommand(this, graphics), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.OemOpenBrackets, new GraphicsDecreaseTargetFramerateCommand(this, graphics), null, ExecutionType.StartingPress);
        }

        public void LoadItem()
        {
            item = SpriteFactory.Instance.CreateSprite("item_maximtomato");
        }

        public void LoadObjects()
        {
            // Creates kirby object
            //make it a list from the get go to make it multiplayer asap
            kirby = new Player(new Vector2(30, Constants.Graphics.FLOOR));
            kirby.PlayerSprite = SpriteFactory.Instance.CreateSprite("kirby_normal_standing_right");
            // Target the camera on Kirby
            camera.TargetPlayer(kirby);


            LoadItem();

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
            camera = new Camera(this);

            SpriteFactory.Instance.LoadGame(this);
            // Load all content through LevelLoader
            LevelLoader.Instance.LoadAllContent(this, Content, GraphicsDevice);

            level = new Level(this);
            level.LoadRoom("testroom1");

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

            GameTime = gameTime;

            kirby.Update(time);
            enemyList[currentEnemyIndex].Update(time);
            item.Update();

            //enemyList2.Add(new Hothead(new Vector2(170, 100))); // FOR PERFORMANCE TESTING
            foreach (IEnemy enemy in enemyList2) enemy.Update(time); // FOR PERFORMANCE TESTING

            CollisionManager.Instance.CheckCollisions();

            camera.Update();
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);

            // Level spritebatch
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.LevelMatrix);
            // Draw level
            level.Draw(spriteBatch);
            // Draw only selected enemy
            enemyList[currentEnemyIndex].Draw(spriteBatch);
            foreach (IEnemy enemy in enemyList2) enemy.Draw(spriteBatch); // FOR PERFORMANCE TESTING
            // Draw kirby
            kirby.Draw(spriteBatch);
            // Draw item
            item.Draw(new Vector2(200, 150), spriteBatch);
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
