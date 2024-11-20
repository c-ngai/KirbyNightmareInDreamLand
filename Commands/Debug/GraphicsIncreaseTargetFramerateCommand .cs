using Microsoft.Xna.Framework;
using System;

namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsIncreaseTargetFramerateCommand : ICommand
    {
        private Game1 game;
        private GraphicsDeviceManager graphics;
        public GraphicsIncreaseTargetFramerateCommand()
        {
            this.game = Game1.Instance;
            this.graphics = game.Graphics;
        }

        public void Execute()
        {
            if (game.TARGET_FRAMERATE < Constants.Graphics.MAX_FRAME_RATE) // If not fullscreen and the window size isn't already maximum
            {
                game.TARGET_FRAMERATE += 1;
                game.TargetElapsedTime = TimeSpan.FromMilliseconds(Constants.Graphics.TIME_CONVERSION / game.TARGET_FRAMERATE);
            }
        }
    }
}
