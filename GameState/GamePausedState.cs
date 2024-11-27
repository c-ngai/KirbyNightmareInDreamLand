using System;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using KirbyNightmareInDreamLand.Levels;

namespace KirbyNightmareInDreamLand.GameState
{
	public class GamePausedState : BaseGameState
	{

        private Sprite backgroundSprite = SpriteFactory.Instance.CreateSprite("pause_screen_background");

        // Holds a sprite for kirby and each enemy type to draw at their spawn points in level debug mode.
        private Dictionary<string, Sprite> pauseSprites = new Dictionary<string, Sprite>()
        {
            { "Normal" , SpriteFactory.Instance.CreateSprite("Normal_pause_screen") },
            { "Beam" , SpriteFactory.Instance.CreateSprite("Beam_pause_screen") },
            { "Spark" , SpriteFactory.Instance.CreateSprite("Spark_pause_screen") },
            { "Fire" , SpriteFactory.Instance.CreateSprite("Fire_pause_screen") },
            { "Professor" , SpriteFactory.Instance.CreateSprite("Professor_pause_screen") },
            { "Dead" , SpriteFactory.Instance.CreateSprite("Dead_pause_screen") }
        };

        public GamePausedState(Level _level) : base(_level)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Camera _camera = _game.cameras[_game.CurrentCamera];

            //GameDebug.Instance.DrawSolidRectangle(spriteBatch, _camera.bounds, Color.White, 1);

            string kirbyType = _manager.Players[_game.CurrentCamera].GetKirbyTypePause();

            backgroundSprite.Draw(new Vector2(_camera.bounds.X, _camera.bounds.Y), spriteBatch);
            pauseSprites[kirbyType].Draw(new Vector2(_camera.bounds.X, _camera.bounds.Y), spriteBatch);
        }

        public override void Update()
        {
            // do nothing
        }

    }
}

