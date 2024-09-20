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

        private void LoadSpriteAnimation(string SpriteAnimationName, string SpriteAnimationFilepath)
        {
            SpriteAnimation spriteAnimation = new SpriteAnimation(SpriteAnimationFilepath, textures);
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
            LoadSpriteAnimation("kirby_normal_standing_left", "Content/Images/Sprites/Kirby/kirby_normal_standing_left.csv");
            LoadSpriteAnimation("kirby_normal_standing_right", "Content/Images/Sprites/Kirby/kirby_normal_standing_right.csv");
            LoadSpriteAnimation("kirby_normal_walking_left", "Content/Images/Sprites/Kirby/kirby_normal_walking_left.csv");
            LoadSpriteAnimation("kirby_normal_walking_right", "Content/Images/Sprites/Kirby/kirby_normal_walking_right.csv");

            LoadSpriteAnimation("tile_grass", "Content/Images/Tiles/tile_grass.csv");
            LoadSpriteAnimation("tile_dirt", "Content/Images/Tiles/tile_dirt.csv");
            LoadSpriteAnimation("tile_rocksurface", "Content/Images/Tiles/tile_rocksurface.csv");
            LoadSpriteAnimation("tile_rock", "Content/Images/Tiles/tile_rock.csv");
            LoadSpriteAnimation("tile_platform", "Content/Images/Tiles/tile_platform.csv");
            LoadSpriteAnimation("tile_stoneblock", "Content/Images/Tiles/tile_stoneblock.csv");
            LoadSpriteAnimation("tile_slope_steep_left", "Content/Images/Tiles/tile_slope_steep_left.csv");
            LoadSpriteAnimation("tile_slope_gentle1_left", "Content/Images/Tiles/tile_slope_gentle1_left.csv");
            LoadSpriteAnimation("tile_slope_gentle2_left", "Content/Images/Tiles/tile_slope_gentle2_left.csv");
            LoadSpriteAnimation("tile_slope_gentle2_right", "Content/Images/Tiles/tile_slope_gentle2_right.csv");
            LoadSpriteAnimation("tile_slope_gentle1_right", "Content/Images/Tiles/tile_slope_gentle1_right.csv");
            LoadSpriteAnimation("tile_slope_steep_right", "Content/Images/Tiles/tile_slope_steep_right.csv");
            LoadSpriteAnimation("tile_waterfall", "Content/Images/Tiles/tile_waterfall.csv");
        }

        public string GetSpriteName(string[] states)
        {
            string currentState = states[3] +"_" + states[2] +"_"+ states[1] ; //+"_"+states[0];
            return currentState;
        }
        public Sprite createSprite(string spriteAnimationName)
        {
            return new Sprite(spriteAnimations[spriteAnimationName]);
        }

        public Sprite createSprite(string[] states)
        {
             string spriteAnimationName = states[3] +"_" + states[2] +"_"+ states[1] +"_"+states[0];
            return new Sprite(spriteAnimations[spriteAnimationName]);
        }



    }
}
