﻿using KirbyNightmareInDreamLand.Entities.Players;
using KirbyNightmareInDreamLand.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static KirbyNightmareInDreamLand.Constants;

namespace KirbyNightmareInDreamLand
{
    public class Camera
    {

        private Game1 _game;
        private ObjectManager _objectManager;
        private int playerIndex;
        private IPlayer _targetPlayer;

        private Vector3 position;
        public Rectangle bounds;
        public Rectangle enemyBounds;
        private Rectangle oldEnemyBounds;
        public Rectangle enemyRespawnBounds;

        public Rectangle ScissorRectangle;

        // Matrix for the level, everything drawn here is in world space.
        public Matrix LevelMatrix { get; set; }
        // Matrix for the screen, everything drawn here is directly in screen space. For HUD, etc. Things not part of the actual "game world".
        public Matrix ScreenMatrix { get; set; }

        public Camera(int playerIndex)
        {
            _game = Game1.Instance;
            _objectManager = ObjectManager.Instance;
            this.playerIndex = playerIndex;

            position = new Vector3(0, 0, 0);
            bounds = new Rectangle(
                (int)position.X,
                (int)position.Y,
                Constants.Graphics.GAME_WIDTH,
                Constants.Graphics.GAME_HEIGHT
            );
            enemyBounds = new Rectangle(
                (int)position.X - Constants.Enemies.SPAWN_BOUNDS_OFFSET,
                (int)position.Y - Constants.Enemies.SPAWN_BOUNDS_OFFSET,
                Constants.Graphics.GAME_WIDTH + 2 * Constants.Enemies.SPAWN_BOUNDS_OFFSET,
                Constants.Graphics.GAME_HEIGHT + 2 * Constants.Enemies.SPAWN_BOUNDS_OFFSET
            );

            ScissorRectangle = new Rectangle(_game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET, _game.WINDOW_WIDTH, _game.WINDOW_HEIGHT);

            LevelMatrix = new Matrix();
            ScreenMatrix = new Matrix();

        }

        public void Update()
        {
            // If player of this Camera's index exists, target it and update the camera to track it
            if (playerIndex < _objectManager.Players.Count)
            {
                _targetPlayer = _objectManager.Players[playerIndex];
                UpdateCameraPosition();
                UpdateBounds();
                UpdateMatrices();
            }
        }

        private void UpdateCameraPosition()
        {
            // Set the camera's X
            if (_game.Level.CurrentRoom.CameraXLock)
            {
                position.X = _game.Level.CurrentRoom.LockedCameraX;
            }
            else
            {
                if (_targetPlayer != null && !_targetPlayer.DEAD)
                {
                    position.X = _targetPlayer.GetKirbyPosition().X - Constants.Graphics.GAME_WIDTH / 2;
                    // Bound camera X by room width
                    if (position.X < 0 || Constants.Graphics.GAME_WIDTH > _game.Level.CurrentRoom.Width)
                    {
                        position.X = 0;
                    }
                    else if (position.X > _game.Level.CurrentRoom.Width - Constants.Graphics.GAME_WIDTH)
                    {
                        position.X = _game.Level.CurrentRoom.Width - Constants.Graphics.GAME_WIDTH;
                    }
                }
            }

            // Set the camera's Y
            if (_game.Level.CurrentRoom.CameraYLock)
            {
                position.Y = _game.Level.CurrentRoom.LockedCameraY;
            }
            else
            {
                if (_targetPlayer != null && !_targetPlayer.DEAD)
                {
                    position.Y = _targetPlayer.GetKirbyPosition().Y - Constants.Graphics.GAME_HEIGHT / 2;
                    // Bound camera Y by room height
                    if (position.Y < 0 || Constants.Graphics.GAME_HEIGHT > _game.Level.CurrentRoom.Height)
                    {
                        position.Y = 0;
                    }
                    else if (position.Y > _game.Level.CurrentRoom.Height - Constants.Graphics.GAME_HEIGHT)
                    {
                        position.Y = _game.Level.CurrentRoom.Height - Constants.Graphics.GAME_HEIGHT;
                    }
                }
            }

            // Floor the camera's position to round it down to the nearest pixel, alignment can be weird otherwise
            position.Floor();
        }

        private void UpdateBounds()
        {
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;

            oldEnemyBounds = enemyBounds;

            enemyBounds.X = (int)position.X - Constants.Enemies.SPAWN_BOUNDS_OFFSET;
            enemyBounds.Y = (int)position.Y - Constants.Enemies.SPAWN_BOUNDS_OFFSET;

            ScissorRectangle.X = _game.WINDOW_XOFFSET;
            ScissorRectangle.Y = _game.WINDOW_YOFFSET;
            ScissorRectangle.Width = _game.WINDOW_WIDTH;
            ScissorRectangle.Height = _game.WINDOW_HEIGHT;
        }

        private void UpdateMatrices()
        {
            float scale = _game.WINDOW_HEIGHT / Constants.Graphics.GAME_HEIGHT;
            LevelMatrix = Matrix.CreateTranslation(-position);
            //ScreenMatrix = Matrix.CreateScale(scale) * Matrix.CreateTranslation(_game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET, 0);
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public Rectangle GetBounds()
        {
            return bounds;
        }

        public Rectangle GetEnemyBounds()
        {
            return enemyBounds;
        }

        public static bool InAnyActiveCamera (Vector2 position) {
            for (int i = 0; i < Game1.Instance.ActiveCameraCount; i++)
            {
                if (Game1.Instance.cameras[i].bounds.Contains(position))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool InAnyActiveEnemyBounds(Vector2 position)
        {
            for (int i = 0; i < Game1.Instance.ActiveCameraCount; i++)
            {
                if (Game1.Instance.cameras[i].enemyBounds.Contains(position))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool InAnyEnemyRespawnBounds(Vector2 position)
        {
            // If level is in a new room this update, it doesn't matter where the cameras were last update.
            if (!Game1.Instance.Level.NewRoom)
            {
                for (int i = 0; i < Game1.Instance.ActiveCameraCount; i++)
                {
                    if (Game1.Instance.cameras[i].oldEnemyBounds.Contains(position))
                    {
                        return false;
                    }
                }
            }
            for (int i = 0; i < Game1.Instance.ActiveCameraCount; i++)
            {
                if (Game1.Instance.cameras[i].enemyBounds.Contains(position))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
