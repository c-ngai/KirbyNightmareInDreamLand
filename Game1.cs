using KirbyNightmareInDreamLand.Commands;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Controllers;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Diagnostics;
using KirbyNightmareInDreamLand.Collision;
using KirbyNightmareInDreamLand.Entities;

namespace KirbyNightmareInDreamLand
{
    public class Game1 : Game
    {
        private SpriteBatch _spriteBatch;
        
        public GraphicsDeviceManager Graphics { get; private set; }
        public KeyboardController Keyboard { get; private set; }
        public MouseController MouseController { get; private set; }

        // Camera instance for the game
        public Camera Camera { get; private set; }

        public Level Level { get; private set; }

        // Single-player but can later be updated to an array of kirbys for multiplayer
        public List<IPlayer> Players;


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

        public static GameTime GameTime { get; private set; }

        public Stopwatch TickStopwatch { get; private set; } = new Stopwatch();


        // Graphics settings modifiable at runtime
        public bool DEBUG_TEXT_ENABLED { get; set; }
        public bool DEBUG_SPRITE_MODE { get; set; }
        public bool DEBUG_LEVEL_MODE { get; set; }
        public bool CULLING_ENABLED { get; set; }
        public bool DEBUG_COLLISION_MODE { get; set; }
        public bool IS_FULLSCREEN { get; set; }
        public int WINDOW_WIDTH { get; set; }
        public int WINDOW_HEIGHT { get; set; }
        public int WINDOW_XOFFSET { get; set; }
        public int WINDOW_YOFFSET { get; set; }
        public int MAX_WINDOW_WIDTH { get; set; }
        public int TARGET_FRAMERATE { get; set; }

        private static Game1 _instance;
        public static Game1 Instance
        {
            get
            {
                return _instance;
            }
        }
        public Game1()
        {
            _instance = this;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            DEBUG_TEXT_ENABLED = true;
            DEBUG_SPRITE_MODE = false;
            DEBUG_LEVEL_MODE = false; // TODO: Change to false by default later, currently no normal level draw behavior
            CULLING_ENABLED = true;
            DEBUG_COLLISION_MODE = false;
            IS_FULLSCREEN = false;
            WINDOW_WIDTH = Constants.Graphics.GAME_WIDTH * 3;
            WINDOW_HEIGHT = Constants.Graphics.GAME_HEIGHT * 3;
            WINDOW_XOFFSET = 0;
            WINDOW_YOFFSET = 0;
            TARGET_FRAMERATE = 60;

            TargetElapsedTime = TimeSpan.FromMilliseconds(1000f / TARGET_FRAMERATE);

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
            Graphics.HardwareModeSwitch = true;
            Graphics.IsFullScreen = IS_FULLSCREEN;
            Graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            Graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            Graphics.ApplyChanges();

            Keyboard = new KeyboardController();
            MouseController = new MouseController();

            base.Initialize();
        }



        // Everything in this region will move into eventual loader file
        #region LoaderDetails

        public void SetCollisionResponses()
        {
            String key1 = "IPlayer";
            String key2 = "Air";
            for (int i = 0; i < Constants.HitBoxes.SIDES; i++)
            {
                CollisionResponse.Instance.RegisterCollision(key1, key2, (CollisionSide) i, null, null);
            }

            key2 = "Water";
            for (int i = 0; i < Constants.HitBoxes.SIDES; i++)
            {
                CollisionResponse.Instance.RegisterCollision(key1, key2, (CollisionSide)i, null, null);
            }



        }

        public void LoadItem()
        {
            item = SpriteFactory.Instance.CreateSprite("item_maximtomato");
        }

        public void LoadObjects()
        {
            CollisionDetection.Instance.ResetDynamicCollisionBoxes();
            // Creates kirby object
            //make it a list from the get go to make it multiplayer asap
            Players = new List<IPlayer>();
            IPlayer kirby = new Player(new Vector2(30, Constants.Graphics.FLOOR));
            kirby.PlayerSprite = SpriteFactory.Instance.CreateSprite("kirby_normal_standing_right");
            Players.Add(kirby);
            // Target the camera on Kirby
            Camera.TargetPlayer(Players[0]);


            // Currently commented out since we don't need the item
            // LoadItem();

            // Creates enemies
            waddledeeTest = new WaddleDee(new Vector2(80, Constants.Graphics.FLOOR));
            waddledooTest = new WaddleDoo(new Vector2(80, Constants.Graphics.FLOOR));
            brontoburtTest = new BrontoBurt(new Vector2(80, Constants.Graphics.FLOOR));
            hotheadTest = new Hothead(new Vector2(80, Constants.Graphics.FLOOR));
            poppybrosjrTest = new PoppyBrosJr(new Vector2(80, Constants.Graphics.FLOOR));
            sparkyTest = new Sparky(new Vector2(80, Constants.Graphics.FLOOR));

            enemyList = new IEnemy[] { waddledeeTest, waddledooTest, brontoburtTest, hotheadTest, poppybrosjrTest, sparkyTest };
            currentEnemyIndex = 0;
            
        }
        #endregion

        protected override void LoadContent()
        {
            System.Diagnostics.Debug.WriteLine("Debug from content load");




            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create camera instance
            Camera = new Camera();

            // Load all content through LevelLoader
            LevelLoader.Instance.LoadAllContent();

            // Load all objects
            LoadObjects();

            // Create level instance and load initial room
            Level = new Level();
            Level.LoadRoom("room1");

            

            // Load the desired keymap by name
            LevelLoader.Instance.LoadKeymap("keymap1");
        }



        protected override void UnloadContent()
        {

        }

        List<IEnemy> enemyList2 = new List<IEnemy>(); // FOR PERFORMANCE TESTING
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            time = gameTime;

            // Reset timer for calculating max fps
            TickStopwatch.Restart();

            Keyboard.Update();
            MouseController.Update();

            GameTime = gameTime;

            foreach(IPlayer player in Players) player.Update(time);
            enemyList[currentEnemyIndex].Update(time);

            // Commented out since we currently do not need item
            // item.Update();

            //enemyList2.Add(new Hothead(new Vector2(170, 100))); // FOR PERFORMANCE TESTING
            foreach (IEnemy enemy in enemyList2) enemy.Update(time); // FOR PERFORMANCE TESTING

            CollisionDetection.Instance.CheckCollisions();

            Level.UpdateLevel();

            Camera.Update();
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            base.Draw(gameTime);

            // Level spritebatch
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.LevelMatrix);
            // Draw level
            Level.Draw(_spriteBatch);
            // Draw only selected enemy
            // enemyList[currentEnemyIndex].Draw(spriteBatch);
            foreach (IEnemy enemy in enemyList2) enemy.Draw(_spriteBatch); // FOR PERFORMANCE TESTING

            // Draw kirby
            foreach(IPlayer player in Players) player.Draw(_spriteBatch);

            // Not currently using item
            // item.Draw(new Vector2(200, 150), spriteBatch);
            if (DEBUG_COLLISION_MODE)
            {
                CollisionDetection.Instance.DebugDraw(_spriteBatch);
            }
            _spriteBatch.End();

            // Static spritebatch
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, Camera.ScreenMatrix);
            _spriteBatch.End();

            // Stop timer for calculating max fps
            TickStopwatch.Stop();
            
            // Draw Debug Text
            if (DEBUG_TEXT_ENABLED)
            {
                GameDebug.Instance.DrawDebugText(_spriteBatch);
            }
            // Draw borders (should only be visible in fullscreen for letterboxing)
            GameDebug.Instance.DrawBorders(_spriteBatch);
        }

    }
}
