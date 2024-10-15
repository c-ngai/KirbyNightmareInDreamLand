namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleDebugCollisionCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleDebugCollisionCommand()
        {
            this.game = Game1.Instance;
        }

        public void Execute()
        {
            game.DEBUG_COLLISION_MODE = !game.DEBUG_COLLISION_MODE;
        }
    }
}
