using Microsoft.Xna.Framework.Audio;

namespace KirbyNightmareInDreamLand.Audio
{
    // Behavior when the end of a sound is reached
    public enum SoundEndBehavior { Nothing, Loop, LoopNext }


    public class Sound
    {
        public SoundEffect soundEffect;
        public SoundEndBehavior soundEndBehavior;
        public SoundEffect nextSound;
        public string name;

        public Sound(SoundEffect _soundEffect, SoundEndBehavior _soundEndBehavior, SoundEffect _nextSound, string _name)
        {
            soundEffect = _soundEffect;
            soundEndBehavior = _soundEndBehavior;
            nextSound = _nextSound;
            name = _name;
        }
    }
}
