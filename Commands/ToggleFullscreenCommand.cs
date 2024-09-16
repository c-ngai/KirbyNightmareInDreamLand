namespace MasterGame
{
    public class ToggleFullscreenCommand : ICommand
    {
        public ToggleFullscreenCommand() { }

        public void SetState()
        {
            Game1.self.IsFullscreen = !Game1.self.IsFullscreen;
            Game1.self.graphics.IsFullScreen = Game1.self.IsFullscreen;
            Game1.self.graphics.ApplyChanges();
        }

        public void Execute()
        {
            Game1.self.Exit();
        }
    }
}
