using KirbyNightmareInDreamLand.Audio;
using Microsoft.Xna.Framework.Audio;

namespace KirbyNightmareInDreamLand.Commands
{
    public class AudioToggleMuteCommand : ICommand
    {
        Game1 game;
        public AudioToggleMuteCommand()
        {
            this.game = Game1.Instance;
        }

        public void Execute()
        {
            if (SoundEffect.MasterVolume == 0)
            {
                SoundEffect.MasterVolume = 1;
            }
            else
            {
                SoundEffect.MasterVolume = 0;
                SoundManager.StopAllSounds();
            }
            
        }
    }
}
