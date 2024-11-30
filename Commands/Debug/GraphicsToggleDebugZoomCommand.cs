namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleDebugZoomCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleDebugZoomCommand()
        {
            this.game = Game1.Instance;
        }

        public void Execute()
        {
            game.DEBUG_ZOOM_MODE = !game.DEBUG_ZOOM_MODE;
        }
    }
}
