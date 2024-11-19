using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyFlamethrower : IProjectile, ICollidable
    {

        private IPlayer player;
        private Vector2 position;
        private Vector2 velocity;
        private List<KirbyFlameSegment> flameSegments;
        private int frameCounter;
        private bool IsActive;
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

        public KirbyFlamethrower(IPlayer player, bool isFacingRight)
        {
            this.player = player;
            
            this.isFacingRight = isFacingRight;
            Vector2 offset = isFacingRight ? Constants.Kirby.FLAME_ATTACK_OFFSET_RIGHT : Constants.Kirby.FLAME_ATTACK_OFFSET_LEFT;
            this.position = player.GetKirbyPosition() + offset;
            IsActive = true;

            flameSegments = new List<KirbyFlameSegment>();
            frameCounter = 0;

            ObjectManager.Instance.AddProjectile(this);

            sound = SoundManager.CreateInstance("kirbyfireattack");
            sound.Play();
        }
        public void EndAttack()
        {
            sound.Stop();
            IsActive = false;
            //foreach (var segment in flameSegments)
            //{
            //    segment.EndAttack();
            //}
        }
        public bool IsDone()
        {
            if (frameCounter < 10)
            {
                return false;
            }
            foreach (var segment in flameSegments)
            {
                if (!segment.IsDone())
                {
                    return false;
                }
            }
            return true;
        }
        public void Update()
        {
            Vector2 offset = isFacingRight ? Constants.Kirby.FLAME_ATTACK_OFFSET_RIGHT : Constants.Kirby.FLAME_ATTACK_OFFSET_LEFT;
            this.position = player.GetKirbyPosition() + offset;

            // Check if it's time to spawn new flame segments
            if (IsActive && frameCounter % 1 == 0)
            {
                SpawnFlameSegment();
            }
            frameCounter++;

            // Update all flame segments
            foreach (var segment in flameSegments)
            {
                segment.Update();
            }
            //This line below doesn't work because the flame segments start out as inactive and then wait to become active.
            flameSegments.RemoveAll(obj => obj.IsDone());
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

                KirbyFlameSegment newSegment = new KirbyFlameSegment(position, direction, randomSpeed, !isFacingRight);
                flameSegments.Add(newSegment);
            }
        }

        private void SpawnFlameSegment()
        {
            Random random = new Random();
            float angleOffset = isFacingRight ? 0f : (float)Math.PI; // Adjust angle based on direction

            float totalAngleRange = Constants.KirbyFire.MAX_ANGLE - Constants.KirbyFire.MIN_ANGLE;

            // Calculate the angle offset based on the number of segments
            float angle = Constants.KirbyFire.MIN_ANGLE + (float)random.NextDouble() * totalAngleRange;

            // Generate a random speed and delay for each segment
            float randomSpeed = (float)(random.NextDouble() * (Constants.KirbyFire.MAX_SPEED - Constants.KirbyFire.MIN_SPEED) + Constants.KirbyFire.MIN_SPEED);

            // Rotate the direction vector by the calculated angle, adjusting for facing direction
            Vector2 velocity = randomSpeed * Vector2.Transform(new Vector2(1, 0), Matrix.CreateRotationZ(angle + angleOffset));

            velocity += player.GetKirbyVelocity();
            
            KirbyFlameSegment newSegment = new KirbyFlameSegment(position, velocity, randomSpeed, !isFacingRight);
            flameSegments.Add(newSegment);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw all flame segments
            for (int i = 0; i < flameSegments.Count / 2; i++)
            {
                flameSegments[i].Draw(spriteBatch);
            }
            for (int i = flameSegments.Count - 1; i >= flameSegments.Count / 2; i--)
            {
                flameSegments[i].Draw(spriteBatch);
            }
            //spriteBatch.DrawString(LevelLoader.Instance.Font, flameSegments.Count + ", " + IsDone(), position, Color.Black);
        }


        // THESE METHODS NEVER CALLED, ObjectManager just needs all IProjectiles to be
        // ICollidables. This class is a non-colliding projectile, so it's basically
        // just an ICollidable that always says no when asked if its collision is active.
        public bool CollisionActive { get; private set; } = false;
        public Rectangle GetHitBox()
        {
            return Rectangle.Empty;
        }
        public Vector2 GetPosition()
        {
            return Vector2.Zero;
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.PlayerAttack;
        }

    }
}
