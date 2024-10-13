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

        float levelWidth = Constants.Graphics.LEVEL_WIDTH;
        public Matrix LevelMatrix { get; set; }
        public Matrix ScreenMatrix { get; set; }
        public Matrix backgroundMatrix { get; set; }

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
            UpdateCameraPosition();
            UpdateBounds();
            UpdateMatrices();
        }

        public void UpdateCameraPosition()
        {
            float targetX = _targetPlayer?.GetKirbyPosition().X ?? position.X;
            float newCameraX = targetX - Constants.Graphics.GAME_WIDTH / 2;

            // Check if Kirby is at the edge of the terrain
            if (newCameraX < 0)
            {
                newCameraX = 0; // Prevent moving left off the screen
            }
            else if (newCameraX > levelWidth - Constants.Graphics.GAME_WIDTH)
            {
                newCameraX = levelWidth - Constants.Graphics.GAME_WIDTH; // Prevent moving right off the terrain
            }

            // Update the camera's position
            position.X = newCameraX;


            // ATROCIOUS COUPLING, I KNOW- i just wanted to get it working, i'll fix it later T______T
            //if (_game.level.room.CameraXLock)
            //{
            //    position.X = _game.level.room.LockedCameraX;
            //}
            //else
            //{
            //    position.X = _targetPlayer?.GetKirbyPosition().X - Constants.Graphics.GAME_WIDTH / 2 ?? position.X;
            //}
            //if (_game.level.room.CameraYLock)
            //{
            //    position.Y = _game.level.room.LockedCameraY;
            //}
            //else
            //{
            //    position.Y = _targetPlayer?.GetKirbyPosition().Y - Constants.Graphics.GAME_HEIGHT / 2 ?? position.Y;
            //}
            //position.Floor();

            position.X = (float)Math.Floor(position.X);
            position.Y = (float)Math.Floor(position.Y);

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

            // Update the LevelMatrix for the foreground
            LevelMatrix = Matrix.CreateTranslation(-position.X, -position.Y, 0) * Matrix.CreateScale(scale);

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
