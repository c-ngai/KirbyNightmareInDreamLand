using System;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace KirbyNightmareInDreamLand.GameState
{
	public class GamePausedState : IGameState
	{
        SpriteBatch spriteBatch;

		public GamePausedState()
		{
            spriteBatch = Game1.Instance._spriteBatch;
		}

        public void Draw()
        {
            List<string> kirbyType = new List<string>();
            foreach (Player player in Game1.Instance.manager.Players)
            {
                kirbyType.Add(player.GetKirbyTypePause());
            }
            Sprite pause_sprite = SpriteFactory.Instance.CreateSprite(kirbyType[0] + "_pause_screen");
            Sprite pause_background = SpriteFactory.Instance.CreateSprite("pause_screen_background");

            pause_background.Draw(Vector2.Zero, spriteBatch);
            pause_sprite.Draw(Vector2.Zero, spriteBatch);
        }

        public void Update()
        {
            // do nothing when paused 
        }

    }
}

