using MasterGame.Block;

namespace MasterGame
{
    public class HideItemCommand : ICommand
    {
        Game1 game;
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
