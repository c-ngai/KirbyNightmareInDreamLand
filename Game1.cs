using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    public class Game1 : Game
    {
        public static Game1 self { get; set; }
        public SpriteBatch spriteBatch { get; set; }
        public UnanimatedUnmovingSprite unanimatedUnmovingSprite { get; set; }
        public AnimatedUnmovingSprite animatedUnmovingSprite { get; set; }
        public UnanimatedMovingVerticallySprite unanimatedMovingVerticallySprite { get; set; }
        public AnimatedMovingHorizontallySprite animatedMovingHorizontallySprite { get; set; }
        public int state { get; set; }
        public int windowWidth { get; set; }
        public int windowHeight { get; set; }
        public ICommand quit { get; set; }
        public ICommand unanimatedUnmoving {  get; set; }
        public ICommand animatedUnmoving { get; set; }
        public ICommand movingVertically { get; set; }
        public ICommand movingHorizontally { get; set; }

        private GraphicsDeviceManager graphics;
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

            // sets up commands
            quit = new QuitCommand();
            unanimatedUnmoving = new UnanimatedUnmovingCommand();
            animatedUnmoving = new AnimatedUnmovingCommand();
            movingVertically = new UnanimatedMovingVerticallyCommand();
            movingHorizontally = new AnimatedMovingHorizontallyCommand();
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

            keyboard.RegisterCommand(Keys.D1, unanimatedUnmoving);

            keyboard.RegisterCommand(Keys.D2, animatedUnmoving);

            keyboard.RegisterCommand(Keys.D3, movingVertically);

            keyboard.RegisterCommand(Keys.D4, movingHorizontally);
        }
        protected override void Initialize()
        {

            base.Initialize();
            SetMouseControls(mouse);
            SetKeyboardControls(keyboard);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D texture = Content.Load<Texture2D>("SmileyWalk");
            unanimatedUnmovingSprite = new UnanimatedUnmovingSprite(texture, 4, 4);
            animatedUnmovingSprite = new AnimatedUnmovingSprite(texture, 4, 4);
            unanimatedMovingVerticallySprite = new UnanimatedMovingVerticallySprite(texture, 4, 4, new Vector2(350, 200));
            animatedMovingHorizontallySprite = new AnimatedMovingHorizontallySprite(texture, 4, 4, new Vector2(350, 200));
            font = Content.Load<SpriteFont>("DefaultFont");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            mouse.Update();
            keyboard.Update();
            animatedUnmovingSprite.Update();
            unanimatedMovingVerticallySprite.Update();
            animatedMovingHorizontallySprite.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            ICommand[] commands = { quit, unanimatedUnmoving, animatedUnmoving, movingVertically, movingHorizontally };

            base.Draw(gameTime);

            // draws the corresponding sprite given current game state
            commands[state].Execute();

            // always draws font
            gameFont.ControlDraw(spriteBatch, font);

        }
    }
}
