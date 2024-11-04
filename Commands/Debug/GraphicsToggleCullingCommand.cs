namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleCullingCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleCullingCommand()
        {
            this.game = Game1.Instance;
        }

        public void Execute()
        {
            game.CULLING_ENABLED = !game.CULLING_ENABLED;
        }
    }
}
