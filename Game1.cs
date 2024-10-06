using KirbyNightmareInDreamLand.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Controllers;

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

        // Graphics settings modifiable at runtime
        public bool DEBUG_SPRITE_MODE { get; set; }
        public bool IS_FULLSCREEN { get; set; }
        public int WINDOW_WIDTH { get; set; }
        public int WINDOW_HEIGHT { get; set; }
        public int WINDOW_XOFFSET { get; set; }
        public int WINDOW_YOFFSET { get; set; }
        public int MAX_WINDOW_WIDTH { get; set; }


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
            DEBUG_SPRITE_MODE = false;
            IS_FULLSCREEN = false;
            WINDOW_WIDTH = 720;
            WINDOW_HEIGHT = 480;
            WINDOW_XOFFSET = 0;
            WINDOW_YOFFSET = 0;

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

            keyboard.RegisterCommand(Keys.Z, new KirbyAttackCommand(kirby), new KirbyStopMovingCommand(kirby), ExecutionType.Pressed);

            keyboard.RegisterCommand(Keys.D1, new KirbyChangeNormalCommand(kirby), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D2, new KirbyChangeBeamCommand(kirby), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D3, new KirbyChangeFireCommand(kirby), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D4, new KirbyChangeSparkCommand(kirby), null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.E, new KirbyTakeDamageCommand(kirby), new KirbyStopMovingCommand(kirby), ExecutionType.Pressed);

            keyboard.RegisterCommand(Keys.U, new HideItemCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.I, new ShowItemCommand(this), null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.O, new PreviousEnemyCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.P, new NextEnemyCommand(this), null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.Q, new QuitCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.R, new ResetCommand(this), null, ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.LeftControl, new GraphicsToggleDebugCommand(this), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.OemPlus, new GraphicsIncreaseWindowSizeCommand(this, graphics), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.OemMinus, new GraphicsDecreaseWindowSizeCommand(this, graphics), null, ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.F, new GraphicsToggleFullscreenCommand(this, graphics), null, ExecutionType.StartingPress); 
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

            LoadItem();

            // Creates enemies
            waddledeeTest = new WaddleDee(new Vector2(170, 100));
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

            // Target the camera on Kirby
            camera.TargetPlayer(kirby);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            time = gameTime;

            keyboard.Update();

            kirby.Update(time);
            enemyList[currentEnemyIndex].Update(time);
            
            if (item != null)
            {
                item.Update();
            }
            camera.Update();
        }

        // TODO: Decoupling: move text draw details out
        public void DrawText()
        {
            string text;
            text = "GraphicsAdapter.DefaultAdapter.CurrentDisplayMode: (" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + ", " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + ")";
            spriteBatch.DrawString(LevelLoader.Instance.font, text, new Vector2(10, 10), Color.Black);
            text = "graphics.PreferredBackBuffer______: (" + graphics.PreferredBackBufferWidth + ", " + graphics.PreferredBackBufferHeight + ")";
            spriteBatch.DrawString(LevelLoader.Instance.font, text, new Vector2(10, 30), Color.Black);
            text = "GraphicsDevice.PresentationParameters: (" + GraphicsDevice.PresentationParameters.BackBufferWidth + ", " + GraphicsDevice.PresentationParameters.BackBufferHeight + ")";
            spriteBatch.DrawString(LevelLoader.Instance.font, text, new Vector2(10, 50), Color.Black);
            text = "GraphicsDevice.Viewport: (" + GraphicsDevice.Viewport.Width + ", " + GraphicsDevice.Viewport.Height + ")";
            spriteBatch.DrawString(LevelLoader.Instance.font, text, new Vector2(10, 70), Color.Black);
        }

        //take off draw text magic numbers
        //eventually take these off and make game only deal with high level objects 
        //game object management takes care of the lists and iterates them
        //game then grabs it from them and does its job.

        // Draws black borders on the edge of the screen. Should be only visible in fullscreen. Done to maintain aspect ratio and integer scaling regardless of display resolution.
        private void DrawBorders()
        {
            // Left side
            spriteBatch.Draw(LevelLoader.Instance.borders, new Rectangle(0, 0, WINDOW_XOFFSET, WINDOW_HEIGHT + 2*WINDOW_YOFFSET), Color.White);
            // Top side
            spriteBatch.Draw(LevelLoader.Instance.borders, new Rectangle(0, 0, WINDOW_WIDTH + 2 * WINDOW_XOFFSET, WINDOW_YOFFSET), Color.White);
            // Right side
            spriteBatch.Draw(LevelLoader.Instance.borders, new Rectangle(WINDOW_XOFFSET + WINDOW_WIDTH, 0, WINDOW_XOFFSET, WINDOW_HEIGHT + 2 * WINDOW_YOFFSET), Color.White);
            // Bottom side
            spriteBatch.Draw(LevelLoader.Instance.borders, new Rectangle(0, WINDOW_YOFFSET + WINDOW_HEIGHT, WINDOW_WIDTH + 2 * WINDOW_XOFFSET, WINDOW_YOFFSET), Color.White);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            // Start spriteBatch
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            //DrawText();
            level.Draw(spriteBatch);

            // Draw only selected enemy
            enemyList[currentEnemyIndex].Draw(spriteBatch);

            kirby.Draw(spriteBatch);

            if (item != null)
            {
                item.LevelDraw(new Vector2(200, 150), spriteBatch);
            }

            // End spriteBatch
            DrawBorders();
            spriteBatch.End();

        }
    }
}
