using KirbyNightmareInDreamLand.Commands;
using KirbyNightmareInDreamLand.Controllers;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly Game1 _game;
        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;

        // Dictionary from string to Tilemap. For easily retrieving a tilemap by name.
        public Dictionary<string, int[][]> Tilemaps { get; private set; }

        // Dictionary from string to Room. For easily retrieving a room by name.
        public Dictionary<string, Room> Rooms { get; private set; }

        // Dictionary from string to Keymap. Keymap is a List of Keymappings.
        public Dictionary<string, List<Keymapping>> Keymaps { get; private set; }


        public SpriteFont Font { get; private set; }
        public Texture2D Borders { get; private set; }


        private static readonly LevelLoader _instance = new LevelLoader();
        public static LevelLoader Instance
        {
            get
            {
                return _instance;
            }
        }
        public LevelLoader()
        {
            _game = Game1.Instance;
            _content = _game.Content;
            _graphics = _game.GraphicsDevice;
            Tilemaps = new Dictionary<string, int[][]>();
            Rooms = new Dictionary<string, Room>();
            Keymaps = new Dictionary<string, List<Keymapping>>();
        }



        public void LoadAllContent()
        {

            LoadAllTextures();
            LoadAllSpriteAnimations(); // Dependent on textures already loaded

            LoadAllTilemaps();
            LoadAllRooms(); // Dependent on sprite animations and tilemaps already loaded

            LoadAllKeymaps();

            // Spritefont only for debug functionality
            Font = _content.Load<SpriteFont>("DefaultFont");

            // Border for fullscreen letterboxing
            Borders = new Texture2D(_graphics, 1, 1);
            Borders.SetData(new Color[] { new Color(0, 0, 0, 127) });

        }



        // Loads a texture image given its name and filepath.
        private void LoadTexture(string textureName, string textureFilepath)
        {
            Texture2D texture = _content.Load<Texture2D>(textureFilepath);
            SpriteFactory.Instance.Textures.Add(textureName, texture);
        }

        // Loads all textures from the texture list file.
        public void LoadAllTextures()
        {
            // Open the texture list data file and read its lines into a string array.
            string[] textureFilepaths = File.ReadAllLines(Constants.Filepaths.TextureList);

            // Run through the array and load each texture.
            foreach (string textureFilepath in textureFilepaths)
            {
                string textureName = Path.GetFileNameWithoutExtension(textureFilepath);
                LoadTexture(textureName, textureFilepath);
            }
        }



        // Loads a sprite animation given its name and data.
        private void LoadSpriteAnimation(string spriteAnimationName, SpriteJsonData spriteJsonData)
        {
            SpriteAnimation spriteAnimation = new SpriteAnimation(spriteJsonData, SpriteFactory.Instance.Textures);
            SpriteFactory.Instance.SpriteAnimations.Add(spriteAnimationName, spriteAnimation);
        }

        // Loads all sprite animations from the .json registry.
        public void LoadAllSpriteAnimations()
        {
            // Open the sprite animation data file and deserialize it into a dictionary.
            Dictionary<string, SpriteJsonData> spriteJsonDatas = JsonSerializer.Deserialize<Dictionary<string, SpriteJsonData>>(File.ReadAllText(Constants.Filepaths.SpriteRegistry), new JsonSerializerOptions());

            // Run through the dictionary and load each sprite.
            foreach (KeyValuePair<string, SpriteJsonData> data in spriteJsonDatas)
            {
                LoadSpriteAnimation(data.Key, data.Value);
            }
        }

        

        // Loads a tilemap given its name and filepath.
        private void LoadTilemap(string tilemapName, string tilemapFilepath)
        {
            int[][] tilemap;

            // Read the .csv spreadsheet values into a 2D int array
            List<string> lines = new(File.ReadLines(tilemapFilepath));
            tilemap = new int[lines.Count][];
            for (int y = 0; y < lines.Count; y++)
            {
                string[] values = lines[y].Split(',');
                tilemap[y] = new int[values.Length];
                for (int x = 0; x < values.Length; x++)
                {
                    tilemap[y][x] = Int32.Parse(values[x]);
                }
            }

            Tilemaps.Add(tilemapName, tilemap);
        }

        // Loads all tilemaps from the tilemap list file.
        public void LoadAllTilemaps()
        {
            // Open the texture list data file and read its lines into a string array.
            string[] tilemapFilepaths = File.ReadAllLines(Constants.Filepaths.TilemapList);

            // Run through the array and load each texture.
            foreach (string tilemapFilepath in tilemapFilepaths)
            {
                string tileMapName = Path.GetFileNameWithoutExtension(tilemapFilepath);
                LoadTilemap(tileMapName, tilemapFilepath);
            }
        }



        // Loads a room given its name and data.
        private void LoadRoom(string roomName, RoomJsonData roomJsonData)
        {
            Room room = new Room(roomName, roomJsonData);
            Rooms.Add(roomName, room);
        }

        // Loads all rooms from the .json registry.
        public void LoadAllRooms()
        {
            // Open the room data file and deserialize it into a dictionary.
            Dictionary<string, RoomJsonData> roomJsonDatas = JsonSerializer.Deserialize<Dictionary<string, RoomJsonData>>(File.ReadAllText(Constants.Filepaths.RoomRegistry), new JsonSerializerOptions());

            // Run through the dictionary and load each room.
            foreach (KeyValuePair<string, RoomJsonData> data in roomJsonDatas)
            {
                LoadRoom(data.Key, data.Value);
            }
        }



        // Loads a keymap into the keyboard.
        public void LoadKeymap(string keymapName)
        {
            if (Keymaps.ContainsKey(keymapName))
            {
                // Clear existing key mappings in the controller
                _game.Keyboard.ClearMappings();

                // Register each new key mapping
                foreach (var mapping in Keymaps[keymapName])
                {
                    ICommand command = (ICommand)mapping.CommandConstructorInfo.Invoke(null);
                    _game.Keyboard.RegisterCommand(mapping.Key, mapping.ExecutionType, command);
                }
            }
            else
            {
                Debug.WriteLine($"Keymap '{keymapName}' not found.");
            }
        }

        // Loads a keymapping given its name and data.
        private void LoadKeymapping(KeymappingJsonData keymappingJsonData, List<Keymapping> keymap)
        {
            // Create a new Keymapping object
            Keymapping keymapping = new Keymapping();

            // Fill out its fields using the JSON data strings
            keymapping.Key = (Keys)Enum.Parse(typeof(Keys), keymappingJsonData.Key);
            keymapping.ExecutionType = (ExecutionType)Enum.Parse(typeof(ExecutionType), keymappingJsonData.ExecutionType);
            keymapping.CommandConstructorInfo = Type.GetType("KirbyNightmareInDreamLand.Commands." + keymappingJsonData.Command)?.GetConstructor(Type.EmptyTypes);

            // Add the new keymapping to its respective list.
            if (keymapping.CommandConstructorInfo != null)
            {
                keymap.Add(keymapping);
            }
            else
            {
                Debug.WriteLine("LevelLoader.LoadKeymapping: ERROR: string \"" + keymappingJsonData.Command + "\" returns null from Type.GetType()");
            }
        }

        // Loads all keymaps from the .json registry.
        public void LoadAllKeymaps()
        {
            // Open the keymap data file and deserialize it into a dictionary.
            Dictionary<string, List<KeymappingJsonData>> KeymapJsonDatas = JsonSerializer.Deserialize<Dictionary<string, List<KeymappingJsonData>>>(File.ReadAllText(Constants.Filepaths.KeymapRegistry), new JsonSerializerOptions());

            // Run through each keymap json data in the dictionary
            foreach (KeyValuePair<string, List<KeymappingJsonData>> KeymapJsonData in KeymapJsonDatas)
            {
                // Add new empty Keymap to the Keymaps dictionary.
                Keymaps.Add(KeymapJsonData.Key, new List<Keymapping>());
                // Run through each keymapping json data in the keymap json data
                foreach (KeymappingJsonData keymappingJsonData in KeymapJsonData.Value)
                {
                    // Load the keymapping. Passes in the individual keymapping json data and a reference to the actual keymap List<Keymapping> to add it to.
                    LoadKeymapping(keymappingJsonData, Keymaps[KeymapJsonData.Key]);
                }
            }
        }
        


    }
}
