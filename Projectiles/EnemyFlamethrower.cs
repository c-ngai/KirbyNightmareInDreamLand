using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MasterGame.Projectiles
{
    public class EnemyFlamethrower
    {
        private List<EnemyFlameSegment> flameSegments;
        private Vector2 startPosition;
        private float elapsedTime;
        private Vector2 flameDirection; 

        public EnemyFlamethrower()
        {
            flameSegments = new List<EnemyFlameSegment>();
            elapsedTime = 0f;
        }

        public void Update(GameTime gameTime, Vector2 startPosition, Vector2 flameDirection)
        {
            this.startPosition = startPosition;
            this.flameDirection = flameDirection;
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check if it's time to spawn new flame segments
            if (elapsedTime >= Constants.EnemyFire.FIRE_RATE)
            {
                SpawnFlameSegments();
                elapsedTime = 0f; // Reset elapsed time
            }

            // Update all flame segments
            foreach (var segment in flameSegments)
            {
                segment.Update();
            }
        }

        private void SpawnFlameSegments()
        {
            Random random = new Random();

            // Create multiple flame segments with varying angles and delays
            for (int i = -Constants.EnemyFire.NUMBER_OF_SEGMENTS / 2; i <= Constants.EnemyFire.NUMBER_OF_SEGMENTS / 2; i++)
            {
                float totalAngleRange = Constants.EnemyFire.MAX_ANGLE - Constants.EnemyFire.MIN_ANGLE;

                // Calculate the angle offset based on the number of segments
                float angle = Constants.EnemyFire.MIN_ANGLE + (i + (Constants.EnemyFire.NUMBER_OF_SEGMENTS / 2)) * (totalAngleRange / Constants.EnemyFire.NUMBER_OF_SEGMENTS);

                // Ensure the angle stays within the minAngle and maxAngle range
                if (angle < Constants.EnemyFire.MIN_ANGLE) angle = Constants.EnemyFire.MIN_ANGLE;
                if (angle > Constants.EnemyFire.MAX_ANGLE) angle = Constants.EnemyFire.MAX_ANGLE;

                // Rotate the direction vector by the calculated angle
                Vector2 direction = Vector2.Transform(flameDirection, Matrix.CreateRotationZ(angle));

                // Generate a random speed and delay for each segment
                float randomSpeed = (float)(random.NextDouble() * (Constants.EnemyFire.MAX_SPEED - Constants.EnemyFire.MIN_SPEED) + Constants.EnemyFire.MIN_SPEED);
                float randomDelay = (float)(random.NextDouble() * (Constants.EnemyFire.MAX_DELAY - Constants.EnemyFire.MIN_DELAY) + Constants.EnemyFire.MIN_DELAY);

                EnemyFlameSegment newSegment = new EnemyFlameSegment(startPosition, direction, randomSpeed, randomDelay);
                flameSegments.Add(newSegment);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var segment in flameSegments)
            {
                segment.Draw(spriteBatch);
            }
        }
        public void ClearSegments()
        {
            flameSegments.Clear();
        }
    }
}
