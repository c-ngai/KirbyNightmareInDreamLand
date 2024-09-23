using Microsoft.Xna.Framework.Graphics;

namespace MasterGame
{
    public class ToggleFullscreenCommand : ICommand
    {
        public ToggleFullscreenCommand() { }

        public void Execute()
        {
            //Game1.self.graphics.ToggleFullScreen();

            // Game1.self.IsFullscreen = !Game1.self.IsFullscreen;
            // Game1.self.graphics.IsFullScreen = Game1.self.IsFullscreen;
            // Game1.self.graphics.ApplyChanges();
            //Game1.self.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
            //Game1.self.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
            //Game1.self.graphics.ApplyChanges();
        }

        public void Undo()
        {

        }
    }
}
