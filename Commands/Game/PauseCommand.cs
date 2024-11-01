

namespace KirbyNightmareInDreamLand.Commands
{
    public class PauseCommand : ICommand
    {

        public void Execute()
        {
            Game1.Instance.PAUSED = !Game1.Instance.PAUSED;
        }
    }
}