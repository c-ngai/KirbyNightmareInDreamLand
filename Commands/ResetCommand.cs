namespace KirbyNightmareInDreamLand.Commands
{
    public class ResetCommand : ICommand
    {
        private ObjectManager manager;
        public ResetCommand()
        {
            manager = ObjectManager.Instance;
        }

        // is there a better way to do this beyond resetting every single game object?
        public void Execute()
        {
            manager.LoadObjects();
        }
    }
}
