
using KirbyNightmareInDreamLand.Entities.Enemies;
using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Entities.PowerUps;
using KirbyNightmareInDreamLand.Levels;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace KirbyNightmareInDreamLand.GameState
{
    public interface IGameState
    {
        void Draw();
        void Update();
    }

    public abstract class BaseGameState : IGameState
    {
        private readonly Game1 _game;
        private readonly Camera _camera;
        private readonly SpriteBatch spriteBatch;
        public Room CurrentRoom;
        private readonly Sprite DoorStarsSprite;
        public Vector2 SpawnPoint { get; set; }

        public BaseGameState()
        {
            _game = Game1.Instance;
            _camera = _game.Camera;
            spriteBatch = _game._spriteBatch;
            CurrentRoom = LevelLoader.Instance.Rooms["room1"]; // when initializing the playing game state, start in room1
            DoorStarsSprite = SpriteFactory.Instance.CreateSprite("doorstars");
            LevelLoader.Instance.LoadKeymap(""); // TODO: LINK KEY MAPS TO GAME STATES 
        }

        public virtual void Draw()
        {
            DrawBackground(spriteBatch);
            DrawDoorStars(spriteBatch);
            DrawLevelObjects(spriteBatch);
        }

        public virtual void Update()
        {
            CurrentRoom.ForegroundSprite.Update();
            DoorStarsSprite.Update();
            foreach (Enemy enemy in Game1.Instance.Level.enemyList)
            {
                enemy.Update(_game.time);
            }
            foreach (PowerUp powerUp in Game1.Instance.Level.powerUpList)
            {
                powerUp.Update();
            }
        }

        public void DrawBackground(SpriteBatch spriteBatch)
        {
            if (CurrentRoom.BackgroundSprite != null)
            {
                Vector2 cameraPosition = new Vector2(
                    _camera.GetPosition().X * (1),
                    _camera.GetPosition().Y * (1)
                );

                Vector2 backgroundScreenPosition = new Vector2(
                    _camera.GetPosition().X * ((float)(_camera.bounds.Width - CurrentRoom.BackgroundSprite.Width) / (CurrentRoom.Width - _camera.bounds.Width)),
                    _camera.GetPosition().Y * ((float)(_camera.bounds.Height - CurrentRoom.BackgroundSprite.Height) / (CurrentRoom.Height - _camera.bounds.Height))
                );

                Vector2 backgroundPosition = cameraPosition + backgroundScreenPosition;
                CurrentRoom.BackgroundSprite.Draw(backgroundPosition, spriteBatch);
            }
        }

        // only draw foreground is not implemented in the base class 
        public void DrawForeground(SpriteBatch spriteBatch)
        {
            if (CurrentRoom.ForegroundSprite != null)
            {
                CurrentRoom.ForegroundSprite.Draw(Vector2.Zero, spriteBatch);
            }
        }

        // draws enemies and tomatoes
        public void DrawLevelObjects(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in Game1.Instance.Level.enemyList)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (PowerUp powerUp in Game1.Instance.Level.powerUpList)
            {
                powerUp.Draw(spriteBatch);
            }
        }

        // Draws the stars around each door
        public void DrawDoorStars(SpriteBatch spriteBatch)
        {
            foreach (Door door in CurrentRoom.Doors)
            {
                Vector2 doorPos = door.Bounds.Location.ToVector2();
                DoorStarsSprite.Draw(doorPos, spriteBatch);
            }
        }
    }
}

