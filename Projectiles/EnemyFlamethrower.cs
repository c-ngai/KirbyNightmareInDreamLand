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

        private const float FireRate = 0.35f; // Time between each segment spawn
        private const int NumberOfSegments = 10; // Number of flame segments to spawn at once
        private const float MinSpeed = 1f;
        private const float MaxSpeed = 4f;
        private const float MinDelay = 0f;
        private float MaxDelay = 0.3f;
        private const float MinAngle = -0.3f;
        private const float MaxAngle = 0.3f;

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
            if (elapsedTime >= FireRate)
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
            for (int i = -NumberOfSegments / 2; i <= NumberOfSegments / 2; i++)
            {
                float totalAngleRange = MaxAngle - MinAngle;

                // Calculate the angle offset based on the number of segments
                float angle = MinAngle + (i + (NumberOfSegments / 2)) * (totalAngleRange / NumberOfSegments);

                // Ensure the angle stays within the minAngle and maxAngle range
                if (angle < MinAngle) angle = MinAngle;
                if (angle > MaxAngle) angle = MaxAngle;

                // Rotate the direction vector by the calculated angle
                Vector2 direction = Vector2.Transform(flameDirection, Matrix.CreateRotationZ(angle));

                // Generate a random speed and delay for each segment
                float randomSpeed = (float)(random.NextDouble() * (MaxSpeed - MinSpeed) + MinSpeed);
                float randomDelay = (float)(random.NextDouble() * (MaxDelay - MinDelay) + MinDelay);

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
