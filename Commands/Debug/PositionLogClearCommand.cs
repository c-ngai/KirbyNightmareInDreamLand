using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Commands
{
    public class PositionLogClearCommand : ICommand
    {
        public void Execute()
        {
            GameDebug.Instance.ClearPositionLog();
        }
    }
}
