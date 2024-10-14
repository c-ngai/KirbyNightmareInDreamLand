namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleDebugCollisionCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleDebugCollisionCommand(Game1 game)
        {
            this.game = game;
        }

        public void Execute()
        {
            game.DEBUG_COLLISION_MODE = !game.DEBUG_COLLISION_MODE;
        }
    }
}
