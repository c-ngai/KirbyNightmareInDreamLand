using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Collision;
using KirbyNightmareInDreamLand.Commands;
using KirbyNightmareInDreamLand.Controllers;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace KirbyNightmareInDreamLand
{
    public class LevelLoader
    {
        private readonly Game1 _game;
        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;
        private CollisionResponse collisionResponse;

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
            collisionResponse = CollisionResponse.Instance;
        }



        public void LoadAllContent()
        {

            LoadAllTextures();
            LoadAllSpriteAnimations(); // Dependent on textures already loaded

            LoadAllTilemaps();
            LoadAllRooms(); // Dependent on sprite animations and tilemaps already loaded

            LoadAllKeymaps();
            SetCollisionResponses();

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

        public void SetCollisionResponses()
        {
            #region Player-Tile Collisons
            String key1 = "Player";
            String key2 = "Air";
            Action<ICollidable, ICollidable, Rectangle> action1 = TileCollisionActions.BottomAirCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);

            // If we plan on implementing swimming this will need to be modified
            key2 = "Water";
            for (int j = 0; j < Constants.HitBoxes.SIDES; j++)
            {
                collisionResponse.RegisterCollision(key1, key2, (CollisionSide)j, null, null);
            }


            key2 = "Platform";
            action1 = TileCollisionActions.BottomPlatformCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);

            key2 = "Block";
            action1 = TileCollisionActions.BottomBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            action1 = TileCollisionActions.RightBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            action1 = TileCollisionActions.LeftBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            key2 = "SlopeGentle1Left";
            action1 = TileCollisionActions.GentleLeftSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = "SlopeGentle2Left";
            action1 = TileCollisionActions.MediumLeftSlopeCollison;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = "SlopeSteepLeft";
            action1 = TileCollisionActions.SteepLeftSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = "SlopeGentle1Right";
            action1 = TileCollisionActions.GentleRightSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            key2 = "SlopeGentle2Right";
            action1 = TileCollisionActions.MediumRightSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            key2 = "SlopeSteepRight";
            action1 = TileCollisionActions.SteepRightSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            #endregion

            #region Enemy-Tile Collisions
            key1 = "Enemy";
            key2 = "Water";
            action1 = TileCollisionActions.WaterCollision;
            for (int j = 0; j < Constants.HitBoxes.SIDES; j++)
            {
                collisionResponse.RegisterCollision(key1, key2, (CollisionSide)j, action1, null);
            }

            /*
            key2 = "Air";
            action1 = TileCollisionActions.BottomAirCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);*/

            key2 = "Block";
            action1 = TileCollisionActions.BottomBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            action1 = TileCollisionActions.RightBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            action1 = TileCollisionActions.LeftBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            // TODO: add the correct commands for the slope handling;
            key2 = "SlopeSteepLeft";
            action1 = TileCollisionActions.BottomBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = "SlopeGentle1Left";
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = "SlopeGentle2Left";
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = "SlopeGentle2Right";
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            key2 = "SlopeGentle1Right";
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            #endregion

            /*
            #region Projectile Collisions
            key1 = "Projectile";
            key2 = "Block";
            action1 = TileCollisionActions.BottomBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            action1 = TileCollisionActions.RightBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            action1 = TileCollisionActions.LeftBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            #endregion
            */

            #region Player-Enemy Collisions
            key1 = "Enemy";
            key2 = "Player";
            action1 = DynamicCollisionActions.KirbyEnemyCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Top, action1, null);

            key2 = "PlayerAttack";
            action1 = DynamicCollisionActions.EnemyKirbyAttackCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Top, action1, null);

            key1 = "Player";
            key2 = "EnemyAttack";
            action1 = DynamicCollisionActions.KirbyEnemyAttackCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Top, action1, null);

            key2 = "PowerUp";
            action1 = DynamicCollisionActions.KirbyItemCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, null, action1);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, null, action1);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, null, action1);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Top, null, action1);

            
            #endregion

            Debug.WriteLine("Dictionary after collisionMapping");
            foreach (var collision in collisionResponse.collisionMapping)
            {
                Debug.WriteLine(collision);
            }
        }
    }
}
