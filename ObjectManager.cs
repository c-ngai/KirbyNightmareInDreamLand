using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Sprites;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;

namespace KirbyNightmareInDreamLand
{
    public class ObjectManager
    {
        public List<ICollidable> DynamicObjects { get; private set; }

        // Need to be able to access specific objects without removing them
        public List<ICollidable> StaticObjects { get; private set; }

        // Single-player but can later be updated to an array of kirbys for multiplayer
        public List<IPlayer> Players { get; set; }

        public List<Tile> relevantTilesLastUpdate { get; private set; }

        public IEnemy[] EnemyList { get; set; }

        // Get enemies (currently one of each but can change to an array of each enemy type)
        private IEnemy waddledeeTest;
        private IEnemy waddledooTest;
        private IEnemy brontoburtTest;
        private IEnemy hotheadTest;
        private IEnemy poppybrosjrTest;
        private IEnemy sparkyTest;

        public int CurrentEnemyIndex { get; set; }

        public Sprite Item { get; set; }

        public string[] tileTypes { get; private set; } = new string[Constants.Level.NUMBER_OF_TILE_TYPES];

        private static ObjectManager instance = new ObjectManager();
        public static ObjectManager Instance
        {
            get
            {
                return instance;
            }
        }
        public ObjectManager()
        {
            DynamicObjects = new List<ICollidable>();
            StaticObjects = new List<ICollidable>();
            InitializeTileTypes();
        }

        public void LoadItem()
        {
            Item = SpriteFactory.Instance.CreateSprite("item_maximtomato");
        }

        public void LoadObjects()
        {
            ObjectManager.Instance.ResetDynamicCollisionBoxes();
            // Creates kirby object
            //make it a list from the get go to make it multiplayer asap
            Players = new List<IPlayer>();
            IPlayer kirby = new Player(new Vector2(30, Constants.Graphics.FLOOR));
            kirby.PlayerSprite = SpriteFactory.Instance.CreateSprite("kirby_normal_standing_right");
            Players.Add(kirby);

            Debug.WriteLine("Created Kirby ");
            foreach (var dynamicOb in DynamicObjects)
            {
                Debug.WriteLine($"dynamic object: {dynamicOb}\n");
            }
            // Target the camera on Kirby
            Camera camera = Game1.Instance.Camera;
            camera.TargetPlayer(Players[0]);


            // Currently commented out since we don't need the item
            // LoadItem();

            // Creates enemies
            waddledeeTest = new WaddleDee(new Vector2(80, Constants.Graphics.FLOOR));
            waddledooTest = new WaddleDoo(new Vector2(80, Constants.Graphics.FLOOR));
            brontoburtTest = new BrontoBurt(new Vector2(80, Constants.Graphics.FLOOR));
            hotheadTest = new Hothead(new Vector2(80, Constants.Graphics.FLOOR));
            poppybrosjrTest = new PoppyBrosJr(new Vector2(80, Constants.Graphics.FLOOR));
            sparkyTest = new Sparky(new Vector2(80, Constants.Graphics.FLOOR));

            EnemyList = new IEnemy[] { waddledeeTest, waddledooTest, brontoburtTest, hotheadTest, poppybrosjrTest, sparkyTest };
            CurrentEnemyIndex = 0;
        }


        #region Collision
        public void InitializeTileTypes()
        {
            tileTypes[(int)TileCollisionType.Air] = "Air";
            tileTypes[(int)TileCollisionType.Block] = "Block";
            tileTypes[(int)TileCollisionType.Platform] = "Platform";
            tileTypes[(int)TileCollisionType.Water] = "Water";
            tileTypes[(int)TileCollisionType.SlopeSteepLeft] = "SlopeSteepLeft";
            tileTypes[(int)TileCollisionType.SlopeGentle1Left] = "SlopeGentle1Left";
            tileTypes[(int)TileCollisionType.SlopeGentle2Left] = "SlopeGentle2Left";
            tileTypes[(int)TileCollisionType.SlopeGentle2Right] = "SlopeGentle2Right";
            tileTypes[(int)TileCollisionType.SlopeGentle1Right] = "SlopeGentle1Right";
            tileTypes[(int)TileCollisionType.SlopeSteepRight] = "SlopeSteepRight";
        }
        public void ResetDynamicCollisionBoxes()
        {
            DynamicObjects.Clear();
        }

        // Note this removes all static objects which will be an issue if there are other non-tile ones
        public void ResetStaticObjects()
        {
            StaticObjects.Clear();
        }

        // Register dynamic objects like Player, Enemy, Projectiles, etc.
        public void RegisterDynamicObject(ICollidable dynamicObj)
        {
            DynamicObjects.Add(dynamicObj);
        }

        // Register static objects like Tiles and the PowerUp.
        public void RegisterStaticObject(ICollidable staticObj)
        {
            if (!StaticObjects.Contains(staticObj)) StaticObjects.Add(staticObj);
        }

        public void RemoveDynamicObject(ICollidable dynamicObj)
        {
            DynamicObjects.Remove(dynamicObj);
        }

        public void RemoveStaticObject(ICollidable staticObj)
        {
            StaticObjects.Remove(staticObj);
        }

        public void UpdateObjectLists()
        {
            ResetStaticObjects();
            DynamicObjects.RemoveAll(obj => !obj.CollisionActive);
        }
        #endregion
    }
}