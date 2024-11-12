using System;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static KirbyNightmareInDreamLand.Levels.Level;

namespace KirbyNightmareInDreamLand.GameState
{
	public class GameLifeLostState : BaseGameState
	{
        private readonly ObjectManager _manager;
        public GameLifeLostState(Level _level) : base( _level)
		{
            _manager = Game1.Instance.manager;
        }
    }
}

