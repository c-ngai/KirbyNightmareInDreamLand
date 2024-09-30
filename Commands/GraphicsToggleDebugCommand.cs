namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleDebugCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleDebugCommand(Game1 game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.DEBUG_SPRITE_MODE = !game.DEBUG_SPRITE_MODE;
        }

        public void Undo()
        {

        }
    }
}
