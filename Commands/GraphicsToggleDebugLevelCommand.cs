namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleDebugLevelCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleDebugLevelCommand(Game1 game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.DEBUG_LEVEL_MODE = !game.DEBUG_LEVEL_MODE;
        }
    }
}
