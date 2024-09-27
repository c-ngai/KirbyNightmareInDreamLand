using MasterGame.Block;
using MasterGame.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace MasterGame
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

        // List to manage projectiles
        private List<IProjectile> projectiles; // TODO: Delete after synching with entity

        // TODO: Decoupling: move this out later
        public int currentEnemyIndex { get; set; }

        // Sets up single reference for game time for things such as commands which cannot get current time elsewise
        // Note this is program time and not game time 
        public GameTime time { get; set; }

        // Flamethrower instance
        private KirbyFlamethrower flamethrower;

        private EnemyBeam enemyBeam; // ENEMYBEAM TEST
        private KirbyBeam kirbyBeam; // KIRBYBEAM TEST


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
            // true = exclusive fullscreen, false = borderless fullscreen
            graphics.HardwareModeSwitch = true;
            graphics.IsFullScreen = Constants.Graphics.IS_FULL_SCREEN;
            graphics.PreferredBackBufferWidth = Constants.Graphics.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.Graphics.WINDOW_HEIGHT;
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

            keyboard.RegisterCommand(Keys.E, new KirbyTakeDamageCommand(kirby), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.T, new PreviousBlockCommand(), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.Y, new NextBlockCommand(), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.U, new HideItemCommand(this), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.I, new ShowItemCommand(this), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.O, new PreviousEnemyCommand(this), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.P, new NextEnemyCommand(this), ExecutionType.StartingPress);

            keyboard.RegisterCommand(Keys.Q, new QuitCommand(this), ExecutionType.StartingPress);
            keyboard.RegisterCommand(Keys.R, new ResetCommand(this), ExecutionType.StartingPress);
            //keyboard.RegisterCommand(Keys.F, new ToggleFullscreenCommand(), ExecutionType.StartingPress);
        }

        public void LoadItem()
        {
            item = SpriteFactory.Instance.createSprite("item_maximtomato");
        }

        public void LoadObjects()
        {
            // Creates kirby object
            //make it a list from the get go to make it multiplayer asap
            kirby = new Player(new Vector2(30, Constants.Graphics.FLOOR));
            kirby.PlayerSprite = SpriteFactory.Instance.createSprite("kirby_normal_standing_right");

            // Creates blocks
            List<string> blockList = new List<string>(); // TODO: Delete when synched with entity
            //want this away from game to a (in the future) level loader file

            //make it its own function
            //create them on demand??  performance vs code 
            //for indestructoble terrain it could be a singleton that gets borrowed by other things
            //so there is not a BUNCH loaded
            blockList = new List<string>
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
            BlockList.Instance.setBlockList(blockList);

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

            // Initialize the flamethrower
            flamethrower = new KirbyFlamethrower(); // TODO: delete when synched with entity
            projectiles = new List<IProjectile>(); // Initialize the projectiles list

            // Initialize the WaddleDoo beam    
            Vector2 beamStartPosition = new Vector2(100, 100); // Example position (would really be WaddleDoo's eye)
            enemyBeam = new EnemyBeam(beamStartPosition, true); // Initialize the beam
            
            Vector2 beamPivotPosition = new Vector2(100, 90);  // Example pivot position (would really be WaddleDoo's eye)
            kirbyBeam = new KirbyBeam(beamStartPosition, true); // Initialize the beam

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
            SpriteFactory.Instance.LoadAllTextures(Content);
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
            
            // TODO: delete after synched with entity
            // Update projectiles
            foreach (var projectile in projectiles)
            {
                projectile.Update();
            }

            // Update the WaddleDoo beam
            enemyBeam.Update();
            kirbyBeam.Update();


            // TODO: delete after synching with entities
            //flamethrower.Update(gameTime, new Vector2 (60, Constants.Graphics.FLOOR - 10), new Vector2 (1, 0)); 

            // TODO: delete when synched with entity
            // Spawn a new projectile every few frames (for demonstration)
            if (gameTime.TotalGameTime.TotalMilliseconds % 2000 < 20) // Spawn every 2000 ms
            {
                projectiles.Add(new KirbyStar(new Vector2(100, 70), new Vector2(1, -2))); // Spawn at this position and move at this speed and direction (up and to the right)
            }


            kirby.Update(time);
            enemyList[currentEnemyIndex].Update(time);
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


            // Draw projectiles
            foreach (var projectile in projectiles)
            {
                projectile.Draw(spriteBatch);
            }

            // What is this??
            // float scale = Constants.Graphics.WINDOW_HEIGHT / Constants.Graphics.GAME_HEIGHT; 

            // Draw only selected enemy
            enemyList[currentEnemyIndex].Draw(spriteBatch);

            kirby.Draw(spriteBatch);

            BlockList.Instance.Draw(new Vector2(100, 150), spriteBatch);

            // Draw the WaddleDoo beam
            enemyBeam.Draw(spriteBatch); // Draw the beam projectile
            //kirbyBeam.Draw(spriteBatch); // Draw the beam projectile

            // Draw the flamethrower segments
            //flamethrower.Draw(spriteBatch); // TODO: delete when synched with enemy.

            if (item != null)
            {
                item.Draw(new Vector2(200, 150), spriteBatch);
            }

            // End spriteBatch
            spriteBatch.End();

        }
    }
}
