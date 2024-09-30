namespace MasterGame.Commands
{
    public class ResetCommand : ICommand
    {
        private Game1 game;
        public ResetCommand(Game1 currentGame)
        {
            game = currentGame;
        }

        // is there a better way to do this beyond resetting every single game object?
        public void Execute()
        {
            game.LoadObjects();
        }
        public void Undo()
        {

        }

    }
}
