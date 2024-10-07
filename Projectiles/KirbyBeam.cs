using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyBeam
    {
        private int segmentsFired;
        private int frameCounter;
        private List<KirbyBeamSegment> beamSegments;
        private Vector2 startPosition;
        private float rotation;
        private const int totalSegments = Constants.KirbyBeam.TOTAL_SEGMENTS;
        private const float rotationStep = Constants.KirbyBeam.ROTATION_STEP;
        private const float initialRotationRight = Constants.KirbyBeam.INIT_POSITION_RIGHT;
        private const float initialRotationLeft = Constants.KirbyBeam.INIT_POSITION_LEFT;
        private const int UnitsPerFrame = Constants.KirbyBeam.UNITS_PER_FRAME;
        private const int fourthFrameInPattern = Constants.KirbyBeam.FRAME_FOUR;
        private const int fifthFrameInPattern = Constants.KirbyBeam.FRAME_FIVE;

        public KirbyBeam(Vector2 startPosition, bool isFacingRight)
        {
            this.startPosition = startPosition;
            beamSegments = new List<KirbyBeamSegment>();
            frameCounter = 0;
            segmentsFired = 0;
        }

        private float GetRotation()
        {
            if (isFacingRight)
            {
                rotation = segmentsFired == 0 ? initialRotationRight : initialRotationRight + (segmentsFired * rotationStep);
            }
            else
            {
                rotation = segmentsFired == 0 ? initialRotationLeft : initialRotationLeft + (segmentsFired * -rotationStep);
            }

            return rotation;
        }

        public void Update()
        {
            if (segmentsFired < totalSegments && (frameCounter % fourthFrameInPattern != 0 || frameCounter % fifthFrameInPattern != 0))
            {
                Vector2 velocity = new Vector2((float)Math.Cos(GetRotation()), (float)Math.Sin(GetRotation())) * UnitsPerFrame;

                beamSegments.Add(new KirbyBeamSegment(startPosition, velocity));
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
            foreach (KirbyBeamSegment segment in beamSegments)
            {
                segment.Draw(spriteBatch);
            }
        }
    }
}