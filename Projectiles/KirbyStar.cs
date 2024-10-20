using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyStar : IProjectile, ICollidable
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

        public KirbyStar(Vector2 kirbyPosition, bool isFacingRight)
        {
            Position = kirbyPosition + (isFacingRight ? Constants.Kirby.STAR_ATTACK_OFFSET_RIGHT: Constants.Kirby.STAR_ATTACK_OFFSET_LEFT);

            // Set the initial velocity based on the direction Kirby is facing
            Velocity = isFacingRight
                ? new Vector2(Constants.Star.SPEED, 0)
                : new Vector2(-Constants.Star.SPEED, 0);

            // Assign the appropriate sprite based on the direction
            projectileSprite = isFacingRight
                ? SpriteFactory.Instance.CreateSprite("projectile_kirby_star_right")
                : SpriteFactory.Instance.CreateSprite("projectile_kirby_star_left");
            
            CollisionDetection.Instance.RegisterDynamicObject(this);
        }
        public String GetCollisionType()
        {
            return "PlayerAttack";
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
            if(!Game1.Instance.Camera.bounds.Contains(position))
            {
                return true;
            }
            return false;
        }
    
    }
}
