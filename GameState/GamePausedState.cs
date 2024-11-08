using System;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Audio;

namespace KirbyNightmareInDreamLand.GameState
{
	public class GamePausedState : IGameState
	{
        SpriteBatch spriteBatch;
        Camera _camera;

		public GamePausedState()
		{
            spriteBatch = Game1.Instance._spriteBatch;
            _camera = Game1.Instance.Camera;
        }

        public void Draw()
        {
            GameDebug.Instance.DrawSolidRectangle(spriteBatch, _camera.bounds, Color.White, 1);
            List<string> kirbyType = new List<string>();
            foreach (Player player in Game1.Instance.manager.Players)
            {
                kirbyType.Add(player.GetKirbyTypePause());
            }
            ISprite pause_sprite = SpriteFactory.Instance.CreateSprite(kirbyType[0] + "_pause_screen");
            ISprite pause_background = SpriteFactory.Instance.CreateSprite("pause_screen_background");
            pause_background.Draw(new Vector2(_camera.bounds.X, _camera.bounds.Y), spriteBatch);
            pause_sprite.Draw(new Vector2(_camera.bounds.X, _camera.bounds.Y), spriteBatch);
        }

        public void Update()
        {
            // do nothing when paused
        }

    }
}

