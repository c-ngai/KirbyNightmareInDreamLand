using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.Projectiles;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System;
using System.Linq;

namespace KirbyNightmareInDreamLand
{
    public class ObjectManager
    {
        public List<ICollidable> DynamicObjects { get; private set; }

        // Need to be able to access specific objects without removing them
        public List<ICollidable> StaticObjects { get; private set; }
        public List<ICollidable> DebugStaticObjects { get; private set; }

        // Single-player but can later be updated to an array of kirbys for multiplayer
        public List<IPlayer> Players { get; private set; }

        public IEnemy[] EnemyList { get; set; }

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

        // fields and methods for score
        public int Score { get; private set; }

        public void UpdateScore(int points)
        {
            Score += points;
        }


        public ObjectManager()
        {
            DynamicObjects = new List<ICollidable>();
            StaticObjects = new List<ICollidable>();
            DebugStaticObjects = new List<ICollidable>();
            InitializeTileTypes();
        }

        public void LoadKirby()
        {
            // Creates kirby object
            Players = new List<IPlayer>();
            IPlayer kirby = new Player(new Vector2(30, Constants.Graphics.FLOOR));
            Players.Add(kirby);
            // Target the camera on Kirby
            Camera camera = Game1.Instance.Camera;
            camera.TargetPlayer(Players[0]);
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

        public void ResetDebugStaticObjects()
        {
            DebugStaticObjects.Clear();
        }

        // Register dynamic objects like Player, Enemy, Projectiles, etc.
        public void RegisterDynamicObject(ICollidable dynamicObj)
        {
            DynamicObjects.Add(dynamicObj);
        }

        // Register static objects like Tiles and the PowerUp.
        public void RegisterStaticObject(ICollidable staticObj)
        {
            if (!StaticObjects.Contains(staticObj))
            {
                StaticObjects.Add(staticObj);
                DebugStaticObjects.Add(staticObj);
            }
        }

        public void RemoveDynamicObject(ICollidable dynamicObj)
        {
            DynamicObjects.Remove(dynamicObj);
        }

        public void RemoveStaticObject(ICollidable staticObj)
        {
            StaticObjects.Remove(staticObj);
        }
        public void OrganizeList()
        {
            DynamicObjects = DynamicObjects.OrderBy<ICollidable, String>(o => o.GetObjectType()).ToList();
        }

        public void UpdateDynamicObjects()
        {
            DynamicObjects.RemoveAll(obj => !obj.CollisionActive);
        }

        public void RemoveNonPlayers()
        {
            DynamicObjects.RemoveAll(obj => !obj.GetObjectType().Equals("Player"));
        }
        #endregion
    }
}