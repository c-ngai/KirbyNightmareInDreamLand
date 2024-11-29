using KirbyNightmareInDreamLand.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyFlamethrower : IProjectile, ICollidable
    {
        private List<EnemyFlameSegment> flameSegments;
        private Vector2 startPosition;
        private float elapsedTime;
        private Vector2 flameDirection; 
        private bool isLeft;
        private int frameCounter;
        private int wait = 0;
        private bool IsActive;
        private SoundInstance sound;

        public Vector2 Position
        {
            get => startPosition;
            set => startPosition = value;
        }

        public Vector2 Velocity
        {
            get => Vector2.Zero;
        }

        public EnemyFlamethrower(Vector2 startPosition, bool isLeft)
        {
            this.startPosition = startPosition;
            this.flameDirection = isLeft ? Constants.Hothead.FLAMETHROWER_LEFT : Constants.Hothead.FLAMETHROWER_RIGHT;
            this.isLeft = isLeft;
            flameSegments = new List<EnemyFlameSegment>();
            elapsedTime = 0f;
            frameCounter = 0;
            IsActive = true;

            ObjectManager.Instance.AddProjectile(this);

            sound = SoundManager.CreateInstance("hotheadflamethrowerattack");
            sound.Play();
        }

        public void EndAttack()
        {
            sound.Stop();
            IsActive = false;
        }
        public bool IsDone()
        {
            if (frameCounter <= wait)
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
            // Check if it's time to spawn new flame segments
            if (IsActive && frameCounter % 2 == 0 && frameCounter >= wait)
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
