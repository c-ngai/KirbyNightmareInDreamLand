using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.StateMachines;
using KirbyNightmareInDreamLand.Particles;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyBouncingStar : IProjectile, ICollidable
    {
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        public bool CollisionActive { get; private set;} = true;
        public bool IsActive;
        public bool IsLeft;
        private float ceiling = Constants.Kirby.CEILING;
        public bool PowerType; 
        private KirbyType powerUp;
        private double timer ;

        public bool isBeingInhaled;
        private IPlayer playerInhalingMe;

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

        public KirbyBouncingStar(Vector2 kirbyPosition, bool IsLeft, KirbyType power)
        {
            IsActive = true;
            isBeingInhaled = false;
            
            powerUp = power;
            Position = kirbyPosition + (IsLeft ? Constants.Kirby.BOUNCING_STAR_OFFSET_RIGHT : Constants.Kirby.BOUNCING_STAR_OFFSET_LEFT);

            Velocity = IsLeft ? Constants.Star.BOUNCING_STAR_VEL_RIGHT : Constants.Star.BOUNCING_STAR_VEL_LEFT;

            // Assign the appropriate sprite based on the direction
            projectileSprite = IsLeft
                ? SpriteFactory.Instance.CreateSprite("projectile_kirby_bouncingstar_right")
                : SpriteFactory.Instance.CreateSprite("projectile_kirby_bouncingstar_left");

            ObjectManager.Instance.AddProjectile(this);
            ObjectManager.Instance.RegisterDynamicObject(this);

            //SoundManager.Play("spit");
            this.IsLeft = IsLeft;
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
            if (isBeingInhaled)
            {
                AccellerateTowards(playerInhalingMe.GetKirbyPosition());
            }
            else
            {
                velocity.Y += Constants.Physics.GRAVITY;
                
                timer += Game1.GameTime.ElapsedGameTime.TotalSeconds;
                if (timer > Constants.Star.BOUNCING_TIMER)
                {
                    CollisionActive = false;
                    IsActive = false;
                    SoundManager.Play("starexplode");
                    new StarExplode(position);
                }
            }
            Position += Velocity;
            // If the star is below the death barrier, despawn it silently
            if (position.Y > Game1.Instance.Level.CurrentRoom.DeathBarrier)
            {
                CollisionActive = false;
                IsActive = false;
            }

            projectileSprite.Update();
            Adjust();
        }
        public void AccellerateTowards(Vector2 _position)
        {
            float magnitude = velocity.Length();
            velocity = _position - position;
            velocity.Normalize();
            velocity *= magnitude + 0.2f;
            //velocity.X += (_position.X - position.X) / 200;
            //velocity.Y += (_position.Y - position.Y) / 200;
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
        public void GetInhaled(Rectangle intersection, IPlayer player)
        {
            if (!isBeingInhaled)
            {
                velocity = Vector2.Zero;
                isBeingInhaled = true;
                playerInhalingMe = player;
            }
        }
        public void GetSwallowed(Rectangle intersection)
        {
            EndAttack();
        }
        public void EndAttack()
        {
            CollisionActive = false;
            IsActive = false;
        }
        public bool IsDone()
        {
            return !IsActive;
        }

        public void WallLeftBounce()
        {
            IsLeft = !IsLeft;
            //velocity = Constants.Star.BOUNCING_STAR_VEL_RIGHT;
            velocity.X *= -1;
            SoundManager.Play("starbounce");
        }
        public void WallRightBounce()
        {
            IsLeft = !IsLeft;
            //velocity = Constants.Star.BOUNCING_STAR_VEL_LEFT;
            velocity.X *= -1;
            SoundManager.Play("starbounce");
        }

        public void FloorBounce()
        {
            velocity.Y = Constants.Star.BOUNCING_STAR_VEL_RIGHT.Y;
            SoundManager.Play("starbounce");
        }
        public void CeilingBounce()
        {
            velocity.Y *= -1;
            SoundManager.Play("starbounce");
        }

        public KirbyType PowerUp()
        {
            return powerUp;
        }
    }
}
