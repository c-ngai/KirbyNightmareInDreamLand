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
using static KirbyNightmareInDreamLand.Constants;

namespace KirbyNightmareInDreamLand
{
    public class Sound
    {

        public static Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();

        public static void Play(string name)
        {
            if (Sounds.ContainsKey(name))
            {
                Sounds[name].Play();
            }
            else
            {
                Debug.WriteLine("ERROR: Sound \"" + name + "\" does not exist in the files. (Check: was it added to Content.mgcb too?)");
            }
        }

        public static SoundEffectInstance CreateInstance(string name)
        {
            if (Sounds.ContainsKey(name))
            {
                return Sounds[name].CreateInstance();
            }
            else
            {
                Debug.WriteLine("ERROR: Sound \"" + name + "\" does not exist in the files. (Check: was it added to Content.mgcb too?)");
                return null;
            }
        }

    }
}
