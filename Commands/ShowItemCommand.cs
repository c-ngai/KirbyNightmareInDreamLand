namespace KirbyNightmareInDreamLand.Commands
{
    public class ShowItemCommand : ICommand
    {
        Game1 game;
        public ShowItemCommand(Game1 currentGame)
        {
            game = currentGame;
        }

        public void Execute()
        {
            game.LoadItem();
        }

        public void Undo()
        {
        }
    }
}
