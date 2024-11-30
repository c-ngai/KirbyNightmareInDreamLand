using Microsoft.Xna.Framework.Audio;

namespace KirbyNightmareInDreamLand.Audio
{
    public class SoundInstance
    {
        public SoundEffectInstance soundEffectInstance;
        public SoundEndBehavior soundEndBehavior;
        public SoundEffectInstance nextSound;
        public string name;

        public bool isPlaying;
        public bool inLoopSection;
        public bool paused;
        public bool deleteMe;
        public bool temp;

        public SoundInstance(Sound sound, bool isTemporary)
        {
            // create SoundEffectInstances from the SoundEffects in the passed Sound data
            soundEffectInstance = sound.soundEffect.CreateInstance();
            soundEndBehavior = sound.soundEndBehavior;
            nextSound = sound.nextSound?.CreateInstance();
            name = sound.name;

            // initialize booleans
            isPlaying = false;
            inLoopSection = false;
            paused = false;
            deleteMe = false;
            temp = isTemporary;

            // if this sound loops, set the loop behavior for the SoundEffectInstance to true
            if (soundEndBehavior == SoundEndBehavior.Loop)
            {
                soundEffectInstance.IsLooped = true;
            }
            // if this sound loops after an intro, set the loop behavior for the nextSound SoundEffectInstance to true
            else if (soundEndBehavior == SoundEndBehavior.LoopNext)
            {
                nextSound.IsLooped = true;
            }
        }

        public void Update()
        {
            // if the sound has finished playing
            if (isPlaying && !inLoopSection && soundEffectInstance.State == SoundState.Stopped)
            {
                // if soundEndBehavior is PlayNext, then play the next sound
                if (soundEndBehavior == SoundEndBehavior.LoopNext)
                {
                    nextSound.Play();
                    inLoopSection = true;
                }
                // If this is a temporary sound (automatically generated from SoundManager.Play), flag self for deletion
                if (temp)
                {
                    deleteMe = true;
                }
            }

            // update pitch
            soundEffectInstance.Pitch = SoundManager.pitch;
            if (nextSound != null)
            {
                nextSound.Pitch = SoundManager.pitch;
            }
        }

        public void Play()
        {
            inLoopSection = false;
            isPlaying = true;
            soundEffectInstance.Play();
        }

        public void Stop()
        {
            soundEffectInstance.Stop();
            nextSound?.Stop();
            isPlaying = false;
        }

        public void Pause()
        {
            if (isPlaying)
            {
                soundEffectInstance.Pause();
                nextSound?.Pause();
                paused = true;
            }
        }

        public void Resume()
        {
            if (paused)
            {
                if (inLoopSection)
                {
                    nextSound?.Resume();
                }
                else
                {
                    soundEffectInstance.Resume();
                }
                paused = false;
            }
        }

        public void Dispose()
        {
            soundEffectInstance.Dispose();
            nextSound?.Dispose();
            deleteMe = true;
        }

    }
}
