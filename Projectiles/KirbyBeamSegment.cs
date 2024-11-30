using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System.Collections.Generic;
using System;
using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyBeamSegment : IProjectile, ICollidable
    {
        public IPlayer player { get; private set; }

        private Vector2 position;
        private Vector2 velocity;
        private int frameCount = 0;
        public bool IsActive {get; private set;}= true;
        private int maxFrames = 6; // Segment disappears after 6 frames
        private ISprite sprite;
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

        public KirbyBeamSegment(Vector2 startPosition, Vector2 beamVelocity, bool odd, IPlayer _player)
        {
            player = _player;

            Position = startPosition;
            Velocity = beamVelocity;
            sprite = odd ?
                SpriteFactory.Instance.CreateSprite("projectile_kirby_beam1")
              : SpriteFactory.Instance.CreateSprite("projectile_kirby_beam2");
            ObjectManager.Instance.RegisterDynamicObject(this);
        }

        public void Update()
        {
            if (IsActive)
            {
                // Update position based on velocity
                Position += Velocity;
            } 

            sprite.Update();

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
            if (IsActive)
            {
                sprite.Draw(Position, spriteBatch);
            }
        }
    }
}
