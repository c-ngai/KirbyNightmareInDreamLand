using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.Actions;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyFlameSegment : IProjectile, ICollidable
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        private static Random random = new Random(); // Random instance for sprite selection
        private int frameCount;
        private bool IsLeft;
        public bool IsActive { get; private set; } // Expose IsActive for external checks
        public bool CollisionActive { get; private set;} = true;
        public CollisionType GetCollisionType()
        {
            return CollisionType.PlayerAttack;
        }
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

        public KirbyFlameSegment(Vector2 startPosition, Vector2 velocity, float currentSpeed, bool isLeft)
        {
            Position = startPosition;
            this.velocity = velocity;
            IsActive = true;
            frameCount = 0;
            IsLeft = isLeft;

            // Randomly select either the first or second set of sprites based on direction
            bool useSecondSprite = random.Next(2) == 0; // 50% chance to use the second sprite

            if (IsLeft)
            {
                // Left-facing direction
                if (useSecondSprite)
                {
                    projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_kirby_fire2_left");
                }
                else
                {
                    projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_kirby_fire1_left");
                }
            }
            else
            {
                // Right-facing direction
                if (useSecondSprite)
                {
                    projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_kirby_fire2_right");
                }
                else
                {
                    projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_kirby_fire1_right");
                }
            }
            ObjectManager.Instance.RegisterDynamicObject(this);
        }

        public void Update()
        {
            ////Reduce delay over time
            //if (delay > 0)
            //{
            //    delay -= Constants.KirbyFire.SECONDS_PER_FRAME; // 60fps. 1/60 = ~0.016 seconds per frame
            //}
            //else
            //{
            //    IsActive = true;
            //}

            // Only update position if the flame segment is active
            if (IsActive)
            {
                Position += Velocity; // Update position based on velocity

                frameCount++;

                // Mark the segment as inactive after a certain number of frames
                if (frameCount >= Constants.KirbyFire.MAX_FRAMES)
                {
                    IsDone();
                    EndAttack();
                }
                else
                {
                    projectileSprite?.Update();
                }
            }
            GetHitBox();
        }
         public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            return pos + (IsLeft ? Constants.HitBoxes.FIRE_OFFSET_LEFT: Constants.HitBoxes.FIRE_OFFSET_RIGHT); 
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(Position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.FIRE_SIZE, Constants.HitBoxes.FIRE_SIZE);
        }
        public Vector2 GetPosition()
        {
            return Position;
        }
        public bool IsDone()
        {
            return !IsActive;
        }
        public void EndAttack()
        {
            IsActive = false;
            projectileSprite = null;
            CollisionActive = false;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive && projectileSprite != null)
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
        }
    }
}
