using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace KirbyNightmareInDreamLand
{
    public class LevelLoader
    {
        // Dictionary from string to Room. For easily retrieving a room by name.
        public Dictionary<string, int[][]> Tilemaps { get; private set; }

        // Dictionary from string to Room. For easily retrieving a room by name.
        public Dictionary<string, Room> Rooms { get; private set; }


        public SpriteFont font { get; private set; }
        public Texture2D borders { get; private set; }


        private static LevelLoader instance = new LevelLoader();
        public static LevelLoader Instance
        {
            get
            {
                return instance;
            }
        }
        public LevelLoader()
        {
            Tilemaps = new Dictionary<string, int[][]>();
            Rooms = new Dictionary<string, Room>();
        }



        public void LoadAllContent(Game1 game, ContentManager content, GraphicsDevice graphics)
        {

            LoadAllTextures(content, game);
            LoadAllSpriteAnimations();
            SpriteDebug.Instance.Load(graphics);

            LoadAllTilemaps();
            LoadAllRooms();

            font = content.Load<SpriteFont>("DefaultFont");
            borders = new Texture2D(graphics, 1, 1);
            borders.SetData(new Color[] { new Color(0, 0, 0, 127) });

        }



        // Loads a texture image given its name and filepath.
        private void LoadTexture(ContentManager content, string TextureName, string TextureFilepath)
        {
            Texture2D texture = content.Load<Texture2D>(TextureFilepath);
            SpriteFactory.Instance.Textures.Add(TextureName, texture);
        }

        // Loads all textures from the texture list file.
        public void LoadAllTextures(ContentManager content, Game1 game)
        {
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



        // Loads a sprite animation given its name and data.
        private void LoadSpriteAnimation(string SpriteAnimationName, SpriteJsonData spriteJsonData)
        {
            SpriteAnimation spriteAnimation = new SpriteAnimation(spriteJsonData, SpriteFactory.Instance.Textures);
            SpriteFactory.Instance.SpriteAnimations.Add(SpriteAnimationName, spriteAnimation);
        }

        // Loads all sprite animations from the .json file.
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

        

        // Loads a tilemap given its name and filepath.
        private void LoadTilemap(string TilemapName, string TilemapFilepath)
        {
            int[][] Tilemap;

            // Read the .csv spreadsheet values into a 2D int array
            List<string> lines = new(File.ReadLines(TilemapFilepath));
            Tilemap = new int[lines.Count][];
            for (int y = 0; y < lines.Count; y++)
            {
                string[] values = lines[y].Split(',');
                Tilemap[y] = new int[values.Length];
                for (int x = 0; x < values.Length; x++)
                {
                    Tilemap[y][x] = Int32.Parse(values[x]);
                }
            }

            Tilemaps.Add(TilemapName, Tilemap);
        }

        // Loads all tilemaps from the tilemap list file.
        public void LoadAllTilemaps()
        {
            // Open the texture list data file and read its lines into a string array.
            string TilemapList = "Content/Tilemaps.txt";
            string[] TilemapFilepaths = File.ReadAllLines(TilemapList);

            // Run through the array and load each texture.
            foreach (string TilemapFilepath in TilemapFilepaths)
            {
                string TileMapName = Path.GetFileNameWithoutExtension(TilemapFilepath);
                LoadTilemap(TileMapName, TilemapFilepath);
            }
        }



        // Loads a room given its name and data.
        private void LoadRoom(string roomName, RoomJsonData roomJsonData)
        {
            Room room = new Room(roomJsonData);
            Rooms.Add(roomName, room);
        }

        // Loads all rooms from the .json file.
        public void LoadAllRooms()
        {
            // Open the sprite animation data file and deserialize it into a dictionary.
            string roomFile = "Content/Rooms.json";
            Dictionary<string, RoomJsonData> RoomJsonDatas = JsonSerializer.Deserialize<Dictionary<string, RoomJsonData>>(File.ReadAllText(roomFile), new JsonSerializerOptions());

            // Run through the dictionary and load each sprite.
            foreach (KeyValuePair<string, RoomJsonData> data in RoomJsonDatas)
            {
                LoadRoom(data.Key, data.Value);
            }
        }

        

    }
}
