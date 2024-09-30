using KirbyNightmareInDreamLand.Block;
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
        
        private GraphicsDeviceManager graphics;
        private SpriteFont font;
        private KeyboardController keyboard;

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
        public int WINDOW_WIDTH;
        public int WINDOW_HEIGHT;



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
            WINDOW_WIDTH = 720;
            WINDOW_HEIGHT = 480;

            // true = exclusive fullscreen, false = borderless fullscreen
            graphics.HardwareModeSwitch = true;
            graphics.IsFullScreen = Constants.Graphics.IS_FULL_SCREEN;
            graphics.PreferredBackBufferWidth = this.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = this.WINDOW_HEIGHT;
            graphics.ApplyChanges();
            

            keyboard = new KeyboardController();

            base.Initialize();
        }

        // Everything in this region will move into eventual loader file
        #region LoaderDetails
        // Will later be changed to read in keyboard control input 
        public void SetKeyboardControls(KeyboardController keyboard)
        {
            keyboard.RegisterCommand(Keys.Right, new KirbyMoveRightCommand(kirby, Keys.Right, keyboard, this), ExecutionType.Pressed);
            keyboard.RegisterCommand(Keys.Left, new KirbyMoveLeftCommand(kirby, Keys.Left, keyboard, this), ExecutionType.Pressed);

            // this is hard-coded bc it needs to know the keybind to attack to check if it needs to slide
            keyboard.RegisterCommand(Keys.Down, new KirbyMoveCrouchedCommand(kirby, Keys.Z, keyboard, this), ExecutionType.Pressed);
            keyboard.RegisterCommand(Keys.Up, new KirbyFloatCommand(kirby), ExecutionType.Pressed);
            keyboard.RegisterCommand(Keys.X, new KirbyJumpCommand(kirby), ExecutionType.Pressed);

            keyboard.RegisterCommand(Keys.A, new KirbyFaceLeftCommand(kirby), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D, new KirbyFaceRightCommand(kirby), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.Z, new KirbyAttackCommand(kirby), ExecutionType.Pressed);

            keyboard.RegisterCommand(Keys.D1, new KirbyChangeNormalCommand(kirby), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D2, new KirbyChangeBeamCommand(kirby), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D3, new KirbyChangeFireCommand(kirby), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.D4, new KirbyChangeSparkCommand(kirby), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.E, new KirbyTakeDamageCommand(kirby), ExecutionType.Pressed);

            keyboard.RegisterCommand(Keys.T, new PreviousBlockCommand(), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.Y, new NextBlockCommand(), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.U, new HideItemCommand(this), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.I, new ShowItemCommand(this), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.O, new PreviousEnemyCommand(this), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.P, new NextEnemyCommand(this), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.Q, new QuitCommand(this), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.R, new ResetCommand(this), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.LeftControl, new GraphicsToggleDebugCommand(this), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.OemPlus, new GraphicsIncreaseWindowSizeCommand(this, graphics), ExecutionType.Pressed);
            keyboard.RegisterCommand(Keys.OemMinus, new GraphicsDecreaseWindowSizeCommand(this, graphics), ExecutionType.Pressed);
            //keyboard.RegisterCommand(Keys.F, new ToggleFullscreenCommand(), ExecutionType.StartingPress);
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

            // Creates blocks
            //make it its own function
            //create them on demand??  performance vs code 
            //for indestructoble terrain it could be a singleton that gets borrowed by other things
            //so there is not a BUNCH loaded
            List<string> blockList = new List<string>
            {
                "tile_dirt",
                "tile_grass",
                "tile_platform",
                "tile_rock",
                "tile_rocksurface",
                "tile_slope_gentle1_left",
                "tile_slope_gentle1_right",
                "tile_slope_gentle2_left",
                "tile_slope_steep_right",
                "tile_slope_gentle2_right",
                "tile_slope_steep_left",
                "tile_stoneblock",
                "tile_waterfall",
            };
            BlockList.Instance.SetBlockList(blockList);

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

            font = Content.Load<SpriteFont>("DefaultFont");

            // Load all sprite factory textures and sprites.
            SpriteFactory.Instance.LoadAllTextures(Content, this);
            SpriteFactory.Instance.LoadAllSpriteAnimations();
            SpriteDebug.Instance.Load(GraphicsDevice);

            // Load all objects 
            LoadObjects();
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
            BlockList.Instance.Update();
        }

        // TODO: Decoupling: move text draw details out
        public void DrawText()
        {
            string text;
            text = "GraphicsAdapter.DefaultAdapter.CurrentDisplayMode: (" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + ", " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 10), Color.Black);
            text = "graphics.PreferredBackBuffer______: (" + graphics.PreferredBackBufferWidth + ", " + graphics.PreferredBackBufferHeight + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 30), Color.Black);
            text = "GraphicsDevice.PresentationParameters: (" + GraphicsDevice.PresentationParameters.BackBufferWidth + ", " + GraphicsDevice.PresentationParameters.BackBufferHeight + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 50), Color.Black);
            text = "GraphicsDevice.Viewport: (" + GraphicsDevice.Viewport.Width + ", " + GraphicsDevice.Viewport.Height + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 70), Color.Black);
        }

        //take off draw text magic numbers
        //eventually take these off and make game only deal with high level objects 
        //game object management takes care of the lists and iterates them
        //game then grabs it from them and does its job.

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            // Start spriteBatch
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            DrawText();

            // What is this??
            // float scale = Constants.Graphics.WINDOW_HEIGHT / Constants.Graphics.GAME_HEIGHT; 

            // Draw only selected enemy
            enemyList[currentEnemyIndex].Draw(spriteBatch);

            kirby.Draw(spriteBatch);

            BlockList.Instance.Draw(new Vector2(100, 150), spriteBatch);

            if (item != null)
            {
                item.Draw(new Vector2(200, 150), spriteBatch);
            }

            // End spriteBatch
            spriteBatch.End();

        }
    }
}
