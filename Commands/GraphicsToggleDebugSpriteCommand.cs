namespace KirbyNightmareInDreamLand.Commands
{
    public class GraphicsToggleDebugSpriteCommand : ICommand
    {
        Game1 game;
        public GraphicsToggleDebugSpriteCommand()
        {
            this.game = Game1.Instance;
        }

        public void Execute()
        {
            game.DEBUG_SPRITE_MODE = !game.DEBUG_SPRITE_MODE;
        }
    }
}
