namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleDebugTextCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleDebugTextCommand(Game1 game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.DEBUG_TEXT_ENABLED = !game.DEBUG_TEXT_ENABLED;
        }

        public void Undo()
        {

        }
    }
}
