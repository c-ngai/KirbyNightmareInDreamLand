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
        private int wait = 16;

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
            // Check if it's time to spawn new flame segments
            if (frameCounter >=  Random.Shared.Next(15, 20))
            {
                SpawnFlameSegment();
            }
            frameCounter++;

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

            float randomSpeed = (float)(random.NextDouble() * (Constants.EnemyFire.MAX_SPEED - Constants.EnemyFire.MIN_SPEED) + Constants.EnemyFire.MIN_SPEED);
            Vector2 velocity = randomSpeed * Vector2.Transform(new Vector2(1, 0), Matrix.CreateRotationZ(angle + angleOffset));

            EnemyFlameSegment newSegment = new EnemyFlameSegment(startPosition, velocity, randomSpeed);
            flameSegments.Add(newSegment);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var segment in flameSegments)
            {
                //segment.Draw(spriteBatch);
            }
        }
        public void ClearSegments()
        {
            flameSegments.Clear();
        }
    }
}
