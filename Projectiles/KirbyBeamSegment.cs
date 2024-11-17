using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System.Collections.Generic;
using System;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Actions;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyBeamSegment : IProjectile, ICollidable
    {
        private Vector2 position;
        private Vector2 velocity;
        private int frameCount = 0;
        public bool IsActive {get; private set;}= true;
        private int maxFrames = 6; // Segment disappears after 6 frames
        private ISprite sprite1;
        private ISprite sprite2;
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

        public KirbyBeamSegment(Vector2 startPosition, Vector2 beamVelocity, bool isLeft)
        {
            Position = startPosition;
            Velocity = beamVelocity;
            sprite1 = SpriteFactory.Instance.CreateSprite("projectile_kirby_beam1");
            sprite2 = SpriteFactory.Instance.CreateSprite("projectile_kirby_beam2");
            ObjectManager.Instance.RegisterDynamicObject(this);
        }

        public void Update()
        {
            if (IsActive)
            {
                // Update position based on velocity
                Position += Velocity;
            } 
            

            // Update animation Chandled internally by sprite)
            if (frameCount % 2 == 0)
            {
                sprite2.Update();

            }
            else
            {
                sprite1.Update();
            }

            // Increment frame count and check if the segment should disappear
            frameCount++;
            if (frameCount >= maxFrames)
            {
                CollisionActive = false;
                IsActive = false; // Mark the segment as inactive
                EndAttack();

            }
            GetHitBox();
        }
        public Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            return pos + Constants.HitBoxes.BEAM_OFFSET; 
        }
        public Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(Position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.BEAM_SIZE, Constants.HitBoxes.BEAM_SIZE);
        }
        public Vector2 GetPosition()
        {
            return Position;
        }
        public void EndAttack()
        {
            CollisionActive = false;
        }
        public bool IsDone()
        {
            return !IsActive;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (frameCount % 2 == 0 && IsActive)
            {
                sprite2.Draw(Position, spriteBatch);

            }
            else if (IsActive)
            {
                sprite1.Draw(Position, spriteBatch);
            }
        }
    }
}
