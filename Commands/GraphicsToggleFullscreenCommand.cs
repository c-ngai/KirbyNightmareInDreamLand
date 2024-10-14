using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleFullscreenCommand : ICommand
    {
        private Game1 game;
        private GraphicsDeviceManager graphics;

        private int MAX_WINDOW_HEIGHT;

        private int old_WINDOW_WIDTH;
        private int old_WINDOW_HEIGHT;

        private int DISPLAY_WIDTH;
        private int DISPLAY_HEIGHT;

        private int FULLSCREEN_XOFFSET;
        private int FULLSCREEN_YOFFSET;

        public GraphicsToggleFullscreenCommand(Game1 game)
        {
            this.game = game;
            this.graphics = game.graphics;

            MAX_WINDOW_HEIGHT = game.MAX_WINDOW_WIDTH * Constants.Graphics.GAME_HEIGHT / Constants.Graphics.GAME_WIDTH;
            old_WINDOW_WIDTH = game.WINDOW_WIDTH;
            old_WINDOW_HEIGHT = game.WINDOW_HEIGHT;

            DISPLAY_WIDTH = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            DISPLAY_HEIGHT = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            FULLSCREEN_XOFFSET = (DISPLAY_WIDTH - game.MAX_WINDOW_WIDTH) / 2;
            FULLSCREEN_YOFFSET = (DISPLAY_HEIGHT - MAX_WINDOW_HEIGHT) / 2;
        }

        public void Execute()
        {
            //graphics.ToggleFullScreen();

            game.IS_FULLSCREEN = !game.IS_FULLSCREEN;
            
            if (game.IS_FULLSCREEN) // Fullscreening
            {
                // Save old window size so it can be returned to when unfullscreening
                old_WINDOW_WIDTH = game.WINDOW_WIDTH;
                old_WINDOW_HEIGHT = game.WINDOW_HEIGHT;
                // Set the window size to the display size
                game.WINDOW_WIDTH = game.MAX_WINDOW_WIDTH;
                game.WINDOW_HEIGHT = MAX_WINDOW_HEIGHT;
                graphics.PreferredBackBufferWidth = DISPLAY_WIDTH;
                graphics.PreferredBackBufferHeight = DISPLAY_HEIGHT;
                // Set window offset to fullscreen preset
                game.WINDOW_XOFFSET = FULLSCREEN_XOFFSET;
                game.WINDOW_YOFFSET = FULLSCREEN_YOFFSET;
                // Applying the changes an extra time after changing the resolution but before activating fullscreen makes the fullscreen transition faster. This makes it SLOWER on unfullscreening, so it's only here.
                graphics.ApplyChanges(); 
            }
            else // Unfullscreening
            {
                // Set window size to the old window size (from before it last went fullscreen)
                game.WINDOW_WIDTH = old_WINDOW_WIDTH;
                game.WINDOW_HEIGHT = old_WINDOW_HEIGHT;
                graphics.PreferredBackBufferWidth = game.WINDOW_WIDTH;
                graphics.PreferredBackBufferHeight = game.WINDOW_HEIGHT;
                // Reset window offset
                game.WINDOW_XOFFSET = 0;
                game.WINDOW_YOFFSET = 0;
            }
            
            graphics.IsFullScreen = game.IS_FULLSCREEN;
            graphics.ApplyChanges();
        }

        public void Undo()
        {

        }
    }
}
