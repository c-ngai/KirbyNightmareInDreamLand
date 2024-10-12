using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Sprites
{
    public class SpriteFactory
    {
        // Dictionary from string to Texture2D. For easily retrieving a texture by name.
        public Dictionary<string, Texture2D> Textures { get; private set; }

        // Dictionary from string to SpriteAnimation. For easily retrieving a sprite animation by name.
        public Dictionary<string, SpriteAnimation> SpriteAnimations { get; private set; }


        private static SpriteFactory instance = new SpriteFactory();

        // Storing a reference to the current game to pass into sprites for retrieving graphics info.
        private Game1 _game;

        public static SpriteFactory Instance
        {
            get
            {
                return instance;
            }
        }

        public SpriteFactory()
        {
            Textures = new Dictionary<string, Texture2D>();
            SpriteAnimations = new Dictionary<string, SpriteAnimation>();
        }


        /* TODO: This is probably bad. The SpriteFactory needs a reference to the current Game1 object
         * to pass into sprites, so maybe SpriteFactory shouldn't be a singleton then? Either that or
         * the reference to the current Game1 should be passed in through the one calling createSprite,
         * which would require everyone calling it to have that in the first place. Idk. */
        public void LoadGame(Game1 game)
        {
            _game = game;
        }

        // Returns a new sprite object from a sprite animation's name.
        public Sprite CreateSprite(string spriteAnimationName)
        {
            // Grab reference to sprite animation dictionary from LevelLoader
            Dictionary<string, SpriteAnimation> spriteAnimations = SpriteAnimations;

            if (spriteAnimations.ContainsKey(spriteAnimationName))
            {
                //System.Console.WriteLine(spriteAnimationName );
                return new Sprite(spriteAnimations[spriteAnimationName], _game);
            }
            else
            {
                Debug.WriteLine("INVALID SPRITE NAME: " + spriteAnimationName); //debug line
                return new Sprite(spriteAnimations["invalidspritename"], _game);
            }
        }



        //method to get sprite file name from current state
        //override method should be undone 

        // Returns a new sprite object from an array of state strings.
        public Sprite CreateSprite(string[] states)
        {
            // Create a single string to combine all the strings of states into one, with underscores between.
            string spriteAnimationName = "";
            for (int i = 0; i < states.Length; i++)
            {
                spriteAnimationName += states[i];
                if (i < states.Length - 1)
                {
                    spriteAnimationName += "_";
                }
            }
            //System.Console.WriteLine(spriteAnimationName );
            return CreateSprite(spriteAnimationName);
        }

    }
}
