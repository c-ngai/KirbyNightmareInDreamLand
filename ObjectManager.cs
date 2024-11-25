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
using KirbyNightmareInDreamLand.Particles;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Entities.PowerUps;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand
{
    public sealed class ObjectManager
    {
        public List<ICollidable> DynamicObjects { get; private set; }

        // Need to be able to access specific objects without removing them
        public List<ICollidable> StaticObjects { get; private set; }
        public List<ICollidable> DebugStaticObjects { get; private set; }

        public List<IPlayer> Players { get; private set; }
        public List<IEnemy> Enemies { get; set; }
        public List<IProjectile> Projectiles { get; set; }
        public List<IParticle> Particles { get; private set; }

        public Sprite Item { get; set; }

        //public string[] tileTypes { get; private set; } = new string[Constants.Level.NUMBER_OF_TILE_TYPES];

        // fields and methods for score
        public int Score { get; private set; }


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
            DebugStaticObjects = new List<ICollidable>();

            Players = new List<IPlayer>();
            Enemies = new List<IEnemy>();
            Projectiles = new List<IProjectile>();
            Particles = new List<IParticle>();
            
            //InitializeTileTypes();
        }

        public void ClearObjects()
        {
            foreach (IEnemy enemy in Enemies)
            {
                enemy.Dispose();
            }
            Enemies.Clear();
            Projectiles.Clear();
            Particles.Clear();

            DynamicObjects.Clear();
        }

        public void UpdateScore(int points)
        {
            Score += points;
        }

        public void AddKirby(IPlayer Kirby)
        {
            Players.Add((Player)Kirby);
        }
        #region keyboard
        public void ChangeKeyboard()
        {
            
        }
        #endregion

        public void AddEnemy(IEnemy enemy)
        {
            Enemies.Add(enemy);
        }
        public void AddProjectile(IProjectile projectile)
        {
            Projectiles.Add(projectile);
        }

        public void AddParticle(IParticle particle)
        {
            Particles.Add(particle);
        }

        #region Collision
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
            DynamicObjects = DynamicObjects.OrderBy<ICollidable, CollisionType>(o => o.GetCollisionType()).ToList();
        }

        private void UpdateDynamicObjects()
        {
            foreach (ICollidable player in Players)
            {
                if (player.CollisionActive && !DynamicObjects.Contains(player))
                {
                    RegisterDynamicObject(player);
                }
            }
            foreach (ICollidable enemy in Enemies)
            {
                if (enemy.CollisionActive && !DynamicObjects.Contains(enemy))
                {
                    RegisterDynamicObject(enemy);
                }
            }
            foreach (ICollidable projectile in Projectiles)
            {
                if (projectile.CollisionActive && !DynamicObjects.Contains(projectile))
                {
                    RegisterDynamicObject(projectile);
                }
            }
            DynamicObjects.RemoveAll(obj => !obj.CollisionActive);
        }

        private void EmptyLists()
        {
            Projectiles.RemoveAll(obj => obj.IsDone());
            Particles.RemoveAll(obj => obj.IsDone());
        }


        public void RemoveNonPlayers()
        {
            DynamicObjects.RemoveAll(obj => !obj.GetCollisionType().Equals("Player"));
        }

        public void ClearPlayerList()
        {
            Players.Clear();
        }
        #endregion

        public bool NearestPlayerDirection(Vector2 position)
        {
            IPlayer nearestPlayer;
            float minXDistance = float.MaxValue;
            float closestPlayerX = 0;
            foreach (IPlayer player in Players)
            {
                float playerX = player.GetKirbyPosition().X;
                float xDistance = Math.Abs(playerX - position.X);
                if (xDistance < minXDistance)
                {
                    minXDistance = xDistance;
                    closestPlayerX = playerX;
                    nearestPlayer = player;
                }
            }
            bool isLeft = closestPlayerX < position.X;
            return isLeft;
        }

        public void UpdatePlayers()
        {
            foreach (IPlayer player in Players)
            {
                player.Update(Game1.Instance.time);
            }
        }

        public void UpdateProjectiles()
        {
            foreach (IProjectile projectile in Projectiles)
            {
                projectile.Update();
            }
        }

        public void Update()
        {
            foreach (IPlayer player in Players)
            {
                player.Update(Game1.Instance.time);
            }
            foreach (Enemy enemy in Enemies)
            {
                enemy.Update(Game1.Instance.time);
            }
            //UpdateEnemies();
            foreach (PowerUp powerUp in Game1.Instance.Level.powerUpList)
            {
                powerUp.Update();
            }
            foreach (IProjectile projectile in Projectiles)
            {
                projectile.Update();
            }
            //UpdateProjectiles();
            foreach (IParticle particle in Particles)
            {
                particle.Update();
            }
            //UpdateParticles();
            UpdateDynamicObjects();
            EmptyLists();
        }

        public void DrawPlayers(SpriteBatch spriteBatch)
        {
            foreach (IPlayer player in Players)
            {
                player.Draw(spriteBatch);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (IPlayer player in Players)
            {
                player.Draw(spriteBatch);
            }
            foreach (Enemy enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
            foreach (PowerUp powerUp in Game1.Instance.Level.powerUpList)
            {
                powerUp.Draw(spriteBatch);
            }
            foreach (IProjectile projectile in Projectiles)
            {
                projectile.Draw(spriteBatch);
            }
            foreach (IParticle particle in Particles)
            {
                particle.Draw(spriteBatch);
            }
        }
    }
}