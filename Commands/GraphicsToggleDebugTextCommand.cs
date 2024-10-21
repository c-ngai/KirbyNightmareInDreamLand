namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleDebugTextCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleDebugTextCommand()
        {
            this.game = Game1.Instance;
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
