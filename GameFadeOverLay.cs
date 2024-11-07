using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand
{
	public class GameFadeOverLay
	{
        SpriteBatch spriteBatch;
        Camera _camera;

		public GameFadeOverLay()
        {
            spriteBatch = Game1.Instance._spriteBatch;
            _camera = Game1.Instance.Camera;
        }

        public void Draw(float alphaFade)
        {
            System.Diagnostics.Debug.WriteLine("Current alpha fade value = " + alphaFade);
            GameDebug.Instance.DrawSolidRectangle(spriteBatch, _camera.bounds, Color.Black, alphaFade);

        }

    }

}

