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
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        // Dictionary from string to SpriteAnimation. For easily retrieving a sprite animation by name.
        private static Dictionary<string, SpriteAnimation> spriteAnimations;

        private static SpriteFactory instance = new SpriteFactory();

        // Storing a reference to the current game to pass into sprites for retrieving graphics info.
        private Game1 game;

        public static SpriteFactory Instance
        {
            get
            {
                return instance;
            }
        }

        public SpriteFactory()
        {
            spriteAnimations = new Dictionary<string, SpriteAnimation>();
        }



        // Loads a texture image given its name and filepath.
        private void LoadTexture(ContentManager content, string TextureName, string TextureFilepath)
        {
            Texture2D texture = content.Load<Texture2D>(TextureFilepath);
            textures.Add(TextureName, texture);
        }



        // Loads a sprite animation given its name and data.
        private void LoadSpriteAnimation(string SpriteAnimationName, SpriteJsonData spriteJsonData)
        {
            SpriteAnimation spriteAnimation = new SpriteAnimation(spriteJsonData, textures);
            spriteAnimations.Add(SpriteAnimationName, spriteAnimation);
        }



        //level loader pulls this open and loads the fatory
        //factory only knows how to build sprites not what sprites it is building
        //reference to graphics and dictionaries get build.
        //data problem, take it to level loader !!
        //tear it out early

        // Loads all textures from the texture list file.
        public void LoadAllTextures(ContentManager content, Game1 game)
        {
            // Loads the current game
            this.game = game;
            
            // Open the texture list data file and read its lines into a string array.
            string textureList = "Content/Images/Textures.txt";
            string[] textureFilepaths = File.ReadAllLines(textureList);

            // Run through the array and load each texture.
            foreach (string textureFilepath in textureFilepaths)
            {
                string textureName = Path.GetFileNameWithoutExtension(textureFilepath);
                LoadTexture(content, textureName, textureFilepath);
            }
        }



        // Loads all sprite animations from the .json file. -- goes to level loader eventually
        public void LoadAllSpriteAnimations()
        {
            // Open the sprite animation data file and deserialize it into a dictionary.
            string spriteFile = "Content/Images/SpriteAnimations.json";
            Dictionary<string, SpriteJsonData> SpriteJsonDatas = JsonSerializer.Deserialize<Dictionary<string, SpriteJsonData>>(File.ReadAllText(spriteFile), new JsonSerializerOptions());

            // Run through the dictionary and load each sprite.
            foreach (KeyValuePair<string, SpriteJsonData> data in SpriteJsonDatas)
            {
                LoadSpriteAnimation(data.Key, data.Value);
            }
        }



        // Returns a new sprite object from a sprite animation's name.
        public Sprite CreateSprite(string spriteAnimationName)
        {
            if (spriteAnimations.ContainsKey(spriteAnimationName))
            {
                //System.Console.WriteLine(spriteAnimationName );
                return new Sprite(spriteAnimations[spriteAnimationName], game);
            }
            else
            {
                Debug.WriteLine("INVALID SPRITE NAME: " + spriteAnimationName); //debug line
                return new Sprite(spriteAnimations["invalidspritename"], game);
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
