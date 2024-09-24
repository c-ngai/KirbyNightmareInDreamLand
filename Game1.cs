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
        //there should be a thing that gets all the  ?? of each kind

        //Priority fix these!!
        //PRIVATE MAKE THEM PRIVATE
        public static Game1 self { get; set; }
        private SpriteBatch spriteBatch;
        



        // TODO: Loosen coupling. GraphicsDeviceManager should probably not be public, but ToggleFullscreenCommand still needs to be able to work.
        private GraphicsDeviceManager graphics;
        private SpriteFont font;
        private KeyboardController keyboard;
        // get kirbys -- make it so it is multiple in cardinality!!
        private IPlayer kirby;
        //get waddledee
        public IEnemy waddledeeTest;

        //get waddledoo
        public IEnemy waddledooTest;

        //list of all enemies
        public IEnemy[] enemyList;

        public int currentEnemyIndex;

        public Game1()
        {
            self = this;
            graphics = new GraphicsDeviceManager(this);
            IsFullscreen = false;
            
            kirby = new Player(new Vector2(30, gameHeight * 4/5));
        }

        // will later be changed to read in mouse control input 
        //not necesary delete
        public void SetMouseControls(MouseController mouse)
        {
            mouse.leftClickIndex = 0;
            mouse.rightClickIndex = 1;
            mouse.quadrantIndex = 2;

            mouse.leftClickPressed = 1;
            mouse.rightClickPressed = 0;
            mouse.quadrant = 1;
        }

        // will later be changed to read in keyboard control input -- put into into eventual loader file
        //you wan key bindngs to be loaded after being instatiated
        public void SetKeyboardControls(KeyboardController keyboard)
        {
            keyboard.RegisterCommand(Keys.Q, new QuitCommand(this));
            keyboard.RegisterCommand(Keys.R, new ResetCommand(this));

            //keyboard.RegisterCommand(Keys.F, toggleFullscreen);

            keyboard.RegisterCommand(Keys.Right, new KirbyMoveRightCommand(kirby));
            keyboard.RegisterCommand(Keys.Left, new KirbyMoveLeftCommand(kirby));
            keyboard.RegisterCommand(Keys.A, new KirbyFaceLeftCommand(kirby));
            keyboard.RegisterCommand(Keys.D, new KirbyFaceRightCommand(kirby));
            keyboard.RegisterCommand(Keys.E, new KirbyTakeDamageCommand(kirby));
            keyboard.RegisterCommand(Keys.T, new NextBlockCommand());
            keyboard.RegisterCommand(Keys.Y, new PreviousBlockCommand());

            keyboard.RegisterCommand(Keys.O, new PreviousEnemyCommand(this));
            keyboard.RegisterCommand(Keys.P, new NextEnemyCommand(this));

        }
        protected override void Initialize()
        {
            // true = exclusive fullscreen, false = borderless fullscreen
            graphics.HardwareModeSwitch = true;
            graphics.IsFullScreen = Constants.Graphics.IS_FULL_SCREEN;
            graphics.PreferredBackBufferWidth = Constants.Graphics.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.Graphics.WINDOW_HEIGHT;
            graphics.ApplyChanges();

            base.Initialize();
        }

        public void LoadObjects()
        {
            // Creates kirby object
            kirby = new Player(new Vector2(30, Constants.Graphics.FLOOR));
            kirby.PlayerSprite = SpriteFactory.Instance.createSprite("kirby_normal_standing_right");

            // Creates blocks
            List<Sprite> blockList = new List<Sprite>();
            //want this away from game to a (in the future) level loader file

            //make it its own function
            blockList = new List<Sprite>
            {
                SpriteFactory.Instance.createSprite("tile_dirt"),
                SpriteFactory.Instance.createSprite("tile_grass"),
                SpriteFactory.Instance.createSprite("tile_platform"),
                SpriteFactory.Instance.createSprite("tile_rock"),
                SpriteFactory.Instance.createSprite("tile_rocksurface"),
                SpriteFactory.Instance.createSprite("tile_slope_gentle1_left"),
                SpriteFactory.Instance.createSprite("tile_slope_gentle1_right"),
                SpriteFactory.Instance.createSprite("tile_slope_gentle2_left"),
                SpriteFactory.Instance.createSprite("tile_slope_gentle2_right"),
                SpriteFactory.Instance.createSprite("tile_slope_steep_left"),
                SpriteFactory.Instance.createSprite("tile_slope_steep_right"),
                SpriteFactory.Instance.createSprite("tile_stoneblock"),
                SpriteFactory.Instance.createSprite("tile_waterfall"),
            // Creates enemies
            // Create a kirby sprite 
            kirby.PlayerSprite = SpriteFactory.Instance.createSprite("kirby_normal_standing_right");

            waddledeeTest = new WaddleDee(new Vector2(170, 100));
            waddledooTest = new WaddleDoo(new Vector2(170, 100));

            enemyList = new IEnemy[] { waddledeeTest, waddledooTest };
            currentEnemyIndex = 0;

            // Remapping keyboard to new Kirby 
            keyboard = new KeyboardController();
            SetKeyboardControls(keyboard);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("DefaultFont");

            // Load all sprite factory textures and sprites.
            SpriteFactory.Instance.LoadAllTextures(Content);
            SpriteFactory.Instance.LoadAllSpriteAnimations();

            // Load all objects 
            LoadObjects();

            //kirby.UpdateTexture();
            //toggleFullscreen = new ToggleFullscreenCommand();

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            keyboard.Update();
            kirby.Update();

            BlockList.Instance.Update();

            enemyList[currentEnemyIndex].Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            // Start spriteBatch
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            // Debug text: keeping track of resolutions
            string text;
            //make this its own method?? -- mark
            //kill all magic numbers 
            //for future; magic.category.
            text = "GraphicsAdapter.DefaultAdapter.CurrentDisplayMode: (" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + ", " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 10), Color.Black);
            text = "graphics.PreferredBackBuffer______: (" + graphics.PreferredBackBufferWidth + ", " + graphics.PreferredBackBufferHeight + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 30), Color.Black);
            text = "GraphicsDevice.PresentationParameters: (" + GraphicsDevice.PresentationParameters.BackBufferWidth + ", " + GraphicsDevice.PresentationParameters.BackBufferHeight + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 50), Color.Black);
            text = "GraphicsDevice.Viewport: (" + GraphicsDevice.Viewport.Width + ", " + GraphicsDevice.Viewport.Height + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 70), Color.Black);

            float scale = Constants.Graphics.WINDOW_HEIGHT / Constants.Graphics.GAME_HEIGHT; //what is this???

            // draw only selected enemy
            enemyList[currentEnemyIndex].Draw(spriteBatch);
            kirby.Draw(spriteBatch);
            waddledeeTest.Draw(spriteBatch);
            BlockList.Instance.Draw(new Vector2(100, 150), spriteBatch);

            // End spriteBatch
            spriteBatch.End();

        }
    }
}
