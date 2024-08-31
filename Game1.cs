using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Sprint0
{
    public class Game1 : Game
    {
        public static Game1 self { get; set; }
        public SpriteBatch spriteBatch { get; set; }
        public UnanimatedUnmovingSprite unanimatedUnmovingSprite { get; set; }
        public AnimatedUnmovingSprite animatedUnmovingSprite { get; set; }
        public UnanimatedMovingVertically unanimatedMovingVertically { get; set; }
        public AnimatedMovingHorizontally animatedMovingHorizontally { get; set; }
        public int state { get; set; }
        public int windowWidth { get; set; }
        public int windowHeight { get; set; }

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
            unanimatedMovingVertically = new UnanimatedMovingVertically(texture, 4, 4, new Vector2(350, 200));
            animatedMovingHorizontally = new AnimatedMovingHorizontally(texture, 4, 4, new Vector2(350, 200));
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
            unanimatedMovingVertically.Update();
            animatedMovingHorizontally.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draws the corresponding sprite given current game state (I cannot figure out how to create an array or dictionary containing non-Action functions)
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

            // always draws font
            gameFont.ControlDraw(spriteBatch, font);

        }
    }
}
