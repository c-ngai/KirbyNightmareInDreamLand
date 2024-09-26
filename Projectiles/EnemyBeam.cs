using MasterGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

public class EnemyBeam
{
    private int totalSegments = 16;
    private int segmentsFired = 0;
    private int frameCounter = 0;
    private List<EnemyBeamSegment> beamSegments;
    private Vector2 startPosition;
    private float initialRotation = -MathHelper.PiOver2; // -90 degrees to fire straight up
    private float rotationStep = MathHelper.PiOver4 / 2; // 22.5 degrees in radians

    public EnemyBeam(Vector2 startPosition)
    {
        this.startPosition = startPosition;
        beamSegments = new List<EnemyBeamSegment>();
    }

    public void Update()
    {
        // Fire a new segment every 2 frames, up to 16 segments
        if (segmentsFired < totalSegments && frameCounter % 2 == 0)
        {
            float rotation;

            if (segmentsFired == 0)
            {
                // Fire the first segment straight up
                rotation = initialRotation;
            }
            else
            {
                // After the first, rotate every other segment
                rotation = initialRotation + (segmentsFired / 2) * rotationStep;
            }

            Vector2 velocity = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation)) * 8; // Move 8 units per frame

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
}
