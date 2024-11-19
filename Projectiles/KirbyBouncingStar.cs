using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Particles;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyBouncingStar : IProjectile, ICollidable
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        public bool CollisionActive { get; private set;} = true;
        public bool IsFacingRight;
        private float ceiling = Constants.Kirby.CEILING;
        public bool PowerType; 
        private KirbyType powerUp;
        private double timer ;

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

        public KirbyBouncingStar(Vector2 kirbyPosition, bool isFacingRight, KirbyType power)
        {
            powerUp = power;
            Position = kirbyPosition + (isFacingRight ? Constants.Kirby.BOUNCING_STAR_OFFSET_RIGHT: Constants.Kirby.BOUNCING_STAR_OFFSET_LEFT);

            Velocity = Constants.Star.BOUNCING_STAR_VEL_LEFT;

            // Assign the appropriate sprite based on the direction
            projectileSprite = isFacingRight
                ? SpriteFactory.Instance.CreateSprite("projectile_kirby_star_right")
                : SpriteFactory.Instance.CreateSprite("projectile_kirby_star_left");

            ObjectManager.Instance.AddProjectile(this);
            ObjectManager.Instance.RegisterDynamicObject(this);

            //SoundManager.Play("spit");
            IsFacingRight = isFacingRight;
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.BouncingStar;
        }

        public void Adjust(){
            if (position.Y < ceiling)
            {
                velocity.Y = 0;
                position.Y = ceiling;
            }
        }
        public void Update()
        {
            velocity.Y += Constants.Physics.GRAVITY *  Constants.Physics.DT;
            Position += Velocity;
            projectileSprite.Update();
            Adjust();
        }
        private Vector2 CalculateRectanglePoint(Vector2 pos)
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
            timer+= Game1.GameTime.ElapsedGameTime.TotalSeconds;
            if(timer > Constants.Star.BOUNCING_TIMER)
            {
                CollisionActive = false;
                SoundManager.Play("starexplode");
                new StarExplode(position);
                return true;
            }
            return false;
        }

        public void WallLeftBounce()
        {
            IsFacingRight = !IsFacingRight;
            velocity = Constants.Star.BOUNCING_STAR_VEL_RIGHT;
        }
        public void WallRightBounce()
        {
            IsFacingRight = !IsFacingRight;
            velocity = Constants.Star.BOUNCING_STAR_VEL_LEFT;
        }

        public void FloorBounce()
        {
            velocity.Y = Constants.Star.BOUNCING_STAR_VEL_RIGHT.Y ;
        }

        public KirbyType PowerUp()
        {
            return powerUp;
        }
    }
}
