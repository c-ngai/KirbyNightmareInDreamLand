using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MasterGame
{
    public class EnemyBeam
    {
        private int totalSegments = 16;
        private int segmentsFired = 0;
        private int frameCounter = 0;
        private List<EnemyBeamSegment> beamSegments;
        private Vector2 startPosition;
        private Vector2 pivotPosition; // WaddleDoo's eye position
        private float rotationStep = MathHelper.PiOver4 / 4; // 22.5 degrees in radians

        public EnemyBeam(Vector2 startPosition, Vector2 pivotPosition)
        {
            this.startPosition = startPosition;
            this.pivotPosition = pivotPosition;
            beamSegments = new List<EnemyBeamSegment>();
        }

        public void Update()
        {
            // Fire a new segment every 2 frames, up to 16 segments
            if (segmentsFired < totalSegments && frameCounter % 2 == 0)
            {
                float rotation = (segmentsFired / 2) * rotationStep; // Change direction every other segment
                Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 8; // Move 8 units per frame

                beamSegments.Add(new EnemyBeamSegment(startPosition, velocity, pivotPosition));
                segmentsFired++;
            }

            frameCounter++;

            // Update all existing beam segments
            for (int i = 0; i < beamSegments.Count; i++)
            {
                beamSegments[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var segment in beamSegments)
            {
                segment.Draw(spriteBatch);
            }
        }
    }
}
