namespace Sprint0
{
    public class QuitCommand : ICommand
    {
        public QuitCommand(){}

        public void SetState() 
        {
            Game1.self.state = 0;
        }

        public void Execute()
        {
            Game1.self.Exit();
        }
    }
}
