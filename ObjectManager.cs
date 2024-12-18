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

        private List<int> projectileOrder;

        public Sprite Item { get; set; }

        //public string[] tileTypes { get; private set; } = new string[Constants.Level.NUMBER_OF_TILE_TYPES];

        // fields and methods for score
        public int Score { get; private set; }


        private Random random;


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
            
            random = new Random();
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
            DynamicObjects.RemoveAll(obj => obj is IProjectile projectile && projectile.IsDone());
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

        public IPlayer NearestPlayer(Vector2 position)
        {
            IPlayer nearestPlayer = null;
            float minDistance = float.MaxValue;
            foreach (IPlayer player in Players)
            {
                float distance = Vector2.Distance(position, player.GetKirbyPosition());
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestPlayer = player;
                }
            }
            return nearestPlayer;
        }

        public bool AllPlayersInactive()
        {
            foreach (IPlayer player in Players)
            {
                if (player.IsActive)
                {
                    return false;
                }
            }
            return true;
        }

        public bool AllPlayersDead()
        {
            foreach (IPlayer player in Players)
            {
                if (!player.DEAD)
                {
                    return false;
                }
            }
            return true;
        }

        public bool AllPlayersOutOfLives()
        {
            foreach (IPlayer player in Players)
            {
                if (player.lives > 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void FillAllPlayerLives()
        {
            foreach (IPlayer player in Players)
            {
                player.FillLives();
            }
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
            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Update();
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
            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Update();
            }
            //UpdateProjectiles();
            foreach (IParticle particle in Particles)
            {
                particle.Update();
            }
            //UpdateParticles();
            UpdateDynamicObjects();
            EmptyLists();
            projectileOrder = Enumerable.Range(0, Projectiles.Count).OrderBy(x => random.Next()).ToList();
        }

        public void DrawPlayers(SpriteBatch spriteBatch)
        {
            foreach (IPlayer player in Players)
            {
                player.Draw(spriteBatch);
            }
        }

        public void DrawEnemies(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        public void DrawPowerups(SpriteBatch spriteBatch)
        {
            foreach (PowerUp powerUp in Game1.Instance.Level.powerUpList)
            {
                powerUp.Draw(spriteBatch);
            }
        }

        public void DrawProjectiles(SpriteBatch spriteBatch)
        {
            // Draw projectiles in random order, makes effects like fire look a lot better
            foreach (int i in projectileOrder)
            {
                Projectiles[i].Draw(spriteBatch);
            }
        }

        public void DrawParticles(SpriteBatch spriteBatch)
        {
            foreach (IParticle particle in Particles)
            {
                particle.Draw(spriteBatch);
            }
        }

        public void DrawAllObjects(SpriteBatch spriteBatch)
        {
            DrawPlayers(spriteBatch);
            DrawEnemies(spriteBatch);
            DrawPowerups(spriteBatch);
            DrawProjectiles(spriteBatch);
            DrawParticles(spriteBatch);
        }

        // Draw ONLY the players currently changing power and their projectiles
        public void DrawPowerChangeObjects(SpriteBatch spriteBatch)
        {
            foreach (IPlayer player in Players)
            {
                if (player.powerChangeAnimation)
                {
                    player.Draw(spriteBatch);
                }
            }
            // temporary projectile order just for the power change projectiles. yes, this generates more than it needs to. for what it's worth to me though, the overhead is negligible.
            List<int> projectileOrder2 = Enumerable.Range(0, Projectiles.Count).OrderBy(x => random.Next()).ToList();
            foreach (int i in projectileOrder2)
            {
                if (Projectiles[i].player != null && Projectiles[i].player.powerChangeAnimation)
                {
                    Projectiles[i].Draw(spriteBatch);
                }
            }
        }

    }
}