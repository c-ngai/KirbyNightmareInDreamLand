using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.PowerUps;
using KirbyNightmareInDreamLand.GameState;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.StateMachines;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace KirbyNightmareInDreamLand.Levels
{

    public class Level
    {

        private readonly Game1 _game;
        private readonly Camera _camera;
        private readonly ObjectManager _manager; 
        public Vector2 SpawnPoint { get; private set; }


        public Room CurrentRoom { get; private set; }

        private ObjectManager manager = ObjectManager.Instance;
        public List<Enemy> enemyList;
        public List<PowerUp> powerUpList;
        public string EnemyNamespace = Constants.Namespaces.ENEMY_NAMESPACE;
        public string PowerUpNamespace = Constants.Namespaces.POWERUP_NAMESPACE;

        public string NextRoom = "room1";
        public Vector2 NextSpawn;


        public IGameState _currentState { get; private set; }

        public readonly IGameState _playingState;
        private readonly IGameState _pausedState;
        private readonly IGameState _gameOverState;
        private readonly IGameState _debugState;
        private readonly IGameState _transitionState;

        public Level()
        {
            _game = Game1.Instance;
            _camera = _game.Camera;
            _manager = Game1.Instance.manager;
            _currentState = new GamePlayingState();

            NextRoom = "room1";
            NextSpawn = new Vector2(0,0);

            _playingState = new GamePlayingState();
            _pausedState = new GamePausedState();
            _gameOverState = new GameGameOverState();
            _debugState = new GameDebugState();
            _transitionState = new GameTransitioningState();
        }

        public void ChangeState(IGameState newState)
        {
            _currentState = newState;
        }

        public bool IsCurrentState(IGameState state)
        {
            return _currentState.GetType().Equals(state.GetType());
        }

        public void Draw()
        {
            _currentState.Draw();
        }

        public void UpdateLevel()
        {
            _currentState.Update();
        }

        public void PauseLevel()
        {
            ChangeState(_pausedState);
        }

        public void UnpauseLevel()
        {
            ChangeState(_playingState);
        }

        public void GameOver()
        {
            ChangeState(_gameOverState);
        }

        // tells player if they are at a door or not 
        public bool atDoor(Vector2 playerPosition)
        {
            bool result = false;
            foreach (Door door in CurrentRoom.Doors)
            {
                if (door.Bounds.Contains(playerPosition))
                {
                    result = true;
                }
            }

            return result;
        }

        // go to the next room, called because a player wants to go through a door 
        public void EnterDoorAt(Vector2 playerPos)
        {
            foreach (Door door in CurrentRoom.Doors)
            {
                if (door.Bounds.Contains(playerPos))
                {
                    NextRoom = door.DestinationRoom;
                    NextSpawn = door.DestinationPoint;
                    ChangeState(_transitionState);
                }
            }
        }



        // Loads a room into the level by name, specifying a spawn point. (for entering from a door)
        public void LoadRoom(string RoomName, Vector2? _spawnPoint)
        {
            if (LevelLoader.Instance.Rooms.ContainsKey(RoomName))
            {
                // Sets it up so players are the only thing remaining in the object lists when rooms change
                _manager.RemoveNonPlayers();
                _manager.ResetStaticObjects();
                CurrentRoom = LevelLoader.Instance.Rooms[RoomName];
                LoadLevelObjects();
                SpawnPoint = _spawnPoint ?? CurrentRoom.SpawnPoint;
                foreach (IPlayer player in _manager.Players)
                {
                    player?.GoToRoomSpawn();
                    _manager.RegisterDynamicObject((Player)player);
                }
            }
            else
            {
                Debug.WriteLine(" [ERROR] \"" + RoomName + "\" is not a valid room name and cannot be loaded.");
            }
        }


        // Overflow method. If no spawn point is specified, the level will load the room's default. TODO: refactor this?? does this implementation suck?????
        public void LoadRoom(string RoomName)
        {
            LoadRoom(RoomName, null);
        }


        //level 
        //instantiate on demand
        // this needs to move to level loader or object manager 
        public void LoadLevelObjects()
        {
            enemyList = new List<Enemy>();
            foreach (EnemyData enemy in CurrentRoom.Enemies)
            {
                Type type = Type.GetType(EnemyNamespace + enemy.EnemyType);

                if (type != null)
                {
                    //System.Diagnostics.Debug.WriteLine("This is the type name for the enemy: " + type);

                    // Get the constructor that takes a Vector2 parameter
                    ConstructorInfo constructor = type.GetConstructor(new[] { typeof(Vector2) });
                    //System.Diagnostics.Debug.WriteLine("this is the enemy constructor" + constructor);

                    if (constructor != null)
                    {
                        // Create an instance of the enemy
                        Enemy enemyObject = (Enemy)constructor.Invoke(new object[] { enemy.SpawnPoint });
                        enemyList.Add(enemyObject);
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine("enemy list contains" + enemyList.Count + "enemies");

            // power ups currently do not require dynamic typing because they all use the same class. Will possibly need to change this later on. 
            powerUpList = new List<PowerUp>();
            foreach (PowerUpData powerUp in CurrentRoom.PowerUps)
            {
                Type type = Type.GetType(PowerUpNamespace);
                PowerUp new_item = new PowerUp(powerUp.SpawnPoint, powerUp.PowerUpType);
                powerUpList.Add(new_item);
            }
        }

    }
}