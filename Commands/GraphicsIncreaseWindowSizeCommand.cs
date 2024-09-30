using Microsoft.Xna.Framework;

namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsIncreaseWindowSizeCommand : ICommand
    {
        Game1 game;
        GraphicsDeviceManager graphics;
        public GraphicsIncreaseWindowSizeCommand(Game1 game, GraphicsDeviceManager graphics)
        {
            this.game = game;
            this.graphics = graphics;
        }

        public void Execute()
        {
            game.WINDOW_WIDTH += 3;
            game.WINDOW_HEIGHT += 2;
            graphics.PreferredBackBufferWidth = game.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = game.WINDOW_HEIGHT;
            graphics.ApplyChanges();
        }

        public void Undo()
        {

        }
    }
}
