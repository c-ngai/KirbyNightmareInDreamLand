namespace KirbyNightmareInDreamLand.Commands
{
    public class ResetCommand : ICommand
    {
        private Game1 game;
        public ResetCommand()
        {
            game = Game1.Instance;
        }

        // is there a better way to do this beyond resetting every single game object?
        public void Execute()
        {
            game.LoadObjects();
        }
    }
}
