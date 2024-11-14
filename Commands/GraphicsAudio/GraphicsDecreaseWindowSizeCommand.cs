﻿using Microsoft.Xna.Framework;

namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsDecreaseWindowSizeCommand : ICommand
    {
        private Game1 game;
        private GraphicsDeviceManager graphics;
        public GraphicsDecreaseWindowSizeCommand()
        {
            this.game = Game1.Instance;
            this.graphics = game.Graphics;
        }

        public void Execute()
        {
            if (!game.IS_FULLSCREEN) // If not fullscreen and the window size isn't already the same as the game resolution (smallest it should ever be)
            {
                if (!game.SPLITSCREEN_MODE && game.WINDOW_WIDTH > Constants.Graphics.GAME_WIDTH)
                {
                    // Decrement window size by game size, keeping it at an integer scale.
                    game.WINDOW_WIDTH -= Constants.Graphics.GAME_WIDTH;
                    game.WINDOW_HEIGHT -= Constants.Graphics.GAME_HEIGHT;
                }
                else if (game.SPLITSCREEN_MODE && game.WINDOW_WIDTH > Constants.Graphics.GAME_WIDTH * 2)
                {
                    // Decrement window size by 2x game size, keeping it at an integer scale.
                    game.WINDOW_WIDTH -= Constants.Graphics.GAME_WIDTH * 2;
                    game.WINDOW_HEIGHT -= Constants.Graphics.GAME_HEIGHT * 2;
                }
                // Set back buffer to new window size and apply changes.
                graphics.PreferredBackBufferWidth = game.WINDOW_WIDTH;
                graphics.PreferredBackBufferHeight = game.WINDOW_HEIGHT;
                graphics.ApplyChanges();
            }
        }
    }
}
