using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MasterGame
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
            if (elapsedTime >= fireRate)
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
            for (int i = -numberOfSegments / 2; i <= numberOfSegments / 2; i++)
            {
                float totalAngleRange = maxAngle - minAngle;

                // Calculate the angle offset based on the number of segments
                float angle = minAngle + (i + (numberOfSegments / 2)) * (totalAngleRange / numberOfSegments);

                // Ensure the angle stays within the minAngle and maxAngle range
                if (angle < minAngle) angle = minAngle;
                if (angle > maxAngle) angle = maxAngle;

                // Rotate the direction vector by the calculated angle
                Vector2 direction = Vector2.Transform(flameDirection, Matrix.CreateRotationZ(angle));

                // Generate a random speed and delay for each segment
                float randomSpeed = (float)(random.NextDouble() * (maxSpeed - minSpeed) + minSpeed);
                float randomDelay = (float)(random.NextDouble() * (maxDelay - minDelay) + minDelay);

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
