using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MasterGame
{
    public class Game1 : Game
    {
        public static Game1 self { get; set; }
        public SpriteBatch spriteBatch { get; set; }
        public Sprite TestSprite { get; set; }
        public int state { get; set; }
        public int windowWidth { get; set; }
        public int windowHeight { get; set; }
        public bool IsFullscreen { get; set; }
        public ICommand quit { get; set; }
        public ICommand toggleFullscreen { get; set; }

        public ICommand kirbyMoveRight { get; set; }

        // TODO: Loosen coupling. GraphicsDeviceManager should probably not be public, but ToggleFullscreenCommand still needs to be able to work.
        public GraphicsDeviceManager graphics;
        private GameFont gameFont;
        private SpriteFont font;
        private MouseController mouse;
        private KeyboardController keyboard;

        public Game1()
        {
            self = this;
            graphics = new GraphicsDeviceManager(this);
            mouse = new MouseController();
            keyboard = new KeyboardController();
            Content.RootDirectory = "Content";
            gameFont = new GameFont();
            IsMouseVisible = true;
            state = 1;
            windowWidth = 800;
            windowHeight = 450;
            IsFullscreen = false;

            // sets up commands
            IPlayer kirby = new Player();
            quit = new QuitCommand(this);
            toggleFullscreen = new ToggleFullscreenCommand();
            kirbyMoveRight = new KirbyMoveRightCommand(kirby);
        }

        // will later be changed to read in mouse control input
        public void SetMouseControls(MouseController mouse)
        {
            mouse.leftClickIndex = 0;
            mouse.rightClickIndex = 1;
            mouse.quadrantIndex = 2;

            mouse.leftClickPressed = 1;
            mouse.rightClickPressed = 0;
            mouse.quadrant = 1;
        }

        // will later be changed to read in keyboard control input
        public void SetKeyboardControls(KeyboardController keyboard)
        {
            keyboard.RegisterCommand(Keys.D0, quit);

            keyboard.RegisterCommand(Keys.F, toggleFullscreen);

            keyboard.RegisterCommand(Keys.Right, kirbyMoveRight);

        }
        protected override void Initialize()
        {
            // true = exclusive fullscreen, false = borderless fullscreen
            graphics.HardwareModeSwitch = true;
            graphics.IsFullScreen = IsFullscreen;
            graphics.ApplyChanges();


            base.Initialize();
            SetMouseControls(mouse);
            SetKeyboardControls(keyboard);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("DefaultFont");

            // Load all sprite factory textures and sprites.
            SpriteFactory.Instance.LoadAllTextures(Content);
            SpriteFactory.Instance.LoadAllSprites();
            // Create a test sprite (TEMPORARY)
            TestSprite = SpriteFactory.Instance.createSprite("kirby_normal_walking");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            mouse.Update();
            keyboard.Update();

            TestSprite.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            ICommand[] commands = { quit };

            base.Draw(gameTime);

            // draws the corresponding sprite given current game state
            //commands[state].Execute();

            // always draws font
            gameFont.ControlDraw(spriteBatch, font);

            // Draw test sprite
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            TestSprite.Draw(spriteBatch, new Vector2(200, 200));

            spriteBatch.End();

        }
    }
}
