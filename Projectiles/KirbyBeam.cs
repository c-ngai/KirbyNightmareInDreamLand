using KirbyNightmareInDreamLand.Audio;
using KirbyNightmareInDreamLand.Entities.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class KirbyBeam : IProjectile, ICollidable
    {
        private IPlayer player;
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

        public KirbyBeam(IPlayer player, bool isFacingRight)
        {
            this.player = player;

            // Calculate the position based on Kirby's position and the offset
            this.isFacingRight = isFacingRight;
            Vector2 offset = isFacingRight ? Constants.Kirby.BEAM_ATTACK_OFFSET_RIGHT : Constants.Kirby.BEAM_ATTACK_OFFSET_LEFT;
            this.position = player.GetKirbyPosition() + offset;

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
            Vector2 offset = isFacingRight ? Constants.Kirby.BEAM_ATTACK_OFFSET_RIGHT : Constants.Kirby.BEAM_ATTACK_OFFSET_LEFT;
            this.position = player.GetKirbyPosition() + offset;

            if (segmentsFired < totalSegments && frameCounter % 5 < 3 )//(frameCounter % fourthFrameInPattern != 0 || frameCounter % fifthFrameInPattern != 0))
            {
                Vector2 velocity = new Vector2((float)Math.Cos(GetRotation()), (float)Math.Sin(GetRotation())) * UnitsPerFrame;
                velocity += player.GetKirbyVelocity();
                bool odd = frameCounter % 2 != 0;
                beamSegments.Add(new KirbyBeamSegment(position, velocity, odd));
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