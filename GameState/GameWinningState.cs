using System;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.GameState
{
	public class GameWinningState : BaseGameState
	{
        private Game1 _game;
        private ObjectManager _manager;
        public Sprite currentButtonSprite;
        public Sprite selectQuitScreen;
        public Sprite selectContinueScreen;

        private Vector2 kirbyHubRoomSpawn = Constants.Level.HUB_SPAWN_POINT;
        private string room1String = Constants.RoomStrings.ROOM_1;
        private Vector2 buttonPosition = Constants.ButtonLocations.LEVEL_COMPLETE_BUTTONS;

        public GameWinningState(Levels.Level _level) : base(_level)
        {
            _game = Game1.Instance;
            _manager = Game1.Instance.manager;
            selectContinueScreen = SpriteFactory.Instance.CreateSprite("Winning_continue_selected_button");
            selectQuitScreen = SpriteFactory.Instance.CreateSprite("Winning_quit_selected");
            currentButtonSprite = selectContinueScreen;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Camera camera = _game.cameras[_game.CurrentCamera];
            DrawBackground(spriteBatch, camera);
            DrawForeground(spriteBatch);
            currentButtonSprite.Draw(buttonPosition, spriteBatch);
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

        public override void SelectQuitButton()
        {
            currentButtonSprite = selectQuitScreen;
            SoundManager.Play("movecursor");
        }

        public override void SelectContinueButton()
        {
            currentButtonSprite = selectContinueScreen;
            SoundManager.Play("movecursor");
        }

        public override void SelectButton()
        {
            SoundManager.Play("select");
            if (currentButtonSprite == selectQuitScreen)
            {               
                Game1.Instance.Exit();

            }
            else
            {
                if(level.PreviousRoom == "room3")
                {
                    level.NextRoom = "hub";
                    level.NextSpawn = null; //new Vector2(112, 270);
                    // level.LoadRoom(level.NextRoom, level.NextSpawn); // load new room
                    level.ChangeToTransitionState();
                }
                
            }
        }

    }
}


