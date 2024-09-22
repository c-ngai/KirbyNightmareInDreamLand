using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace MasterGame {


    public class SpriteFactory
    {
        // Dictionary from string to Texture2D. For easily retrieving a texture by name.
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        // Dictionary from string to string. For easily retrieving a sprite filepath by name.
        private static Dictionary<string, SpriteAnimation> spriteAnimations = new Dictionary<string, SpriteAnimation>();

        private Texture2D kirby_normal;
        private Texture2D kirby_beam;
        private Texture2D kirby_spark;
        private Texture2D kirby_fire;
        private Texture2D Sprint2Tileset;

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

        private void LoadTexture(ContentManager content, Texture2D texture, string TextureName, string TextureFilepath)
        {
            texture = content.Load<Texture2D>(TextureFilepath);
            textures.Add(TextureName, texture);
        }

        private void LoadSpriteAnimation(string SpriteAnimationName, SpriteJsonData spriteJsonData)
        {
            SpriteAnimation spriteAnimation = new SpriteAnimation(spriteJsonData, textures);
            spriteAnimations.Add(SpriteAnimationName, spriteAnimation);
        }

        public void LoadAllTextures(ContentManager content)
        {
            LoadTexture(content, kirby_normal, "kirby_normal", "Images/Sprites/Kirby/kirby_normal");
            LoadTexture(content, kirby_beam, "kirby_beam", "Images/Sprites/Kirby/kirby_beam");
            LoadTexture(content, kirby_spark, "kirby_spark", "Images/Sprites/Kirby/kirby_spark");
            LoadTexture(content, kirby_fire, "kirby_fire", "Images/Sprites/Kirby/kirby_fire");
            LoadTexture(content, Sprint2Tileset, "Sprint2Tileset", "Images/Tiles/Sprint2Tileset");
        }

        public void LoadAllSpriteAnimations()
        {
            // Open the sprite animation data file and deserialize it into a dictionary.
            string spriteFile = "Content/Images/Sprites/SpriteAnimations.json";
            Dictionary<string, SpriteJsonData> SpriteJsonDatas = new Dictionary<string, SpriteJsonData>();
            SpriteJsonDatas = JsonSerializer.Deserialize<Dictionary<string, SpriteJsonData>>(File.ReadAllText(spriteFile), new JsonSerializerOptions());

            // Run through the dictionary and load each pair.
            foreach (KeyValuePair<string, SpriteJsonData> data in SpriteJsonDatas)
            {
                LoadSpriteAnimation(data.Key, data.Value);
                
                Debug.WriteLine("//////////////////////////////");
                Debug.WriteLine("key: " + data.Key);
                Debug.WriteLine("texture: " + data.Value.texture);
                Debug.WriteLine("loopPoint: " + data.Value.loopPoint);
                Debug.WriteLine("flip: " + data.Value.flip);
                Debug.WriteLine("frame count: " + data.Value.frames.Count);
            }

        }

        public Sprite createSprite(string spriteAnimationName)
        {
            return new Sprite(spriteAnimations[spriteAnimationName]);
        }

        //method to get sprite file name from current state 
        public Sprite createSprite(string[] states)
        {
            string spriteAnimationName = states[3] +"_" + states[2] +"_"+ states[1] +"_"+states[0];
            return new Sprite(spriteAnimations[spriteAnimationName]);
        }

    }
}
