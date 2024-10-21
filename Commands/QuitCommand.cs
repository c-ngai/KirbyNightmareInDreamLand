namespace KirbyNightmareInDreamLand.Commands
{
    public class QuitCommand : ICommand
    {
        private Game1 myGame;
        public QuitCommand()
        {
            myGame = Game1.Instance;
        }

        public void Execute()
        {
            myGame.Exit();
        }
    }
}
