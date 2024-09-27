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
            for (int i = -Constants.KirbyFire.NUMBER_OF_SEGMENTS / 2; i <= Constants.KirbyFire.NUMBER_OF_SEGMENTS / 2; i++)
            {
                float totalAngleRange = Constants.KirbyFire.MAX_ANGLE - Constants.KirbyFire.MIN_ANGLE;

                // Calculate the angle offset based on the number of segments
                float angle = Constants.KirbyFire.MIN_ANGLE + (i + (Constants.KirbyFire.NUMBER_OF_SEGMENTS / 2)) * (totalAngleRange / Constants.KirbyFire.NUMBER_OF_SEGMENTS);

                // Ensure the angle stays within the minAngle and maxAngle range
                if (angle < Constants.KirbyFire.MIN_ANGLE) angle = Constants.KirbyFire.MIN_ANGLE;
                if (angle > Constants.KirbyFire.MAX_ANGLE) angle = Constants.KirbyFire.MAX_ANGLE;

                // Rotate the direction vector by the calculated angle
                Vector2 direction = Vector2.Transform(flameDirection, Matrix.CreateRotationZ(angle));

                // Generate a random speed and delay for each segment
                float randomSpeed = (float)(random.NextDouble() * (Constants.KirbyFire.MAX_SPEED - Constants.KirbyFire.MIN_SPEED) + Constants.KirbyFire.MIN_SPEED);
                float randomDelay = (float)(random.NextDouble() * (Constants.KirbyFire.MAX_DELAY - Constants.KirbyFire.MIN_DELAY) + Constants.KirbyFire.MIN_DELAY);

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
