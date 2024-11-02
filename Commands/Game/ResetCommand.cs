using KirbyNightmareInDreamLand.Levels;
namespace KirbyNightmareInDreamLand.Commands
{
    public class ResetCommand : ICommand
    {
        private ObjectManager manager;
        private Level level;
        public ResetCommand()
        {
            manager = ObjectManager.Instance;
            level = Game1.Instance.Level;
        }

        public void Execute()
        {
            level.LoadRoom(level.CurrentRoom.Name);
        }
    }
}
