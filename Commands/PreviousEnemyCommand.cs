namespace KirbyNightmareInDreamLand.Commands
{
    public class PreviousEnemyCommand : ICommand
    {
        private Game1 game;

        public PreviousEnemyCommand()
        {
            this.game = Game1.Instance;
        }

        // Move to the previous enemy in the list
        public void Execute()
        {
            if (game.currentEnemyIndex == 0)
            {
                game.currentEnemyIndex = game.enemyList.Length - 1;
            }
            else
            {
                game.currentEnemyIndex--;
            }
        }
    }

}
