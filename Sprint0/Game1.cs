using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sprint0
{
    public class Game1 : Game
    {

        public static Game1 self;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private UnanimatedUnmovingSprite unanimatedUnmovingSprite;
        //private AnimatedSprite animatedSprite;
        private SpriteFont font;
        private IController mouse;
        private IController keyboard;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            mouse = new MouseController();
            keyboard = new KeyboardController();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // will later be changed to read in mouse control input
        public void setMouseControls(IController mouse)
        {

        }

        // will later be changed to read in keyboard control input
        public void setKeyboardControls(IController keyboard)
        {

        }
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            setMouseControls(mouse);
            setKeyboardControls(keyboard);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Texture2D texture1 = Content.Load<Texture2D>("Idle");
            unanimatedUnmovingSprite = new UnanimatedUnmovingSprite(texture1);
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
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            base.Draw(gameTime);
            unanimatedUnmovingSprite.ControlDraw(spriteBatch, new Vector2(200, 200));
        }
    }
}
