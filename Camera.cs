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
        public Matrix LevelMatrix { get; set; }
        public Matrix ScreenMatrix { get; set; }

        public Camera(Game1 game)
        {
            _game = game;

            position = new Vector3(0,0,0);
            bounds = new Rectangle((int)position.X, (int)position.Y, Constants.Graphics.GAME_WIDTH, Constants.Graphics.GAME_HEIGHT);

            LevelMatrix = new Matrix();
            ScreenMatrix = new Matrix();
        }

        public void Update()
        {
            // ATROCIOUS COUPLING, I KNOW- i just wanted to get it working, i'll fix it later T______T
            if (_game.level.room.CameraXLock)
            {
                position.X = _game.level.room.LockedCameraX;
            }
            else
            {
                position.X = _targetPlayer?.GetKirbyPosition().X - Constants.Graphics.GAME_WIDTH / 2 ?? position.X;
            }
            if (_game.level.room.CameraYLock)
            {
                position.Y = _game.level.room.LockedCameraY;
            }
            else
            {
                position.Y = _targetPlayer?.GetKirbyPosition().Y - Constants.Graphics.GAME_HEIGHT / 2 ?? position.Y;
            }
            position.Floor();

            // Adjust the X and Y of the bounds Rectangle
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;

            // Update matrices
            float scale = _game.WINDOW_HEIGHT / Constants.Graphics.GAME_HEIGHT;
            LevelMatrix = Matrix.CreateTranslation(-position) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(_game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET, 0);
            ScreenMatrix = Matrix.CreateScale(scale) * Matrix.CreateTranslation(_game.WINDOW_XOFFSET, _game.WINDOW_YOFFSET, 0);
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
