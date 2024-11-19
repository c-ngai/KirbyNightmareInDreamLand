using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System.Net.NetworkInformation;
using System;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Actions;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyPuff : IProjectile, ICollidable, IExplodable
    {
        private Sprite projectileSprite;
        private Vector2 position;
        public bool CollisionActive { get; private set;} = true;
        private Vector2 velocity;
        private bool isFacingRight;
        private int frameCount = 0;
        public bool isActive = true;
        public CollisionType GetCollisionType()
        {
            return CollisionType.PlayerAttack;
        }
        public Vector2 Position
        {
            get => position;            // Return position of puff
            set => position = value;    // Set the position of the puff to the given value
        }

        public Vector2 Velocity
        {
            get => velocity;            // Return the current velocity of the puff
            set => velocity = value;    // Set the velocity of the puff to the given value
        }
        public KirbyPuff(Vector2 kirbyPosition, bool isFacingRight)
        {

            this.isFacingRight = isFacingRight;
            Vector2 offset = isFacingRight ? Constants.Kirby.PUFF_ATTACK_OFFSET : -Constants.Kirby.PUFF_ATTACK_OFFSET;


            Position = kirbyPosition + offset;

            // Set initial velocity based on direction
            Velocity = isFacingRight
                ? new Vector2(Constants.Puff.INITIAL_SPEED, 0)
                : new Vector2(-Constants.Puff.INITIAL_SPEED, 0);

            // Assign the appropriate sprite based on direction
            projectileSprite = isFacingRight
                ? SpriteFactory.Instance.CreateSprite("projectile_kirby_airpuff_right")
                : SpriteFactory.Instance.CreateSprite("projectile_kirby_airpuff_left");

            ObjectManager.Instance.AddProjectile(this);
            ObjectManager.Instance.RegisterDynamicObject(this);

            SoundManager.Play("spitair");
        }
         public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            return pos + Constants.HitBoxes.PUFF_OFFSET; 
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(Position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.PUFF_SIZE, Constants.HitBoxes.PUFF_SIZE);
        }
        public Vector2 GetPosition()
        {
            return Position;
        }
        public void EndAttack()
        {
            isActive = false;
            CollisionActive = false;
        }
        public bool IsDone()
        {
            return isActive;
        }
        public void Update()
        {
            if (isActive)
            {
                // Decelerate the puff by reducing its velocity
                if (Velocity.Length() > 0)
                {
                    Vector2 deceleration = Vector2.Normalize(Velocity) * Constants.Puff.DECELERATION_RATE;
                    Velocity -= deceleration;

                    // Make the velocity zero if it becomes negative or close to zero
                    if (Velocity.Length() < Constants.Puff.SMALL_VELOCITY)
                    {
                        Velocity = Vector2.Zero;
                    }
                }

                Position += Velocity;

                // Increment frame count and check if puff should disappear
                frameCount++;
                if (frameCount >= Constants.Puff.MAX_FRAMES || Velocity == Vector2.Zero)
                {
                    isActive = false;
                    projectileSprite = null; // Remove the sprite to avoid memory leaks
                    EndAttack();
                }

                // Update the puff's sprite animation only if it's still active
                if (isActive)
                {
                    projectileSprite?.Update();
                }
            }
            GetHitBox();
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw if the puff is active and the sprite exists
            if (isActive && projectileSprite != null)
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
        }
    }
}
