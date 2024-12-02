using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Commands;
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
using static KirbyNightmareInDreamLand.Constants;

namespace KirbyNightmareInDreamLand.Levels
{

    public class Level
    {

        private readonly Game1 _game;
        private readonly ObjectManager _manager;
        private Vector2 spawnPoint;
        public Vector2 SpawnPoint
        {
            get => spawnPoint;
            set => spawnPoint = value;
        }

        public float FadeAlpha;

        public Room CurrentRoom { get; private set; }
        public String PreviousRoom;
        public Vector2 PreviousSpawn;
        public bool NewRoom; // Is level in a new room this update? i.e. Is the currentRoom of this update different from the last one?

        private ObjectManager manager = ObjectManager.Instance;
        public List<PowerUp> powerUpList;
        public string EnemyNamespace = Constants.Namespaces.ENEMY_NAMESPACE;
        public string PowerUpNamespace = Constants.Namespaces.POWERUP_NAMESPACE;

        public string NextRoom;
        public Vector2? NextSpawn;


        // Fields for detecting if a door is being opened and which one (index in CurrentRoom.Doors[])
        public bool IsDoorBeingOpened;
        public int DoorBeingOpened;
        public bool IsDoorBeingExited;
        public int DoorBeingExited;


        public IGameState _currentState { get; set; }
        private string oldGameState;

        public readonly IGameState _playingState;
        private readonly IGameState _pausedState;
        public readonly IGameState _gameOverState;
        public readonly IGameState _winningState;

        public Level()
        {
            _game = Game1.Instance;
            _manager = Game1.Instance.manager;
            _currentState = new GamePlayingState(this);

            IsDoorBeingOpened = false;
            IsDoorBeingExited = false;
            DoorBeingOpened = 0;

            _playingState = new GamePlayingState(this);
            _pausedState = new GamePausedState(this);
            _gameOverState = new GameGameOverState(this);
            _winningState = new GameWinningState(this);
            oldGameState = _currentState.ToString();
        }

        public void ChangeState(IGameState newState)
        {
            _currentState = newState;
        }

        public bool IsCurrentState(string state)
        {
            return (_currentState).ToString().Equals(state);
        }

        public bool InMenuRoom()
        {
            return CurrentRoom.Name == Constants.RoomStrings.GAME_OVER_ROOM
                || CurrentRoom.Name == Constants.RoomStrings.LEVEL_COMPLETE_ROOM;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _currentState.Draw(spriteBatch);
        }

        private static Dictionary<string, string> gameStateControllermaps = new  Dictionary<string, string> 
        {
            {"KirbyNightmareInDreamLand.GameState.GamePlayingState", "main"},
            {"KirbyNightmareInDreamLand.GameState.GamePausedState", "paused"},
            {"KirbyNightmareInDreamLand.GameState.GameGameOverState", "gameover"},
            {"KirbyNightmareInDreamLand.GameState.GameWinningState", "winning"},
            {"KirbyNightmareInDreamLand.GameState.GameTransitioningState", "no_control"},
            {"KirbyNightmareInDreamLand.GameState.GamePowerChangeState", "no_control"}
        };

        private void UpdateControllerMaps()
        {
            string currentGameState = _currentState.ToString();
            if(currentGameState != oldGameState)
            {
                if (gameStateControllermaps.ContainsKey(currentGameState))
                {
                    LevelLoader.Instance.LoadKeymap(gameStateControllermaps[currentGameState]);
                    LevelLoader.Instance.LoadButtonmap(gameStateControllermaps[currentGameState]);
                }
                else
                {
                    Debug.WriteLine(" [ERROR] Level.UpdateControllerMaps(): gameStateControllermaps does not contain key \"" + currentGameState + "\"");
                }
            }
            oldGameState = _currentState.ToString();
        }

        public void UpdateLevel()
        {
            UpdateControllerMaps();
            _currentState.Update();
            NewRoom = false;
        }

        public void PauseLevel()
        {
            ChangeState(_pausedState);
        }

        public void UnpauseLevel()
        {
            ChangeState(_playingState);
        }

        public void SelectQuit()
        {
            _currentState.SelectQuitButton();
        }

        public void SelectContinue()
        {
            _currentState.SelectContinueButton();
        }

        public void SelectButton()
        {
            _currentState.SelectButton();
        }

        public void ChangeToTransitionState()
        {
            _currentState = new GameTransitioningState(this);
        }
        public void ChangeToPowerChangeState()
        {
            _currentState = new GamePowerChangeState(this);
        }

        public void GameOver()
        {
            NextRoom = "game_over";
            NextSpawn = null; // gameOverSpawnPoint;
            PreviousRoom = CurrentRoom.Name;
            PreviousSpawn = CurrentRoom.SpawnPoint;
            Debug.WriteLine("next room to load is - " + NextRoom);
            _currentState = new GameTransitioningState(this);
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
            for (int i = 0; i < CurrentRoom.Doors.Count; i++)
            {
                if (CurrentRoom.Doors[i].Bounds.Contains(playerPos))
                {
                    NextRoom = CurrentRoom.Doors[i].DestinationRoom;
                    NextSpawn = CurrentRoom.Doors[i].DestinationPoint;
                    PreviousRoom = CurrentRoom.Name;
                    PreviousSpawn = CurrentRoom.SpawnPoint;

                    IsDoorBeingOpened = true;
                    DoorBeingOpened = i;
                    _currentState = new GameTransitioningState(this);
                    break;
                }
            }
        }

        public void ExitDoorAt(Vector2? _doorPosition)
        {
            Vector2 doorPosition = _doorPosition ?? CurrentRoom.SpawnPoint;
            for (int i = 0; i < CurrentRoom.Doors.Count; i++)
            {
                if (CurrentRoom.Doors[i].Bounds.Contains(doorPosition))
                {
                    DoorBeingExited = i;
                }
            }
        }

        // Loads a room into the level by name, specifying a spawn point. (for entering from a door)
        public void LoadRoom(string RoomName, Vector2? _spawnPoint)
        {
            if (LevelLoader.Instance.Rooms.ContainsKey(RoomName))
            {
                // Sets it up so players are the only thing remaining in the object lists when rooms change
                //_manager.RemoveNonPlayers();
                _manager.ResetStaticObjects();
                CurrentRoom = LevelLoader.Instance.Rooms[RoomName];
                // Debug.WriteLine("current room is " + CurrentRoom);
                LoadLevelObjects();
                spawnPoint = _spawnPoint ?? CurrentRoom.SpawnPoint;
                spawnPoint.Y += Constants.Collision.GROUND_COLLISION_OFFSET;
                foreach (IPlayer player in _manager.Players)
                {
                    player?.GoToRoomSpawn();
                    //_manager.RegisterDynamicObject((Player)player);
                }
                // Disable splitscreen if room camera is locked in both directions
                _game.SPLITSCREEN_AVAILABLE = !(CurrentRoom.CameraXLock && CurrentRoom.CameraYLock);
                GraphicsToggleSplitscreenCommand.UpdateSplitscreenScaling();
                SoundManager.PlaySong(CurrentRoom.Song);
                NewRoom = true;
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
            // Clear all existing enemies from the previous room before loading new ones
            _manager.ClearObjects();

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
                        //_manager.Enemies.Add(enemyObject);
                    }
                }
            }

            //System.Diagnostics.Debug.WriteLine("enemy list contains" + enemyList.Count + "enemies");

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