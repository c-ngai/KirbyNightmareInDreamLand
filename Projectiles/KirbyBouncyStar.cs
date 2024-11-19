using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using KirbyNightmareInDreamLand.Audio;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyBouncingStar : IProjectile, ICollidable
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        public bool CollisionActive { get; private set;} = true;

        public Vector2 Position
        {
            get => position;            // Return position of star
            set => position = value;    // Set the position of the star to the given value
        }

        public Vector2 Velocity
        {
            get => velocity;            // Return the current velocity of the star
            set => velocity = value;    // Set the velocity of the star to the given value
        }

        public KirbyBouncingStar(Vector2 kirbyPosition, bool isFacingRight)
        {
            Position = kirbyPosition + (isFacingRight ? Constants.Kirby.BOUNCING_STAR_OFFSET_RIGHT: Constants.Kirby.BOUNCING_STAR_OFFSET_LEFT);

            // Set the initial velocity based on the direction Kirby is facing
            Velocity = isFacingRight
                ? Constants.Star.BOUNCING_STAR_VEL_RIGHT
                : Constants.Star.BOUNCING_STAR_VEL_LEFT;

            // Assign the appropriate sprite based on the direction
            projectileSprite = isFacingRight
                ? SpriteFactory.Instance.CreateSprite("projectile_kirby_star_right")
                : SpriteFactory.Instance.CreateSprite("projectile_kirby_star_left");
            
            ObjectManager.Instance.RegisterDynamicObject(this);

            //SoundManager.Play("spit");
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.PlayerAttack;
        }

        public void Update()
        {
            Position += Velocity;
            projectileSprite.Update();
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
        public void Draw(SpriteBatch spriteBatch)
        {
            projectileSprite.Draw(Position, spriteBatch);
        }
        public void EndAttack()
        {
            CollisionActive = false;
        }
        public bool IsDone()
        {
            if(!Camera.InAnyActiveCamera(position))
            {
                EndAttack();
                return true;
            }
            return false;
        }
    
    }
}
