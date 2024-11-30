using KirbyNightmareInDreamLand;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using KirbyNightmareInDreamLand.Entities.Players;
using System.Reflection.Metadata.Ecma335;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyBeam : IProjectile, ICollidable
    {
        public IPlayer player { get => null; } // this projectile never originates from a player
        private int segmentsFired = 0;
        private int frameCounter = 0;
        private List<EnemyBeamSegment> beamSegments;
        private Vector2 startPosition;
        private bool isFacingRight; // Track if WaddleDoo is facing left

        public Vector2 Position
        {
            get => startPosition;
            set => startPosition = value;
        }

        public Vector2 Velocity
        {
            get => Vector2.Zero;
        }

        public EnemyBeam(Vector2 startPosition, bool isFacingRight)
        {
            this.startPosition = startPosition;
            this.isFacingRight = isFacingRight;
            beamSegments = new List<EnemyBeamSegment>();

            ObjectManager.Instance.AddProjectile(this);
        }

        public void Update()
        {
            // Fire a new segment every 2 frames, up to 16 segments
            if (segmentsFired < Constants.WaddleDooBeam.TOTAL_SEGMENTS && frameCounter % 2 == 0)
            {
                float rotation;

                if (segmentsFired == 0)
                {
                    // Fire the first segment straight up
                    rotation = Constants.WaddleDooBeam.INITIAL_ROTATION;
                }
                else
                {
                    // After the first, rotate every other segment
                    rotation = Constants.WaddleDooBeam.INITIAL_ROTATION + (segmentsFired / 2) * Constants.WaddleDooBeam.ROTATION_STEP;
                }

                Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * Constants.WaddleDooBeam.UNITS_PER_FRAME; // Move 8 units per frame

                // If facing left, mirror the velocity
                if (!isFacingRight)
                {
                    velocity.X = -velocity.X;
                }

                beamSegments.Add(new EnemyBeamSegment(startPosition, velocity));
                segmentsFired++;
            }

            frameCounter++;

            //Update all existing beam segments
            //for (int i = beamSegments.Count - 1; i >= 0; i--) // Loop backwards to avoid index issues
            //{
            //    beamSegments[i].Update();
            //    Remove the segment if it's no longer active

            //    if (!beamSegments[i].IsActive)
            //    {
            //        beamSegments.RemoveAt(i);
            //    }
            //}
        }

        public void EndAttack()
        {

        }
        // Done if all segments are done (and not on the firs)
        public bool IsDone()
        {
            if (frameCounter <= 1)
            {
                return false;
            }
            foreach (var segment in beamSegments)
            {
                if (!segment.IsDone())
                {
                    return false;
                }
            }
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //foreach (var segment in beamSegments)
            //{
            //    segment.Draw(spriteBatch);
            //}
        }

        public bool IsDone2()
        {
            // Check if there are any active segments
            return beamSegments.Count == 0; // Returns true if there are active segments
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
