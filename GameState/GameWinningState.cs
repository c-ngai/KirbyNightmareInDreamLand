﻿using System;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KirbyNightmareInDreamLand.GameState
{
	public class GameWinningState : BaseGameState
	{
        private ObjectManager _manager;
        SpriteBatch spriteBatch;
        public Sprite currentButtonSprite;
        public Sprite selectQuitScreen;
        public Sprite selectContinueScreen;

        private Vector2 kirbyStartRoomSpawn = Constants.Level.ROOM1_SPAWN_POINT;
        private string room1String = Constants.RoomStrings.ROOM_1;
        private Vector2 buttonPosition = Constants.ButtonLocations.LEVEL_COMPLETE_BUTTONS;

        public GameWinningState(Levels.Level _level) : base(_level)
        {

            _manager = Game1.Instance.manager;
            spriteBatch = Game1.Instance._spriteBatch;
            selectContinueScreen = SpriteFactory.Instance.CreateSprite("Winning_continue_selected_button");
            selectQuitScreen = SpriteFactory.Instance.CreateSprite("Winning_quit_selected");
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

