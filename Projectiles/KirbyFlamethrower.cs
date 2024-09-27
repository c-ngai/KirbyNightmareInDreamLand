using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MasterGame
{
    public class KirbyFlamethrower
    {
        private List<KirbyFlameSegment> flameSegments;
        private Vector2 startPosition; 
        private float fireRate = 0.35f; // Time between each segment spawn
        private float elapsedTime; 
        private Vector2 flameDirection;
        private int numberOfSegments = 10;
        
        private const float MinSpeed = 1f; 
        private const float MaxSpeed = 4f; 
        private const float MinDelay = 0f; 
        private const float MaxDelay = 0.3f; 
        private const float MinAngle = -0.3f;
        private const float MaxAngle = 0.3f;

        public KirbyFlamethrower()
        {
            flameSegments = new List<KirbyFlameSegment>();
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
                float totalAngleRange = MaxAngle - MinAngle;

                // Calculate the angle offset based on the number of segments
                float angle = MinAngle + (i + (numberOfSegments / 2)) * (totalAngleRange / numberOfSegments);

                // Ensure the angle stays within the minAngle and maxAngle range
                if (angle < MinAngle) angle = MinAngle;
                if (angle > MaxAngle) angle = MaxAngle;

                // Rotate the direction vector by the calculated angle
                Vector2 direction = Vector2.Transform(flameDirection, Matrix.CreateRotationZ(angle));

                // Generate a random speed and delay for each segment
                float randomSpeed = (float)(random.NextDouble() * (MaxSpeed - MinSpeed) + MinSpeed);
                float randomDelay = (float)(random.NextDouble() * (MaxDelay - MinDelay) + MinDelay);

                KirbyFlameSegment newSegment = new KirbyFlameSegment(startPosition, direction, randomSpeed, randomDelay);
                flameSegments.Add(newSegment);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw all flame segments
            foreach (var segment in flameSegments)
            {
                segment.Draw(spriteBatch);
            }
        }
    }
}
