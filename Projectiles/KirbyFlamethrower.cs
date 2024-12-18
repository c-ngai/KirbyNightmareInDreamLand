﻿using KirbyNightmareInDreamLand.Audio;
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

        public IPlayer player { get; private set; }
        private Vector2 position;
        private Vector2 velocity;
        private List<KirbyFlameSegment> flameSegments;
        private int frameCounter;
        private int wait = 16; // constant later
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
            Vector2 offset = isFacingRight ? Constants.Kirby.FLAME_ATTACK_OFFSET_RIGHT : Constants.Kirby.FLAME_ATTACK_OFFSET_LEFT;
            this.position = player.GetKirbyPosition() + offset;

            // Check if it's time to spawn new flame segments
            if (IsActive && frameCounter % 1 == 0 && frameCounter >= wait)
            {
                SpawnFlameSegment();
            }
            frameCounter++;

            // Update all flame segments
            foreach (var segment in flameSegments)
            {
                //segment.Update();
            }
            //This line below doesn't work because the flame segments start out as inactive and then wait to become active.
            flameSegments.RemoveAll(obj => obj.IsDone());
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
            
            KirbyFlameSegment newSegment = new KirbyFlameSegment(position, velocity, randomSpeed, !isFacingRight, player);
            flameSegments.Add(newSegment);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //not needed
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
