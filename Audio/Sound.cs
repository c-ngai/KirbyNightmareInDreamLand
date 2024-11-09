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
    // Behavior when the end of a sound is reached
    public enum SoundEndBehavior { Nothing, Loop, LoopNext }


    public class Sound
    {
        public SoundEffect soundEffect;
        public SoundEndBehavior soundEndBehavior;
        public SoundEffect nextSound;

        public Sound(SoundEffect _soundEffect, SoundEndBehavior _soundEndBehavior, SoundEffect _nextSound)
        {
            soundEffect = _soundEffect;
            soundEndBehavior = _soundEndBehavior;
            nextSound = _nextSound;
        }
    }
}
