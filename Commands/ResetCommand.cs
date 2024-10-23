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

        // is there a better way to do this beyond resetting every single game object?
        public void Execute()
        {
            manager.LoadKirby();
            level.LoadRoom(level.CurrentRoom.Name);
        }
    }
}
