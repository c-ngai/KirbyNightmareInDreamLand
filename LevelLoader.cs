using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Collision;
using KirbyNightmareInDreamLand.Commands;
using KirbyNightmareInDreamLand.Controllers;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using static KirbyNightmareInDreamLand.Constants;
using System.Reflection;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand
{
    public sealed class LevelLoader
    {
        private readonly Game1 _game;
        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;
        private CollisionResponse collisionResponse;
        private ObjectManager manager;

        // Dictionary from string to Tilemap. For easily retrieving a tilemap by name.
        public Dictionary<string, int[][]> Tilemaps { get; private set; }

        // Dictionary from string to Room. For easily retrieving a room by name.
        public Dictionary<string, Room> Rooms { get; private set; }

        // Dictionary from string to Keymap. Keymap is a List of Keymappings.
        public Dictionary<string, List<Keymapping>> Keymaps { get; private set; }

        // Dictionary from string to Buttonmap. Keymap is a List of Buttonmappings.
        public Dictionary<string, List<Buttonmapping>> Buttonmaps { get; private set; }

        // Dictionary from string to Dictionary<string, Rectangle>.
        // The outer dictionary takes object type (kirby, waddledee, etc) and the inner
        // dictionary takes pose. Each type is required to have a "default" hitbox for
        // when pose is unspecified.
        public Dictionary<string, Dictionary<string, Rectangle>> Hitboxes { get; private set; }


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
            Buttonmaps = new Dictionary<string, List<Buttonmapping>>();
            Hitboxes = new Dictionary<string, Dictionary<string, Rectangle>>();
            collisionResponse = CollisionResponse.Instance;
            manager = ObjectManager.Instance;
        }



        public void LoadAllContent()
        {

            LoadAllTextures();
            LoadAllSpriteAnimations(); // Dependent on textures already loaded

            LoadAllSounds();

            LoadAllTilemaps();
            LoadAllRooms(); // Dependent on sprite animations and tilemaps already loaded

            LoadAllKeymaps();
            LoadAllButtonmaps();

            LoadAllHitboxes();
            SetCollisionResponses();


            // Spritefont only for debug functionality
            Font = _content.Load<SpriteFont>("DefaultFont");

            // Border for fullscreen letterboxing
            Borders = new Texture2D(_graphics, 1, 1);
            Borders.SetData(new Color[] { Color.Black });

        }

        #region Kirby
        public void LoadKirby()
        {
            // Ensures player list is empty
            manager.ClearPlayerList();
            manager.AddKirby(new Player(new Vector2(Constants.Kirby.STARTINGXPOSITION, Constants.Graphics.FLOOR), 0));
        }
        #endregion


        #region Textures/Sprites
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
        #endregion

        #region Audio
        public void LoadAllSounds()
        {
            // For each file in Content/Audio
            foreach (string filepath in Directory.GetFiles(Constants.Filepaths.AudioDirectory, "*", SearchOption.AllDirectories))
            {
                //Debug.WriteLine("Loading sound: " + filepath);

                // Generate the content path as the relative path from /Content/ without a file extension (because monogame is dumb)
                string directory = Path.GetDirectoryName(Path.GetRelativePath("Content", filepath));
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filepath);
                string contentPath = Path.Combine(directory, fileNameWithoutExtension);

                // Load sound effect, and initialize default end behavior and next sound to nothing/null.
                SoundEffect soundEffect = _content.Load<SoundEffect>(contentPath);
                SoundEndBehavior soundEndBehavior = SoundEndBehavior.Nothing;
                SoundEffect nextSound = null;

                // Create a new Sound object from these three parameters and add it to Sounds
                Sound sound = new Sound(soundEffect, soundEndBehavior, nextSound);
                SoundManager.Sounds.Add(fileNameWithoutExtension, sound);
            }

            // Set looping sound behaviors. YES this should probably be data driven, but other stuff is more pressing rn.
            SoundManager.Sounds["inhale"].soundEndBehavior = SoundEndBehavior.LoopNext;
            SoundManager.Sounds["inhale"].nextSound = SoundManager.Sounds["inhale_loop"].soundEffect;
            SoundManager.Sounds["inhale_loop"].soundEndBehavior = SoundEndBehavior.Loop;

            SoundManager.Sounds["kirbyfireattack"].soundEndBehavior = SoundEndBehavior.LoopNext;
            SoundManager.Sounds["kirbyfireattack"].nextSound = SoundManager.Sounds["kirbyfireattack_loop"].soundEffect;
            SoundManager.Sounds["kirbyfireattack_loop"].soundEndBehavior = SoundEndBehavior.Loop;

            SoundManager.Sounds["kirbysparkattack"].soundEndBehavior = SoundEndBehavior.Loop;

            SoundManager.Sounds["sparkyattack"].soundEndBehavior = SoundEndBehavior.Loop;

            SoundManager.Sounds["hotheadflamethrowerattack"].soundEndBehavior = SoundEndBehavior.Loop;

            SoundManager.Sounds["song_vegetablevalley"].soundEndBehavior = SoundEndBehavior.LoopNext;
            SoundManager.Sounds["song_vegetablevalley"].nextSound = SoundManager.Sounds["song_vegetablevalley_loop"].soundEffect;
            SoundManager.Sounds["song_vegetablevalley_loop"].soundEndBehavior = SoundEndBehavior.Loop;
        }
        #endregion

        #region Rooms/Tilemaps
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
        #endregion

        #region Keyboard
        // Loads a keymap into the keyboard controller.
        public void LoadKeymap(string keymapName)
        {
            if (Keymaps.ContainsKey(keymapName))
            {
                // Clear existing key mappings in the controller
                _game.Keyboard.ClearMappings();

                // Register each new key mapping
                foreach (var mapping in Keymaps[keymapName])
                {
                    ICommand command;
                    if (mapping.PlayerIndex != null)
                    {
                        command = (ICommand)mapping.CommandConstructorInfo.Invoke(new object[] { mapping.PlayerIndex });
                    }
                    else
                    {
                        command = (ICommand)mapping.CommandConstructorInfo.Invoke(null);
                    }
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
            if (keymappingJsonData.PlayerIndex == null)
            {
                keymapping.CommandConstructorInfo = Type.GetType("KirbyNightmareInDreamLand.Commands." + keymappingJsonData.Command)?.GetConstructor(Type.EmptyTypes);
            }
            else
            {
                keymapping.CommandConstructorInfo = Type.GetType("KirbyNightmareInDreamLand.Commands." + keymappingJsonData.Command)?.GetConstructor( new Type[]{ typeof(int) } );
            }
            keymapping.PlayerIndex = keymappingJsonData.PlayerIndex;


            // Add the new keymapping to its respective list.
            if (keymapping.CommandConstructorInfo != null)
            {
                keymap.Add(keymapping);
            }
            else
            {
                Debug.WriteLine(" [ERROR] LevelLoader.LoadKeymapping: string \"" + keymappingJsonData.Command + "\" returns null from Type.GetType()");
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
        #endregion

        #region Gamepad
        // Loads a buttonmap into the gamepad controller.
        public void LoadButtonmap(string buttonmapName)
        {
            if (Buttonmaps.ContainsKey(buttonmapName))
            {
                // Clear existing button mappings in the controller
                _game.Gamepad.ClearMappings();

                // Register each new button mapping
                foreach (Buttonmapping mapping in Buttonmaps[buttonmapName])
                {
                    _game.Gamepad.RegisterCommand(mapping.Button, mapping.ExecutionType, mapping.CommandConstructorInfo);
                }
            }
            else
            {
                Debug.WriteLine($"Buttonmap '{buttonmapName}' not found.");
            }
        }

        // Loads a buttonmapping given its name and data.
        private void LoadButtonmapping(ButtonmappingJsonData buttonmappingJsonData, List<Buttonmapping> buttonmap)
        {
            // Create a new Buttonmapping object
            Buttonmapping buttonmapping = new Buttonmapping();

            // Fill out its fields using the JSON data strings
            buttonmapping.Button = (Buttons)Enum.Parse(typeof(Buttons), buttonmappingJsonData.Button);
            buttonmapping.ExecutionType = (ExecutionType)Enum.Parse(typeof(ExecutionType), buttonmappingJsonData.ExecutionType);
            // Set ConstructorInfo to constructor with no parameters. If that returns null, then try a constructor that takes one integer.
            // (ASSUMPTION THAT ALL COMMAND CONSTRUCTORS TAKE EITHER NO PARAMETERS OR ONLY ONE INTEGER)
            buttonmapping.CommandConstructorInfo = Type.GetType("KirbyNightmareInDreamLand.Commands." + buttonmappingJsonData.Command)?.GetConstructor(Type.EmptyTypes);
            buttonmapping.CommandConstructorInfo ??= Type.GetType("KirbyNightmareInDreamLand.Commands." + buttonmappingJsonData.Command)?.GetConstructor(new Type[] { typeof(int) });

            // Add the new buttonmapping to its respective list.
            if (buttonmapping.CommandConstructorInfo != null)
            {
                buttonmap.Add(buttonmapping);
            }
            else
            {
                Debug.WriteLine(" [ERROR] LevelLoader.LoadButtonmapping: string \"" + buttonmappingJsonData.Command + "\" returns null from Type.GetType()");
            }
        }

        // Loads all buttonmaps from the .json registry.
        public void LoadAllButtonmaps()
        {
            // Open the buttonmap data file and deserialize it into a dictionary.
            Dictionary<string, List<ButtonmappingJsonData>> ButtonmapJsonDatas = JsonSerializer.Deserialize<Dictionary<string, List<ButtonmappingJsonData>>>(File.ReadAllText(Constants.Filepaths.ButtonmapRegistry), new JsonSerializerOptions());

            // Run through each buttonmap json data in the dictionary
            foreach (KeyValuePair<string, List<ButtonmappingJsonData>> ButtonmapJsonData in ButtonmapJsonDatas)
            {
                // Add new empty Buttonmap to the Buttonmaps dictionary.
                Buttonmaps.Add(ButtonmapJsonData.Key, new List<Buttonmapping>());
                // Run through each buttonmapping json data in the keymap json data
                foreach (ButtonmappingJsonData buttonmappingJsonData in ButtonmapJsonData.Value)
                {
                    // Load the buttonmapping. Passes in the individual keymapping json data and a reference to the actual keymap List<Keymapping> to add it to.
                    LoadButtonmapping(buttonmappingJsonData, Buttonmaps[ButtonmapJsonData.Key]);
                }
            }
            //Debug.WriteLine("buttonmap1 size: " + Buttonmaps["buttonmap1"]);
        }
        #endregion

        #region Hitboxes
        // Loads a hitbox given its and data.
        private void LoadHitbox(Dictionary<string, Rectangle> _poseDictionary, string _poseName, HitboxJsonData _hitboxJsonData)
        {
            Rectangle _hitbox = new Rectangle(
                _hitboxJsonData.XOffset,
                _hitboxJsonData.YOffset,
                _hitboxJsonData.Width,
                _hitboxJsonData.Height
            );
            _poseDictionary.Add(_poseName, _hitbox);
        }

        // Loads all hitboxes from the .json registry.
        public void LoadAllHitboxes()
        {
            // Open the hitbox data file and deserialize it into a dictionary of dictionaries.
            Dictionary<string, Dictionary<string, HitboxJsonData>> hitboxJsonDatas = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, HitboxJsonData>>>(File.ReadAllText(Constants.Filepaths.HitboxRegistry), new JsonSerializerOptions());

            // Run through the json dictionary of hitbox TYPE dictionaries.
            foreach (KeyValuePair<string, Dictionary<string, HitboxJsonData>> jsonTypeDictionary in hitboxJsonDatas)
            {
                // Create a hitbox POSE dictionary in Hitboxes for the current type.
                Dictionary<string, Rectangle> poseDictionary = new Dictionary<string, Rectangle>();
                Hitboxes.Add(jsonTypeDictionary.Key, poseDictionary);

                foreach (KeyValuePair<string, HitboxJsonData> data in jsonTypeDictionary.Value)
                {
                    LoadHitbox(poseDictionary, data.Key, data.Value);
                }
            }
        }

        // Get a hitbox from an object and a pose key
        public Rectangle GetHitbox(string objectKey, string poseKey)
        {
            // If the hitbox dictionary contains a hitbox set for this object
            if (Hitboxes.ContainsKey(objectKey))
            {
                
                Dictionary<string, Rectangle> poseDictionary = Hitboxes[objectKey];

                // If the hitbox set for this object contains a hitbox for the given pose, return it.
                if (poseDictionary.ContainsKey(poseKey)) 
                {
                    return poseDictionary[poseKey];
                }
                // If the hitbox set for this object does NOT contain a hitbox for the given pose, return its default hitbox.
                else if (poseDictionary.ContainsKey("default")) 
                {
                    return poseDictionary["default"];
                }
                // If the hitbox set for this object does not contain a default hitbox, something is wrong.
                else
                {
                    Debug.WriteLine("ERROR (Hitboxes.json): Hitbox set for object type \"" + objectKey + "\" does not specify a default hitbox.");
                    return new Rectangle();
                }
                
            }
            // If the hitbox dictionary does NOT contain a hitbox set for this object
            else
            {
                Debug.WriteLine("ERROR (Hitboxes.json): No hitbox set for object type \"" + objectKey + "\" is specified.");
                return new Rectangle();
            }
        }

        public Rectangle GetHitbox(string objectKey)
        {
            return GetHitbox(objectKey, "default");
        }
        #endregion

        #region Collision Responses
        public void SetCollisionResponses()
        {
            #region Player-Tile Collisons
            CollisionType key1 = CollisionType.Player;
            CollisionType key2 = CollisionType.Platform;
            Action<ICollidable, ICollidable, Rectangle> action1 = TileCollisionActions.BottomPlatformCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);

            key2 = CollisionType.Block;
            action1 = TileCollisionActions.BottomBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            action1 = TileCollisionActions.RightBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            action1 = TileCollisionActions.LeftBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            key2 = CollisionType.SlopeGentle1Left;
            action1 = TileCollisionActions.GentleLeftSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null); // Register opposite side for gentle1 bc kirby registers that side when near the top

            key2 = CollisionType.SlopeGentle2Left;
            action1 = TileCollisionActions.MediumLeftSlopeCollison;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = CollisionType.SlopeSteepLeft;
            action1 = TileCollisionActions.SteepLeftSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = CollisionType.SlopeGentle1Right;
            action1 = TileCollisionActions.GentleRightSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null); // Register opposite side for gentle1 bc kirby registers that side when near the top

            key2 = CollisionType.SlopeGentle2Right;
            action1 = TileCollisionActions.MediumRightSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            key2 = CollisionType.SlopeSteepRight;
            action1 = TileCollisionActions.SteepRightSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            #endregion

            #region Enemy-Tile Collisions
            key1 = CollisionType.Enemy;
            // Is this necessary? Enemies already fall every update, I don't think we need to check for air. Either way, fall should only be called once per update. -Mark
            key2 = CollisionType.Air;
            action1 = TileCollisionActions.BottomAirCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);

            key2 = CollisionType.Water;
            action1 = TileCollisionActions.WaterCollision;
            collisionResponse.RegisterCollision(key1, key2, action1, null);

            key2 = CollisionType.Block;
            action1 = TileCollisionActions.BottomBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            action1 = TileCollisionActions.RightBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            action1 = TileCollisionActions.LeftBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            key2 = CollisionType.SlopeGentle1Left;
            action1 = TileCollisionActions.GentleLeftSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null); // Register opposite side for gentle1 bc enemy registers that side when near the top

            key2 = CollisionType.SlopeGentle2Left;
            action1 = TileCollisionActions.MediumLeftSlopeCollison;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = CollisionType.SlopeSteepLeft;
            action1 = TileCollisionActions.SteepLeftSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);

            key2 = CollisionType.SlopeGentle1Right;
            action1 = TileCollisionActions.GentleRightSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null); // Register opposite side for gentle1 bc enemy registers that side when near the top

            key2 = CollisionType.SlopeGentle2Right;
            action1 = TileCollisionActions.MediumRightSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            key2 = CollisionType.SlopeSteepRight;
            action1 = TileCollisionActions.SteepRightSlopeCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            #endregion

            
            #region Projectile Collisions
            key1 = CollisionType.EnemyAttack;
            key2 = CollisionType.Block;
            action1 = TileCollisionActions.BottomBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            action1 = TileCollisionActions.RightBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            action1 = TileCollisionActions.LeftBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);

            key1 = CollisionType.KirbyStar;
            key2 = CollisionType.Block;
            action1 = TileCollisionActions.BottomBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Bottom, action1, null);
            action1 = TileCollisionActions.RightBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Right, action1, null);
            action1 = TileCollisionActions.LeftBlockCollision;
            collisionResponse.RegisterCollision(key1, key2, CollisionSide.Left, action1, null);
            #endregion


            #region Player-Enemy Collisions
            key1 = CollisionType.Enemy;
                key2 = CollisionType.Player;
                    action1 = DynamicCollisionActions.KirbyEnemyCollision;
                    collisionResponse.RegisterCollision(key1, key2, action1, null);

                key2 = CollisionType.PlayerAttack;
                    action1 = DynamicCollisionActions.EnemyKirbyAttackCollision;
                    collisionResponse.RegisterCollision(key1, key2, action1, null);

                key2 = CollisionType.KirbyStar;
                    action1 = DynamicCollisionActions.EnemyKirbyAttackCollision;
                    collisionResponse.RegisterCollision(key1, key2, action1, null);

            key1 = CollisionType.EnemyAttack;
                key2 = CollisionType.Player;
                    action1 = DynamicCollisionActions.KirbyEnemyAttackCollision;
                    collisionResponse.RegisterCollision(key1, key2, action1, null);

            key1 = CollisionType.Player;
                key2 = CollisionType.PowerUp;
                    action1 = DynamicCollisionActions.KirbyItemCollision;
                    collisionResponse.RegisterCollision(key1, key2, null, action1);

            
            #endregion

            //Debug.WriteLine("Dictionary after collisionMapping");
            //foreach (var collision in collisionResponse.collisionMapping)
            //{
            //    Debug.WriteLine(collision);
            //}
        }
        #endregion
    }
}
