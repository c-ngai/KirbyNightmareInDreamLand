using MasterGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MasterGame.Projectiles
{
    public class EnemyBeam
    {
        private int segmentsFired = 0;
        private int frameCounter = 0;
        private List<EnemyBeamSegment> beamSegments;
        private Vector2 startPosition;
        private bool isFacingRight; // Track if WaddleDoo is facing left

        public EnemyBeam(Vector2 startPosition, bool isFacingRight)
        {
            this.startPosition = startPosition;
            this.isFacingRight = isFacingRight;
            beamSegments = new List<EnemyBeamSegment>();
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

            // Update all existing beam segments
            for (int i = beamSegments.Count - 1; i >= 0; i--) // Loop backwards to avoid index issues
            {
                beamSegments[i].Update();
                // Remove the segment if it's no longer active
                if (!beamSegments[i].IsActive)
                {
                    beamSegments.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var segment in beamSegments)
            {
                segment.Draw(spriteBatch);
            }
        }

        public bool IsBeamActive()
        {
            // Check if there are any active segments
            return beamSegments.Count > 0; // Returns true if there are active segments
        }
    }
}
