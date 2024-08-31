using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    public class Game1 : Game
    {

        public static Game1 self { get; set; }
        public Window window { get; set; }
        private GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public UnanimatedUnmovingSprite unanimatedUnmovingSprite { get; set; }
        public AnimatedUnmovingSprite animatedUnmovingSprite { get; set; }
        public UnanimatedMovingVertically unanimatedMovingVertically { get; set; }
        public AnimatedMovingHorizontally animatedMovingHorizontally { get; set; }
        private GameFont gameFont;
        private SpriteFont font;
        public MouseController mouse;
        private KeyboardController keyboard;
        public int state = 1;

        public Game1()
        {
            self = this;
            graphics = new GraphicsDeviceManager(this);
            mouse = new MouseController();
            keyboard = new KeyboardController();
            Content.RootDirectory = "Content";
            window = new Window();
            gameFont = new GameFont();
            IsMouseVisible = true;
        }

        // will later be changed to read in mouse control input
        public void SetMouseControls(MouseController mouse)
        {
            //MouseControllerState state = new MouseControllerState();

            //state.leftClick = 1;
            //state.quadrant = 1;
            //ICommand command = new Command("UnanimatedUnmoving");
            //mouse.RegisterCommand(state, command);

            //state.quadrant = 2;
            //command = new Command("AnimatedUnmoving");
            //mouse.RegisterCommand(state, command);

            //state.quadrant = 3;
            //command = new Command("UnanimatedMovingVertically");
            //mouse.RegisterCommand(state, command);

            //state.quadrant = 4;
            //command = new Command("AnimatedMovingHorizontally");
            //mouse.RegisterCommand(state, command);

            //state.rightClick = 1;
            //command = new Command("Quit");
            //for (int quad = 0; quad < 4; quad++)
            //{
            //    state.quadrant = quad;
            //    mouse.RegisterCommand(state, command);
            //}
        }

        // will later be changed to read in keyboard control input
        public void SetKeyboardControls(KeyboardController keyboard)
        {
            ICommand command = new Command("Quit");
            keyboard.RegisterCommand(Keys.D0, command);

            command = new Command("UnanimatedUnmoving");
            keyboard.RegisterCommand(Keys.D1, command);

            command = new Command("AnimatedUnmoving");
            keyboard.RegisterCommand(Keys.D2, command);

            command = new Command("UnanimatedMovingVertically");
            keyboard.RegisterCommand(Keys.D3, command);

            command = new Command("AnimatedMovingHorizontally");
            keyboard.RegisterCommand(Keys.D4, command);
        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            SetMouseControls(mouse);
            SetKeyboardControls(keyboard);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Texture2D texture = Content.Load<Texture2D>("SmileyWalk");
            unanimatedUnmovingSprite = new UnanimatedUnmovingSprite(texture, 4, 4);
            animatedUnmovingSprite = new AnimatedUnmovingSprite(texture, 4, 4);
            unanimatedMovingVertically = new UnanimatedMovingVertically(texture, 4, 4, new Vector2(350, 200));
            animatedMovingHorizontally = new AnimatedMovingHorizontally(texture, 4, 4, new Vector2(350, 200));
            font = Content.Load<SpriteFont>("DefaultFont");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            base.Update(gameTime);
            mouse.Update();
            keyboard.Update();
            animatedUnmovingSprite.Update();
            unanimatedMovingVertically.Update();
            animatedMovingHorizontally.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            base.Draw(gameTime);

            if (state == 1)
            {
                unanimatedUnmovingSprite.Draw(spriteBatch, new Vector2(350, 200));
            } 
            else if (state == 2)
            {
                animatedUnmovingSprite.Draw(spriteBatch, new Vector2(350, 200));
            }
            else if (state == 3)
            {
                unanimatedMovingVertically.Draw(spriteBatch, new Vector2(350, 200));
            }
            else if (state == 4)
            {
                animatedMovingHorizontally.Draw(spriteBatch, new Vector2(350, 200));
            }
            else if (state == 0)
            {
                this.Exit();
            }
            gameFont.ControlDraw(spriteBatch, font);

        }
    }
}
