using System.Collections.Generic;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Audio
{
    public class SoundManager
    {
        // dictionary of all Sounds by name
        public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();
        // list of all currently active SoundInstances
        public static List<SoundInstance> SoundInstances = new List<SoundInstance>();
        // pitch to play sounds at based on target framerate
        public static float pitch;

        private static SoundInstance song;

        public static void Play(string name)
        {
            // if the Sounds dictionary contains a sound for the given name, generate a temporary instance of it
            // that will automatically flag itself for deletion after playing.
            if (Sounds.ContainsKey(name))
            {
                if (Sounds[name].soundEndBehavior == SoundEndBehavior.Nothing)
                {
                    SoundInstance newSoundInstance = new SoundInstance(Sounds[name], true);

                    // add reference to the sound instances list
                    SoundInstances.Add(newSoundInstance);

                    newSoundInstance.Play();
                }
                else
                {
                    Debug.WriteLine(" [ERROR] Sound \"" + name + "\" loops. You cannot directly play a looping sound, please create an instance first.");
                }
            }
            else
            {
                Debug.WriteLine(" [ERROR] Sound \"" + name + "\" does not exist in the files. (Check: was it added to Content.mgcb too?)");
            }
        }

        public static void PlaySong(string name)
        {
            if (song?.name != name)
            {
                song?.Dispose();
                if (name != "" && name != null)
                {
                    song = new SoundInstance(Sounds[name], false);
                    song.Play();
                }
                else
                {
                    song = null;
                }
            }
        }

        public static SoundInstance CreateInstance(string name)
        {
            // if the Sounds dictionary contains a sound for the given name, return an instance of it.
            if (Sounds.ContainsKey(name))
            {
                SoundInstance newSoundInstance = new SoundInstance(Sounds[name], false);

                // Add reference to the sound instances list
                SoundInstances.Add(newSoundInstance);

                return newSoundInstance;
            }
            // otherwise, return an error.
            else
            {
                Debug.WriteLine(" [ERROR] Sound \"" + name + "\" does not exist in the files. (Check: was it added to Content.mgcb too?)");
                return null;
            }
        }

        public static void Update()
        {
            // update pitch
            pitch = (float)(Game1.Instance.TARGET_FRAMERATE - Constants.SoundValues.PITCH_ADJUSTMENT) / Constants.SoundValues.PITCH_ADJUSTMENT;

            List<SoundInstance> OldSoundInstances = new List<SoundInstance>(SoundInstances);
            // update each SoundInstance
            foreach (SoundInstance soundInstance in OldSoundInstances)
            {
                soundInstance.Update();
            }

            song?.Update();

            // remove all sound instances flagged for deletion
            SoundInstances.RemoveAll(x => x.deleteMe);
        }

        public static void PauseAllSounds()
        {
            foreach(SoundInstance soundInstance in SoundInstances)
            {
                soundInstance.Pause();
            }
        }

        public static void ResumeAllSounds()
        {
            foreach (SoundInstance soundInstance in SoundInstances)
            {
                soundInstance.Resume();
            }
        }

        public static void StopAllSounds()
        {
            foreach (SoundInstance soundInstance in SoundInstances)
            {
                soundInstance.Stop();
            }
        }

    }
}
