using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KirbyNightmareInDreamLand
{
    public class Camera
    {

        private Game1 _game;
        private IPlayer _targetPlayer;

        private Vector2 position;
        private Rectangle bounds;

        public int LeftX { get; private set; }
        public int RightX { get; private set; }
        public int TopY { get; private set; }
        public int BottomY { get; private set; }

        public Camera(Game1 game)
        {
            _game = game;

            position = new Vector2(0,0);
            bounds = new Rectangle((int)position.X, (int)position.Y, Constants.Graphics.GAME_WIDTH, Constants.Graphics.GAME_HEIGHT);
            LeftX = (int)position.X;
            RightX = (int)position.X + Constants.Graphics.GAME_WIDTH;
            TopY = (int)position.Y;
            BottomY = (int)position.Y + Constants.Graphics.GAME_HEIGHT;
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
                position.X = _targetPlayer.GetKirbyPosition().X - Constants.Graphics.GAME_WIDTH / 2;
            }
            if (_game.level.room.CameraYLock)
            {
                position.Y = _game.level.room.LockedCameraY;
            }
            else
            {
                position.Y = _targetPlayer.GetKirbyPosition().Y - Constants.Graphics.GAME_HEIGHT / 2;
            }
            

            // Adjust the X and Y of the bounds Rectangle
            bounds.X = (int)position.X;
            bounds.Y = (int)position.Y;

            // Adjust the side coordinates
            LeftX = (int)position.X;
            RightX = (int)position.X + Constants.Graphics.GAME_WIDTH;
            TopY = (int)position.Y;
            BottomY = (int)position.Y + Constants.Graphics.GAME_HEIGHT;
        }

        public void TargetPlayer(IPlayer targetPlayer)
        {
            _targetPlayer = targetPlayer;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public Rectangle GetBounds()
        {
            return bounds;
        }

    }
}
