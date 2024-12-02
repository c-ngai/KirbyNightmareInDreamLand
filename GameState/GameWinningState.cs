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

        private Vector2 buttonPosition = Constants.ButtonLocations.LEVEL_COMPLETE_BUTTONS;

        private Vector2 hubDoor1Position = Constants.Level.HUB_DOOR_1_SPAWN_POINT;
        private Vector2 hubDoor2Position = Constants.Level.HUB_DOOR_2_SPAWN_POINT;

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
            _manager.DrawAllObjects(spriteBatch);
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
                    level.NextSpawn = hubDoor1Position;
                    level.ChangeToTransitionState();
                }
                if (level.PreviousRoom == "level2_room3")
                {
                    level.NextRoom = "hub";
                    level.NextSpawn = hubDoor2Position;
                    level.ChangeToTransitionState();
                }
            }
        }

    }
}


