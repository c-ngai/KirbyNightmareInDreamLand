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
            // Create SoundEffectInstances from the SoundEffects in the passed Sound data
            soundEffectInstance = sound.soundEffect.CreateInstance();
            soundEndBehavior = sound.soundEndBehavior;
            nextSound = sound.nextSound?.CreateInstance();

            // Initialize booleans
            isPlaying = false;
            inLoopSection = false;
            DELETE_ME = false;
            temp = _temp;

            // If this sound loops, set the loop behavior for the SoundEffectInstance to true
            if (soundEndBehavior == SoundEndBehavior.Loop)
            {
                soundEffectInstance.IsLooped = true;
            }
            // If this sound loops after an intro, set the loop behavior for the nextSound SoundEffectInstance to true
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
                // If this is a temporary sound (automatically generated from SoundManager.Play), flag self for deletion
                if (temp)
                {
                    DELETE_ME = true;
                }
            }

            // Update pitch
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



        public void Dispose()
        {
            soundEffectInstance.Dispose();
            nextSound?.Dispose();
            DELETE_ME = true;
        }

    }
}
