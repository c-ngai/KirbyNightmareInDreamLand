using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MasterGame
{
    public class SpriteFactory
    {
        // Dictionary from string to Texture2D. For easily retrieving a texture by name.
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        // Dictionary from string to string. For easily retrieving a sprite filepath by name.
        private static Dictionary<string, string> sprites = new Dictionary<string, string>();

        private Texture2D kirby_normal;
        private Texture2D kirby_beam;
        private Texture2D kirby_spark;
        private Texture2D kirby_fire;


        private static SpriteFactory instance = new SpriteFactory();

        public static SpriteFactory Instance
        {
            get
            {
                return instance;
            }
        }

        private SpriteFactory()
        {
        }

        private void LoadTexture(ContentManager content, Texture2D texture, string TextureName, string TextureFilepath)
        {
            texture = content.Load<Texture2D>(TextureFilepath);
            textures.Add(TextureName, texture);
        }

        public void LoadAllTextures(ContentManager content)
        {
            LoadTexture(content, kirby_normal, "kirby_normal", "Kirby/kirby_normal");
            LoadTexture(content, kirby_beam, "kirby_beam", "Kirby/kirby_beam");
            LoadTexture(content, kirby_spark, "kirby_spark", "Kirby/kirby_spark");
            LoadTexture(content, kirby_fire, "kirby_fire", "Kirby/kirby_fire");

        }

        public void LoadAllSprites()
        {
            sprites.Add("kirby_normal_standing", "Content/Kirby/kirby_normal_standing.csv");
            sprites.Add("kirby_normal_walking", "Content/Kirby/kirby_normal_walking.csv");

        }

        // TODO: clean this up, it's super messy
        public Sprite createSprite(string SpriteName)
        {
            return new Sprite(sprites[SpriteName], textures);
        }

    }
}
