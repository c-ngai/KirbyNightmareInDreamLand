using KirbyNightmareInDreamLand.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyBeam : IProjectile
    {
        private Vector2 position;
        private Vector2 velocity;
        private int segmentsFired;
        private int frameCounter;
        private List<KirbyBeamSegment> beamSegments;
        private bool isFacingRight;
        private const int totalSegments = Constants.KirbyBeam.TOTAL_SEGMENTS;
        private const float rotationStep = Constants.KirbyBeam.ROTATION_STEP;
        private const float initialRotationRight = Constants.KirbyBeam.INIT_POSITION_RIGHT;
        private const float initialRotationLeft = Constants.KirbyBeam.INIT_POSITION_LEFT;
        private const int UnitsPerFrame = Constants.KirbyBeam.UNITS_PER_FRAME;
        private const int fourthFrameInPattern = Constants.KirbyBeam.FRAME_FOUR;
        private const int fifthFrameInPattern = Constants.KirbyBeam.FRAME_FIVE;

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

        public KirbyBeam(Vector2 kirbyPosition, bool isFacingRight)
        {
            // Calculate the position based on Kirby's position and the offset
            this.isFacingRight = isFacingRight;
            Vector2 offset = isFacingRight ? Constants.Kirby.BEAM_ATTACK_OFFSET_RIGHT : Constants.Kirby.BEAM_ATTACK_OFFSET_LEFT;
            this.position = kirbyPosition + offset;

            beamSegments = new List<KirbyBeamSegment>();
            frameCounter = 0;
            segmentsFired = 0;
            SoundManager.Play("kirbybeamattack");

            ObjectManager.Instance.AddProjectile(this);
        }

        private float GetRotation()
        {
            float rotation = isFacingRight ? initialRotationRight : initialRotationLeft;
            rotation += segmentsFired * (isFacingRight ? rotationStep : -rotationStep);
            return rotation;
        }

        public void Update()
        {
            if (segmentsFired < totalSegments && (frameCounter % fourthFrameInPattern != 0 || frameCounter % fifthFrameInPattern != 0))
            {
                Vector2 velocity = new Vector2((float)Math.Cos(GetRotation()), (float)Math.Sin(GetRotation())) * UnitsPerFrame;
                beamSegments.Add(new KirbyBeamSegment(position, velocity, !isFacingRight));
                segmentsFired++;
            }

            frameCounter++;
            foreach (var segment in beamSegments)
            {
                segment.Update();
            }
        }
        public void EndAttack()
        {
            foreach (var segment in beamSegments)
            {
                if(segment.IsDone())
                {
                    //segment.EndAttack(); //destroys the hit box if it is done
                }
            }
        }
        public bool IsDone()
        {
            foreach (var segment in beamSegments)
            {
                if(!segment.IsDone())
                {
                    return false;
                }
            }
            return true;
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