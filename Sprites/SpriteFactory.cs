using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace MasterGame {


    public class SpriteFactory
    {
        
        // Dictionary from string to Texture2D. For easily retrieving a texture by name.
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        // Dictionary from string to SpriteAnimation. For easily retrieving a sprite animation by name.
        private static Dictionary<string, SpriteAnimation> spriteAnimations = new Dictionary<string, SpriteAnimation>();



        private static SpriteFactory instance = new SpriteFactory();

        public static SpriteFactory Instance
        {
            get
            {
                return instance;
            }
        }



        public SpriteFactory()
        {
        }



        // Loads a texture image given its name and filepath.
        private void LoadTexture(string TextureName, string TextureFilepath)
        {
            ContentManager content = Game1.self.Content;
            Texture2D texture = content.Load<Texture2D>(TextureFilepath);
            textures.Add(TextureName, texture);
        }



        // Loads a sprite animation given its name and data.
        private void LoadSpriteAnimation(string SpriteAnimationName, SpriteJsonData spriteJsonData)
        {
            SpriteAnimation spriteAnimation = new SpriteAnimation(spriteJsonData, textures);
            spriteAnimations.Add(SpriteAnimationName, spriteAnimation);
        }



        // Loads all textures from the texture list file.
        public void LoadAllTextures(ContentManager content)
        {
            // Open the texture list data file and read its lines into a string array.
            string textureList = "Content/Images/Textures.txt";
            string[] textureFilepaths = File.ReadAllLines(textureList);

            // Run through the array and load each texture.
            foreach (string textureFilepath in textureFilepaths)
            {
                string textureName = Path.GetFileNameWithoutExtension(textureFilepath);
                LoadTexture(textureName, textureFilepath);
            }
        }



        // Loads all sprite animations from the .json file.
        public void LoadAllSpriteAnimations()
        {
            // Open the sprite animation data file and deserialize it into a dictionary.
            string spriteFile = "Content/Images/SpriteAnimations.json";
            Dictionary<string, SpriteJsonData> SpriteJsonDatas = new Dictionary<string, SpriteJsonData>();
            SpriteJsonDatas = JsonSerializer.Deserialize<Dictionary<string, SpriteJsonData>>(File.ReadAllText(spriteFile), new JsonSerializerOptions());

            // Run through the dictionary and load each sprite.
            foreach (KeyValuePair<string, SpriteJsonData> data in SpriteJsonDatas)
            {
                LoadSpriteAnimation(data.Key, data.Value);
            }
        }



        // Returns a new sprite object from a sprite animation's name.
        public Sprite createSprite(string spriteAnimationName)
        {
            if (spriteAnimations.ContainsKey(spriteAnimationName))
                return new Sprite(spriteAnimations[spriteAnimationName]);
            else
                return new Sprite(spriteAnimations["invalidspritename"]);
        }



        //method to get sprite file name from current state 
        public Sprite createSprite(string[] states)
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

            if (spriteAnimations.ContainsKey(spriteAnimationName))
                return new Sprite(spriteAnimations[spriteAnimationName]);
            else
                return new Sprite(spriteAnimations["invalidspritename"]);
        }

    }
}
