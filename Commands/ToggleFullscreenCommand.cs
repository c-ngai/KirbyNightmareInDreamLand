namespace MasterGame
{
    public class ToggleFullscreenCommand : ICommand
    {
        public ToggleFullscreenCommand() { }

        public void Execute()
        {
            Game1.self.IsFullscreen = !Game1.self.IsFullscreen;
            Game1.self.graphics.IsFullScreen = Game1.self.IsFullscreen;
            Game1.self.graphics.ApplyChanges();
        }
    }
}
