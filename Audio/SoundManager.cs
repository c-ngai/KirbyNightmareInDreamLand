using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static KirbyNightmareInDreamLand.Constants;

namespace KirbyNightmareInDreamLand.Audio
{
    public class SoundManager
    {
        // Dictionary of all Sounds by name
        public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();
        // List of all currently active SoundInstances
        public static List<SoundInstance> SoundInstances = new List<SoundInstance>();
        // Pitch to play sounds at based on target framerate
        public static float pitch;

        private static SoundInstance song;

        public static void Play(string name)
        {
            // If the Sounds dictionary contains a sound for the given name, generate a temporary instance of it
            // that will automatically flag itself for deletion after playing.
            if (Sounds.ContainsKey(name))
            {
                if (Sounds[name].soundEndBehavior == SoundEndBehavior.Nothing)
                {
                    SoundInstance newSoundInstance = new SoundInstance(Sounds[name], true);

                    // Add reference to the sound instances list
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
            // If the Sounds dictionary contains a sound for the given name, return an instance of it.
            if (Sounds.ContainsKey(name))
            {
                SoundInstance newSoundInstance = new SoundInstance(Sounds[name], false);

                // Add reference to the sound instances list
                SoundInstances.Add(newSoundInstance);

                return newSoundInstance;
            }
            // Otherwise, return an error.
            else
            {
                Debug.WriteLine(" [ERROR] Sound \"" + name + "\" does not exist in the files. (Check: was it added to Content.mgcb too?)");
                return null;
            }
        }

        public static void Update()
        {
            // Update pitch
            pitch = (float)(Game1.Instance.TARGET_FRAMERATE - 60) / 60;

            List<SoundInstance> OldSoundInstances = new List<SoundInstance>(SoundInstances);
            // Update each SoundInstance
            foreach (SoundInstance soundInstance in OldSoundInstances)
            {
                soundInstance.Update();
            }

            song?.Update();

            // Remove all sound instances flagged for deletion
            SoundInstances.RemoveAll(x => x.DELETE_ME);
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
