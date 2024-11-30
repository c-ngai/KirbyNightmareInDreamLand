using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleSplitscreenCommand : ICommand
    {
        private static Game1 game = Game1.Instance;
        private static GraphicsDeviceManager graphics = game.Graphics;
        public GraphicsToggleSplitscreenCommand()
        {

        }

        public void Execute()
        {
            if (game.SPLITSCREEN_AVAILABLE) {
                game.SPLITSCREEN_MODE = !game.SPLITSCREEN_MODE;
            }

            UpdateSplitscreenScaling();
        }

        public static void UpdateSplitscreenScaling()
        {
            if (game.IS_FULLSCREEN)
            {
                // When toggling ON splitscreen while in fullscreen mode
                if (game.SPLITSCREEN_MODE && game.SPLITSCREEN_AVAILABLE)
                {
                    // If the window size is not already a multiple of twice the game size (2x, 4x, 6x, etc)
                    if ((game.WINDOW_WIDTH / Constants.Graphics.GAME_WIDTH) % 2 != 0)
                    {
                        // The window must already be as big an integer scale as it can be to fit in the screen, so we must decrement it by one game size
                        game.WINDOW_WIDTH -= Constants.Graphics.GAME_WIDTH;
                        game.WINDOW_HEIGHT -= Constants.Graphics.GAME_HEIGHT;
                        // Increment window offset by half the game size to recenter it in the display
                        game.WINDOW_XOFFSET += Constants.Graphics.GAME_WIDTH / 2;
                        game.WINDOW_YOFFSET += Constants.Graphics.GAME_HEIGHT / 2;
                    }
                }
                // When toggling OFF splitscreen while in fullscreen mode
                else
                {
                    // If the window is not the max size
                    if (game.WINDOW_WIDTH < game.MAX_WINDOW_WIDTH)
                    {
                        // We must be one game size below max size (see above), so me must increment by one game size
                        game.WINDOW_WIDTH += Constants.Graphics.GAME_WIDTH;
                        game.WINDOW_HEIGHT += Constants.Graphics.GAME_HEIGHT;
                        // Decrement window offset by half the game size to recenter it in the display
                        game.WINDOW_XOFFSET -= Constants.Graphics.GAME_WIDTH / 2;
                        game.WINDOW_YOFFSET -= Constants.Graphics.GAME_HEIGHT / 2;
                    }
                }
            }

            // When toggling ON splitscreen while NOT in fullscreen mode
            else if (game.SPLITSCREEN_MODE && game.SPLITSCREEN_AVAILABLE)
            {
                // If the window size is not already a multiple of twice the game size (2x, 4x, 6x, etc)
                if ((game.WINDOW_WIDTH / Constants.Graphics.GAME_WIDTH) % 2 != 0)
                {
                    // If increasing the window by 1x game size would still fit in the screen
                    if (game.WINDOW_WIDTH + Constants.Graphics.GAME_WIDTH <= game.MAX_WINDOW_WIDTH)
                    {
                        // Increment window size by game size, keeping it at an integer scale.
                        game.WINDOW_WIDTH += Constants.Graphics.GAME_WIDTH;
                        game.WINDOW_HEIGHT += Constants.Graphics.GAME_HEIGHT;
                    }
                    // If increasing the window by 1x game size would make the window bigger than the display
                    else
                    {
                        // Decrement window size by game size, keeping it at an integer scale.
                        game.WINDOW_WIDTH -= Constants.Graphics.GAME_WIDTH;
                        game.WINDOW_HEIGHT -= Constants.Graphics.GAME_HEIGHT;
                    }
                }
                // Set back buffer to new window size and apply changes.
                graphics.PreferredBackBufferWidth = game.WINDOW_WIDTH;
                graphics.PreferredBackBufferHeight = game.WINDOW_HEIGHT;
                graphics.ApplyChanges();
            }
        }

    }
}
