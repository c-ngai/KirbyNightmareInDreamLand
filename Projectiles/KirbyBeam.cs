using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MasterGame
{
    public class KirbyBeam
    {
        private int totalSegments = 18;
        private int segmentsFired = 0;
        private int frameCounter = 1;
        private List<KirbyBeamSegment> beamSegments;
        private Vector2 startPosition;
        private float rotation;
        private float rotationStep = MathHelper.Pi / 32 ; // 4 degrees in radians
        private float initialRotationRight = -MathHelper.PiOver4;
        private float initialRotationLeft = MathHelper.PiOver4 * 5;

        private bool isFacingRight;


        public KirbyBeam(Vector2 startPosition, bool isFacingRight)
        {
            this.startPosition = startPosition;
            beamSegments = new List<KirbyBeamSegment>();
            this.isFacingRight = isFacingRight;


        }

        private float getRotation()
        {
            if(isFacingRight)
            {
                if (segmentsFired == 0)
                {
                    rotation = initialRotationRight;
                }
                else
                {
                    rotation = initialRotationRight + (segmentsFired * rotationStep);
                }
            }
            else
            {
                if (segmentsFired == 0)
                {
                    rotation = initialRotationLeft;
                }
                else
                {
                    rotation = initialRotationLeft + (segmentsFired * -rotationStep);
                }
            }

            return rotation;
        }


        public void Update()
        {
            // Fire a new segment every 2 frames, up to 16 segments
            if (segmentsFired < totalSegments && (frameCounter % 5 != 0 || frameCounter % 6 != 0) )
            { 
                Vector2 velocity = new Vector2((float)Math.Cos(getRotation()), (float)Math.Sin(getRotation())) * 8; // Move 8 units per frame

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
            foreach (var segment in beamSegments)
            {
                segment.Draw(spriteBatch);
            }
        }
    }
}