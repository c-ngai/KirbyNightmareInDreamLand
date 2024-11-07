using System;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static KirbyNightmareInDreamLand.Levels.Level;

namespace KirbyNightmareInDreamLand.GameState
{
	public class GameGameOverState : IGameState
	{
        SpriteBatch spriteBatch;
        public Sprite currentSprite;
        public Sprite selectQuitScreen;
        public Sprite selectContinueScreen;
        public Sprite gameOverAnimation;

        public GameGameOverState()
		{
            spriteBatch = Game1.Instance._spriteBatch;
            LevelLoader.Instance.LoadKeymap(""); // TODO: LINK KEY MAPS TO GAME STATES
            selectContinueScreen = SpriteFactory.Instance.CreateSprite("Game_over_continue_button");
            selectQuitScreen = SpriteFactory.Instance.CreateSprite("Game_over_quit_button");
            gameOverAnimation = SpriteFactory.Instance.CreateSprite("Game_over_foreground");
            currentSprite = gameOverAnimation;
        }

        public void Draw()
        {
            gameOverAnimation.Draw(Vector2.Zero, spriteBatch);
        }

        public void Update()
        {
            currentSprite.Update();
        }


        public void SelectQuit()
        {
            if (currentSprite == selectContinueScreen)
            {
                currentSprite = selectQuitScreen;
            }
        }

        public void SelectContinue()
        {
            if (currentSprite == selectQuitScreen)
            {
                currentSprite = selectContinueScreen;
            }
        }
    }
}

