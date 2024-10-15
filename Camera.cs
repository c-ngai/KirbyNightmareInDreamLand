using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand
{
    public class Camera
    {

        private Game1 _game;
        private IPlayer _targetPlayer;

        private Vector3 position;
        private Rectangle bounds;

        // Parallax factor for the background (1.0 = same speed as foreground)
        public float BackgroundParallaxFactor { get; set; } = 0.5f;

        // Matrix for the level, everything drawn here is in world space.
        public Matrix LevelMatrix { get; set; }
        // Matrix for the screen, everything drawn here is directly in screen space. For HUD, etc. Things not part of the actual "game world".
        public Matrix ScreenMatrix { get; set; }
        public Matrix backgroundMatrix { get; set; }

        public Camera()
        {
            _game = Game1.Instance;

            position = new Vector3(0,0,0);
            bounds = new Rectangle((int)position.X, (int)position.Y, Constants.Graphics.GAME_WIDTH, Constants.Graphics.GAME_HEIGHT);

            LevelMatrix = new Matrix();
            ScreenMatrix = new Matrix();
        }

        public void Update()
        {
            UpdateCameraPosition();
            UpdateBounds();
            UpdateMatrices();
        }

        public void UpdateCameraPosition()
        {
            // Set the camera's X
            if (_game.level.room.CameraXLock)
            {
                position.X = _game.level.room.LockedCameraX;
            }
            else
            {
                position.X = _targetPlayer?.GetKirbyPosition().X - Constants.Graphics.GAME_WIDTH / 2 ?? position.X;
                // Bound camera X by room width
                if (position.X < 0)
                {
                    position.X = 0;
                }
                else if (position.X > _game.level.room.Width - Constants.Graphics.GAME_WIDTH)
                {
                    position.X = _game.level.room.Width - Constants.Graphics.GAME_WIDTH;
                }
            }

            // Set the camera's Y
            if (_game.level.room.CameraYLock)
            {
                position.Y = _game.level.room.LockedCameraY;
            }
            else
            {
                position.Y = _targetPlayer?.GetKirbyPosition().Y - Constants.Graphics.GAME_HEIGHT / 2 ?? position.Y;
                // Bound camera Y by room height
                if (position.Y < 0)
                {
                    position.Y = 0;
                }
                else if (position.Y > _game.level.room.Height - Constants.Graphics.GAME_HEIGHT)
                {
                    position.Y = _game.level.room.Height - Constants.Graphics.GAME_HEIGHT;
                }
            }

            // Floor the camera's position to round it down to the nearest pixel, alignment can be weird otherwise
            position.Floor();
        }

        public void UpdateBounds()
        {
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;
        }

        public void UpdateMatrices()
        {
            float scale = _game.WINDOW_HEIGHT / Constants.Graphics.GAME_HEIGHT;

            // Calculate background position for parallax effect
            Vector3 backgroundPosition = new Vector3(
                position.X * BackgroundParallaxFactor,
                position.Y * BackgroundParallaxFactor,
                position.Z
            );

            
            LevelMatrix = Matrix.CreateTranslation(-position) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(_game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET, 0);
            ScreenMatrix = Matrix.CreateScale(scale) * Matrix.CreateTranslation(_game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET, 0);

            // Update the background matrix for the background
            backgroundMatrix = Matrix.CreateTranslation(-backgroundPosition.X, -backgroundPosition.Y, 0) * Matrix.CreateScale(scale);

            
        }

        public void TargetPlayer(IPlayer targetPlayer)
        {
            _targetPlayer = targetPlayer;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public Rectangle GetBounds()
        {
            return bounds;
        }

    }
}
