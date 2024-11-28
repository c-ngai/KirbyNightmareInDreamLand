

using KirbyNightmareInDreamLand.Audio;

namespace KirbyNightmareInDreamLand.Commands
{
    public class PauseCommand : ICommand
    {

        public void Execute()
        {
            if (Game1.Instance.Level.IsCurrentState("KirbyNightmareInDreamLand.GameState.GamePausedState"))
            {
                SoundManager.ResumeAllSounds();
                SoundManager.Play("select");
                Game1.Instance.Level.UnpauseLevel();
            }
            else
            {
                SoundManager.PauseAllSounds();
                SoundManager.Play("pause");
                Game1.Instance.Level.PauseLevel();
            }           
        }

    }
}