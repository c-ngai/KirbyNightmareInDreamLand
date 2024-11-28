using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.PowerUps;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace KirbyNightmareInDreamLand.GameState
{
	public class GamePlayingState : BaseGameState
	{

        public GamePlayingState(Level _level) : base(_level)
        {
        }

        public override void Update()
        {
            base.Update();
            // If all players are dead and inactive...
            if (_manager.AllPlayersInactive())
            {
                // ...and if they were all out of extra lives, then game over.
                if (_manager.AllPlayersOutOfLives())
                {
                    level.GameOver();
                }
                // If at least one of them had at least one extra life, restart the room
                else
                {
                    level.NextRoom = level.CurrentRoom.Name;
                    level.NextSpawn = level.SpawnPoint;
                    level.ChangeToTransitionState();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            
        }
    }
}

