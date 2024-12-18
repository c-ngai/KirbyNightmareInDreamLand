﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Actions;
using System.Diagnostics;
using KirbyNightmareInDreamLand.Particles;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyStar : IProjectile, ICollidable, IExplodable
    {
        public IPlayer player { get; private set; }

        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;

        private int frameCounter;

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

        public KirbyStar(IPlayer _player, Vector2 kirbyPosition, bool isFacingRight)
        {
            player = _player;

            Position = kirbyPosition + (isFacingRight ? Constants.Kirby.STAR_ATTACK_OFFSET_RIGHT: Constants.Kirby.STAR_ATTACK_OFFSET_LEFT);

            // Set the initial velocity based on the direction Kirby is facing
            Velocity = isFacingRight
                ? new Vector2(Constants.Star.SPEED, 0)
                : new Vector2(-Constants.Star.SPEED, 0);

            // Assign the appropriate sprite based on the direction
            projectileSprite = isFacingRight
                ? SpriteFactory.Instance.CreateSprite("projectile_kirby_star_right")
                : SpriteFactory.Instance.CreateSprite("projectile_kirby_star_left");

            frameCounter = 0;

            ObjectManager.Instance.AddProjectile(this);

            SoundManager.Play("spit");
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.KirbyStar;
        }

        public void Update()
        {
            Position += Velocity;
            projectileSprite.Update();
            if (position.X < -16 || position.X > Game1.Instance.Level.CurrentRoom.Width + 16)
            {
                CollisionActive = false;
            }
            if (frameCounter % Constants.Particle.STARDUST_MAX_FRAMES == 0)
            {
                new Stardust(position - velocity * 2);
            }
            frameCounter++;
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
            if (CollisionActive)
            {
                CollisionActive = false;
                SoundManager.Play("starexplode");
                new StarExplode(position);
            }
            
        }
        public bool IsDone()
        {
            //if(!Camera.InAnyActiveCamera(position))
            //{
            //    EndAttack();
            //    return true;
            //}
            //return false;
            return !CollisionActive;
        }
    
    }
}
