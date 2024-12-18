﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using KirbyNightmareInDreamLand.Entities.Enemies.EnemyState.WaddleDooState;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyBeamSegment : IProjectile, ICollidable, IExplodable
    {
        public IPlayer player { get => null; } // this projectile never originates from a player
        private Vector2 position; // acts as pivot point
        private Vector2 velocity;
        private int frameCount = 0;
        private Sprite projectileSprite;
        public bool IsActive = true;

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
        public CollisionType GetCollisionType()
        {
            return CollisionType.EnemyAttack;
        }
        public EnemyBeamSegment(Vector2 startPosition, Vector2 beamVelocity)
        {
            Position = startPosition;
            Velocity = beamVelocity;
            projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_waddledoo_beam");
            ObjectManager.Instance.AddProjectile(this);
        }


        public void Update()
        {
            if (IsActive)
            {
                Position += Velocity;

                projectileSprite.Update();

                // Increment frame count and check if the segment should disappear
                frameCount++;
                if (frameCount >= Constants.WaddleDooBeam.MAX_FRAMES)
                {
                    IsActive = false; // Mark the segment as inactive
                    CollisionActive = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
            else
            {
                ObjectManager.Instance.RemoveDynamicObject(this); // Deregister if dead
            }
        }

        public bool IsDone()
        {
            return !IsActive;
        }

        public bool CollisionActive { get; set; } = true;

        public virtual Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.BEAM_WIDTH / 2;
            float y = pos.Y - Constants.HitBoxes.BEAM_HEIGHT + Constants.HitBoxes.BEAM_HEIGHT_OFFSET;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint;
        }
        public virtual Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.BEAM_WIDTH, Constants.HitBoxes.BEAM_HEIGHT);
        }
        public Vector2 GetPosition()
        {
            return position;
        }
        public void EndAttack()
        {
            IsActive = false;
            CollisionActive = false;
        }
    }
}
