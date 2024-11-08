using System;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace KirbyNightmareInDreamLand.GameState
{
	public class GameGameOverState : BaseGameState
	{
        private ObjectManager _manager;
        SpriteBatch spriteBatch;
        public Sprite currentButtonSprite;
        public Sprite selectQuitScreen;
        public Sprite selectContinueScreen;

        public GameGameOverState(Level _level) : base(_level)
        {
            _manager = Game1.Instance.manager;
            spriteBatch = Game1.Instance._spriteBatch;
            selectContinueScreen = SpriteFactory.Instance.CreateSprite("button_continue");
            selectQuitScreen = SpriteFactory.Instance.CreateSprite("button_quit");
            currentButtonSprite = selectContinueScreen;
        }

        public override void Draw()
        {
            DrawBackground(spriteBatch);
            DrawForeground(spriteBatch);
            currentButtonSprite.Draw(new Vector2(136, 71), spriteBatch);
            foreach (IPlayer player in _manager.Players) player.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();
        }


        public void SelectQuit()
        {
            if (currentButtonSprite == selectContinueScreen)
            {
                currentButtonSprite = selectQuitScreen;
            }
        }

        public void SelectContinue()
        {
            if (currentButtonSprite == selectQuitScreen)
            {
                currentButtonSprite = selectContinueScreen;
            }
        }
    }
}

