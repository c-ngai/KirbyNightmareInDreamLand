namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleCullingCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleCullingCommand(Game1 game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.CULLING_ENABLED = !game.CULLING_ENABLED;
        }

        public void Undo()
        {

        }
    }
}
