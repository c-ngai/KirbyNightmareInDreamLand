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
            //System.Diagnostics.Debug.WriteLine("Current alpha fade value = " + alphaFade);

            //GameDebug.Instance.DrawSolidRectangle(spriteBatch, camera.bounds, Color.White, alphaFade);

            // This version uses color addition instead of the typical multiplication to achieve a fade effect more similar to the GBA. Most probably won't notice the difference lol -Mark
            int a = (int)(alphaFade * 255);
            GameDebug.Instance.DrawSolidRectangle(spriteBatch, camera.bounds, new Color(a, a, a, 0), 1f);
        }



        // Do we still need this? -Mark

        //public void DrawPause(SpriteBatch spriteBatch)
        //{
        //    Camera camera = _game.cameras[_game.CurrentCamera];
        //    GameDebug.Instance.DrawSolidRectangle(spriteBatch, camera.bounds, Color.White, 1);
        //    List<string> kirbyType = new List<string>();
        //    foreach (Player player in Game1.Instance.manager.Players)
        //    {
        //        kirbyType.Add(player.GetKirbyTypePause());
        //    }
        //    ISprite pause_sprite = SpriteFactory.Instance.CreateSprite(kirbyType[0] + "_pause_screen");
        //    ISprite pause_background = SpriteFactory.Instance.CreateSprite("pause_screen_background");
        //    pause_background.Draw(new Vector2(camera.bounds.X, camera.bounds.Y), spriteBatch);
        //    pause_sprite.Draw(new Vector2(camera.bounds.X, camera.bounds.Y), spriteBatch);
        //}

    }

}

