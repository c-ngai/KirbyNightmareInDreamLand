using System;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Sprites;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand
{
	public class GameOverLay
	{
        Game1 _game;

		public GameOverLay()
        {
            _game = Game1.Instance;
        }

        public void DrawFade(SpriteBatch spriteBatch, float alphaFade)
        {
            Camera camera = _game.cameras[_game.CurrentCamera];

            // this version uses color addition instead of the typical multiplication to achieve a fade effect more similar to the GBA
            int a = (int)(alphaFade * Constants.Graphics.FADE_COLOR_ADDITION);
            GameDebug.Instance.DrawSolidRectangle(spriteBatch, camera.bounds, new Color(a, a, a, 0), 1f);
        }
    }

}

