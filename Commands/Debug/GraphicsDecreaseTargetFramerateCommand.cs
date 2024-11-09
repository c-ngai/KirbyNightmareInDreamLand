using Microsoft.Xna.Framework;
using System;

namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsDecreaseTargetFramerateCommand : ICommand
    {
        private Game1 game;
        private GraphicsDeviceManager graphics;
        public GraphicsDecreaseTargetFramerateCommand()
        {
            this.game = Game1.Instance;
            this.graphics = game.Graphics;
        }

        public void Execute()
        {
            if (game.TARGET_FRAMERATE > Constants.Graphics.MIN_FRAME_RATE) // If not fullscreen and the window size isn't 
            {
                game.TARGET_FRAMERATE -= Constants.Graphics.MIN_FRAME_RATE;
                game.TargetElapsedTime = TimeSpan.FromMilliseconds(Constants.Graphics.TIME_CONVERSION / game.TARGET_FRAMERATE);
            }
        }
    }
}
