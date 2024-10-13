using Microsoft.Xna.Framework;
using System;

namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsIncreaseTargetFramerateCommand : ICommand
    {
        private Game1 game;
        private GraphicsDeviceManager graphics;
        public GraphicsIncreaseTargetFramerateCommand(Game1 game, GraphicsDeviceManager graphics)
        {
            this.game = game;
            this.graphics = graphics;
        }

        public void Execute()
        {
            if (game.TARGET_FRAMERATE < 60) // If not fullscreen and the window size isn't already maximum
            {
                game.TARGET_FRAMERATE += 5;
                game.TargetElapsedTime = TimeSpan.FromMilliseconds(1000f / game.TARGET_FRAMERATE);
            }
        }
    }
}
