using KirbyNightmareInDreamLand.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyFlamethrower : IProjectile
    {
        private Vector2 position;
        private Vector2 velocity;
        private List<KirbyFlameSegment> flameSegments;
        private float elapsedTime;
        private bool isFacingRight; // Boolean to determine the direction Kirby is facing
        private SoundInstance sound;

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public Vector2 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public KirbyFlamethrower(Vector2 kirbyPosition, bool isFacingRight)
        {
            this.isFacingRight = isFacingRight;
            Vector2 offset = isFacingRight ? Constants.Kirby.FLAME_ATTACK_OFFSET_RIGHT : Constants.Kirby.FLAME_ATTACK_OFFSET_LEFT;
            this.position = kirbyPosition + offset;

            flameSegments = new List<KirbyFlameSegment>();
            elapsedTime = 0f;

            ObjectManager.Instance.AddProjectile(this);

            sound = SoundManager.CreateInstance("kirbyfireattack");
            sound.Play();
        }
        public void EndAttack()
        {
            sound.Stop();
            foreach (var segment in flameSegments)
            {
                segment.EndAttack();
            }
        }
        public bool IsDone()
        {
            foreach (var segment in flameSegments)
            {
                segment.IsDone();
            }
            return true;
        }
        public void Update()
        {
            // Use a static reference to Game1 to access the game time.
            elapsedTime += (float)Game1.GameTime.ElapsedGameTime.TotalSeconds;

            // Check if it's time to spawn new flame segments
            if (elapsedTime >= Constants.KirbyFire.FIRE_RATE)
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
            float angleOffset = isFacingRight ? 0f : (float)Math.PI; // Adjust angle based on direction

            // Create multiple flame segments with varying angles and delays
            for (int i = -Constants.KirbyFire.NUMBER_OF_SEGMENTS / 2; i <= Constants.KirbyFire.NUMBER_OF_SEGMENTS / 2; i++)
            {
                float totalAngleRange = Constants.KirbyFire.MAX_ANGLE - Constants.KirbyFire.MIN_ANGLE;

                // Calculate the angle offset based on the number of segments
                float angle = Constants.KirbyFire.MIN_ANGLE + (i + (Constants.KirbyFire.NUMBER_OF_SEGMENTS / 2)) * (totalAngleRange / Constants.KirbyFire.NUMBER_OF_SEGMENTS);

                // Ensure the angle stays within the minAngle and maxAngle range
                if (angle < Constants.KirbyFire.MIN_ANGLE) angle = Constants.KirbyFire.MIN_ANGLE;
                if (angle > Constants.KirbyFire.MAX_ANGLE) angle = Constants.KirbyFire.MAX_ANGLE;

                // Rotate the direction vector by the calculated angle, adjusting for facing direction
                Vector2 direction = Vector2.Transform(new Vector2(1, 0), Matrix.CreateRotationZ(angle + angleOffset));

                // Generate a random speed and delay for each segment
                float randomSpeed = (float)(random.NextDouble() * (Constants.KirbyFire.MAX_SPEED - Constants.KirbyFire.MIN_SPEED) + Constants.KirbyFire.MIN_SPEED);
                float randomDelay = (float)(random.NextDouble() * (Constants.KirbyFire.MAX_DELAY - Constants.KirbyFire.MIN_DELAY) + Constants.KirbyFire.MIN_DELAY);

                KirbyFlameSegment newSegment = new KirbyFlameSegment(position, direction, randomSpeed, randomDelay, !isFacingRight);
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
