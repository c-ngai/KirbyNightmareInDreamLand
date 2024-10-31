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

namespace KirbyNightmareInDreamLand
{
    public class SoundManager
    {
        // Dictionary of all Sounds by name
        public static Dictionary<string, Sound> Sounds = new Dictionary<string, Sound>();
        // List of all currently active SoundInstances
        public static List<SoundInstance> SoundInstances = new List<SoundInstance>();

        public static void Play(string name)
        {
            if (Sounds.ContainsKey(name))
            {
                if (Sounds[name].soundEndBehavior == SoundEndBehavior.Nothing)
                {
                    SoundInstance newSoundInstance = new SoundInstance(Sounds[name], true);
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

        public static SoundInstance CreateInstance(string name)
        {
            if (Sounds.ContainsKey(name))
            {
                SoundInstance newSoundInstance = new SoundInstance(Sounds[name], false);
                return newSoundInstance;
            }
            else
            {
                Debug.WriteLine(" [ERROR] Sound \"" + name + "\" does not exist in the files. (Check: was it added to Content.mgcb too?)");
                return null;
            }
        }

        public static void Update()
        {
            List<SoundInstance> OldSoundInstances = new List<SoundInstance>(SoundInstances);
            // Update each SoundInstance
            foreach (SoundInstance soundInstance in OldSoundInstances)
            {
                soundInstance.Update();
            }

            // Remove all sound instances flagged for deletion
            SoundInstances.RemoveAll(x => x.DELETE_ME);
        }

    }



    public class Sound
    {
        public SoundEffect soundEffect;
        public SoundEndBehavior soundEndBehavior;
        public SoundEffect nextSound;

        public Sound (SoundEffect _soundEffect, SoundEndBehavior _soundEndBehavior, SoundEffect _nextSound)
        {
            soundEffect = _soundEffect;
            soundEndBehavior = _soundEndBehavior;
            nextSound = _nextSound;
        }
    }

    public class SoundInstance
    {
        public SoundEffectInstance soundEffectInstance;
        public SoundEndBehavior soundEndBehavior;
        public SoundEffectInstance nextSound;

        public bool isPlaying;
        public bool inLoopSection;
        public bool DELETE_ME;
        public bool temp;


        public SoundInstance(Sound sound, bool _temp)
        {
            soundEffectInstance = sound.soundEffect.CreateInstance();
            soundEndBehavior = sound.soundEndBehavior;
            nextSound = sound.nextSound?.CreateInstance();

            isPlaying = false;
            inLoopSection = false;
            DELETE_ME = false;
            temp = _temp;

            if (soundEndBehavior == SoundEndBehavior.Loop)
            {
                soundEffectInstance.IsLooped = true;
            }
            else if (soundEndBehavior == SoundEndBehavior.LoopNext)
            {
                nextSound.IsLooped = true;
            }

            // Add reference to self to the sound instances list in SoundManager
            SoundManager.SoundInstances.Add(this);
        }



        public void Update()
        {
            // If the sound has finished playing
            if (isPlaying && !inLoopSection && soundEffectInstance.State == SoundState.Stopped)
            {
                // If soundEndBehavior is PlayNext, then play the next sound
                if (soundEndBehavior == SoundEndBehavior.LoopNext)
                {
                    nextSound.Play();
                    inLoopSection = true;
                }
                // Flag self for deletion
                if (temp)
                {
                    DELETE_ME = true;
                }

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

        public void Dispose()
        {
            soundEffectInstance.Dispose();
            nextSound?.Dispose();
            DELETE_ME = true;
        }
    }

    public enum SoundEndBehavior { Nothing, Loop, LoopNext }

}
