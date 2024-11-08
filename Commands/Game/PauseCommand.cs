

namespace KirbyNightmareInDreamLand.Commands
{
    public class PauseCommand : ICommand
    {

        public void Execute()
        {
            if (Game1.Instance.Level.IsCurrentState("KirbyNightmareInDreamLand.GameState.GamePausedState"))
            {
                Game1.Instance.Level.UnpauseLevel();
            }
            else
            {
                Game1.Instance.Level.PauseLevel();
            }           
        }
    }
}