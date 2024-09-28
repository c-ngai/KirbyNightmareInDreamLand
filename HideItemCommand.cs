namespace MasterGame.Commands
{
    public class HideItemCommand : ICommand
    {
        private Game1 game;
        public HideItemCommand(Game1 currentGame)
        {
            game = currentGame;
        }

        public void Execute()
        {
            game.item = null;
        }

        public void Undo()
        {
        }
    }
}
