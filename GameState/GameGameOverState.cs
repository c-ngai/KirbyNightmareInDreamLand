using System;
using System.Diagnostics;
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
        private SpriteBatch spriteBatch;
        private Sprite currentButtonSprite;
        private Sprite selectQuitScreen;
        private Sprite selectContinueScreen;
        private Vector2 kirbyStartRoomSpawn = Constants.Level.ROOM1_SPAWN_POINT;
        private string room1String = Constants.RoomStrings.ROOM_1;
        private Vector2 buttonPosition = Constants.ButtonLocations.GAMEOVER_BUTTONS;

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
            currentButtonSprite.Draw(buttonPosition, spriteBatch);
            foreach (IPlayer player in _manager.Players) player.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void SelectQuitButton()
        {
            currentButtonSprite = selectQuitScreen;            
        }

        public override void SelectContinueButton()
        {
            currentButtonSprite = selectContinueScreen;
        }

        public override void SelectButton()
        {
            if (currentButtonSprite == selectQuitScreen)
            {
                Game1.Instance.Exit();
            }
            else
            {
                level.NextRoom = room1String;
                level.NextSpawn = kirbyStartRoomSpawn;
                level.LoadRoom(level.NextRoom, level.NextSpawn); // load new room
                level.ChangeToPlaying();
            }
        }

    }
}

