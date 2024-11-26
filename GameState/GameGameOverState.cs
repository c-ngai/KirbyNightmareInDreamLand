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
        private ISprite currentButtonSprite;
        private ISprite selectQuitScreen;
        private ISprite selectContinueScreen;
        private Vector2 kirbyStartRoomSpawn = Constants.Level.ROOM1_SPAWN_POINT;
        private string room1String = Constants.RoomStrings.ROOM_1;
        private Vector2 buttonPosition = Constants.ButtonLocations.GAMEOVER_BUTTONS;

        public GameGameOverState(Level _level) : base(_level)
        {
            selectContinueScreen = SpriteFactory.Instance.CreateSprite("button_continue");
            selectQuitScreen = SpriteFactory.Instance.CreateSprite("button_quit");
            currentButtonSprite = selectContinueScreen;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Camera camera = _game.cameras[_game.CurrentCamera];
            DrawBackground(spriteBatch, camera);
            DrawForeground(spriteBatch);
            currentButtonSprite.Draw(buttonPosition, spriteBatch);
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
                Game1.Instance.manager.FillAllPlayerLives();
                level.NextRoom = "hub";//level.PreviousRoom;
                level.NextSpawn = null;//level.CurrentRoom.SpawnPoint;
                level.ChangeToTransitionState();
            }
        }

    }
}
