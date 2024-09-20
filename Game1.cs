using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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

        // TODO: Loosen coupling. GraphicsDeviceManager should probably not be public, but ToggleFullscreenCommand still needs to be able to work.
        public GraphicsDeviceManager graphics;
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
            IsMouseVisible = true;
            state = 1;
            windowWidth = 240;
            windowHeight = 160;
            IsFullscreen = false;

            // sets up commands
            quit = new QuitCommand();
            toggleFullscreen = new ToggleFullscreenCommand();
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

            //keyboard.RegisterCommand(Keys.F, toggleFullscreen);

        }
        protected override void Initialize()
        {
            // true = exclusive fullscreen, false = borderless fullscreen
            graphics.HardwareModeSwitch = true;
            graphics.IsFullScreen = IsFullscreen;
            graphics.PreferredBackBufferWidth = 240;
            graphics.PreferredBackBufferHeight = 160;
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
            SpriteFactory.Instance.LoadAllSpriteAnimations();
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

            // Start spriteBatch
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);

            // Debug text: keeping track of resolutions
            string text;
            text = "GraphicsAdapter.DefaultAdapter.CurrentDisplayMode: (" + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width + ", " + GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 10), Color.Black);
            text = "graphics.PreferredBackBuffer______: (" + graphics.PreferredBackBufferWidth + ", " + graphics.PreferredBackBufferHeight + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 30), Color.Black);
            text = "GraphicsDevice.PresentationParameters: (" + GraphicsDevice.PresentationParameters.BackBufferWidth + ", " + GraphicsDevice.PresentationParameters.BackBufferHeight + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 50), Color.Black);
            text = "GraphicsDevice.Viewport: (" + GraphicsDevice.Viewport.Width + ", " + GraphicsDevice.Viewport.Height + ")";
            spriteBatch.DrawString(font, text, new Vector2(10, 70), Color.Black);

            // Draw test sprite
            TestSprite.Draw(new Vector2(100, 100));

            // End spriteBatch
            spriteBatch.End();

        }
    }
}
