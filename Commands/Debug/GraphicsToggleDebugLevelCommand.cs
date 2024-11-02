namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleDebugLevelCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleDebugLevelCommand()
        {
            this.game = Game1.Instance;
        }

        public void Execute()
        {
            game.DEBUG_LEVEL_MODE = !game.DEBUG_LEVEL_MODE;
        }
    }
}
