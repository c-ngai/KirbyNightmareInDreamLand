namespace KirbyNightmareInDreamLand.Commands
{
    public class HideItemCommand : ICommand
    {
        private ObjectManager manager;
        public HideItemCommand()
        {
            manager = ObjectManager.Instance;
        }

        public void Execute()
        {
            manager.Item = null;
        }
    }
}
