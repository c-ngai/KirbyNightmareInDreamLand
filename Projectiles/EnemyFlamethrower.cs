using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyFlamethrower
    {
        private List<EnemyFlameSegment> flameSegments;
        private Vector2 startPosition;
        private float elapsedTime;
        private Vector2 flameDirection; 
        private bool isLeft;
        private int frameCounter;

        public EnemyFlamethrower(Vector2 startPosition, bool isLeft)
        {
            this.startPosition = startPosition;
            this.flameDirection = isLeft ? Constants.Hothead.FLAMETHROWER_LEFT : Constants.Hothead.FLAMETHROWER_RIGHT;
            this.isLeft = isLeft;
            flameSegments = new List<EnemyFlameSegment>();
            elapsedTime = 0f;
            frameCounter = 0;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 offset = !isLeft ? Constants.Kirby.FLAME_ATTACK_OFFSET_RIGHT : Constants.Kirby.FLAME_ATTACK_OFFSET_LEFT;
            //this.position = player.GetKirbyPosition() + offset;

            // Check if it's time to spawn new flame segments
            if (frameCounter % 1 == 0)
            {
                SpawnFlameSegment();
            }
            frameCounter++;

            // Update all flame segments
            foreach (var segment in flameSegments)
            {
                //segment.Update();
            }
            flameSegments.RemoveAll(obj => obj.IsDone());
        }
         private void SpawnFlameSegment()
        {
            Random random = new Random();
            float angleOffset = !isLeft ? 0f : (float)Math.PI; // Adjust angle based on direction

            float totalAngleRange = Constants.KirbyFire.MAX_ANGLE - Constants.KirbyFire.MIN_ANGLE;

            // Calculate the angle offset based on the number of segments
            float angle = Constants.KirbyFire.MIN_ANGLE + (float)random.NextDouble() * totalAngleRange;

           // Rotate the direction vector by the calculated angle
            Vector2 direction = Vector2.Transform(flameDirection, Matrix.CreateRotationZ(angle));

            float randomSpeed = (float)(random.NextDouble() * (Constants.EnemyFire.MAX_SPEED - Constants.EnemyFire.MIN_SPEED) + Constants.EnemyFire.MIN_SPEED);
            Vector2 velocity = randomSpeed * Vector2.Transform(new Vector2(1, 0), Matrix.CreateRotationZ(angle + angleOffset));

            EnemyFlameSegment newSegment = new EnemyFlameSegment(startPosition, velocity, randomSpeed);
            flameSegments.Add(newSegment);
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

                EnemyFlameSegment newSegment = new EnemyFlameSegment(startPosition, direction, randomSpeed);
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
