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
using static KirbyNightmareInDreamLand.Levels.Level;

namespace KirbyNightmareInDreamLand.GameState
{
	public class GamePlayingState : BaseGameState
	{
        SpriteBatch spriteBatch;

        public GamePlayingState()
		{
            spriteBatch = Game1.Instance._spriteBatch;
        }

        public override void Draw()
        {
            DrawBackground(spriteBatch);
            DrawForeground(spriteBatch);
            DrawDoorStars(spriteBatch);
            DrawLevelObjects(spriteBatch);
        }
    }
}

