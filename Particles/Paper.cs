using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KirbyNightmareInDreamLand.Sprites;
using System;
using System.Diagnostics;
using KirbyNightmareInDreamLand.Levels;

namespace KirbyNightmareInDreamLand.Particles
{
    public class Paper : IParticle
    {
        private Sprite sprite;
        private Vector2 position;
        private Vector2 velocity;
        private float terminalVelocity;
        private bool completed;

        private Level _level;

        public Paper(Vector2 _position)
        {
            _level = Game1.Instance.Level;

            // Set initial position relative to given position (position of suitcase)
            position = _position + Constants.Particle.PAPER_SPAWN_OFFSET;

            // Set initial velocity based on random ranges
            velocity.X = (float)Random.Shared.NextDouble() * (Constants.Particle.PAPER_INITIAL_XVEL_MAX - Constants.Particle.PAPER_INITIAL_XVEL_MIN) + Constants.Particle.PAPER_INITIAL_XVEL_MIN;
            velocity.Y = (float)Random.Shared.NextDouble() * (Constants.Particle.PAPER_INITIAL_YVEL_MAX - Constants.Particle.PAPER_INITIAL_YVEL_MIN) + Constants.Particle.PAPER_INITIAL_YVEL_MIN;

            // Set terminal velocity of this piece of paper based on the range
            terminalVelocity = (float)Random.Shared.NextDouble() * (Constants.Particle.PAPER_TERMINALVEL_MAX - Constants.Particle.PAPER_TERMINALVEL_MIN) + Constants.Particle.PAPER_TERMINALVEL_MIN;

            // Pick randomly between the mirrored versions of the sprite
            sprite = Random.Shared.Next(0, 2) == 0 ? 
                SpriteFactory.Instance.CreateSprite("particle_paper1") :
                SpriteFactory.Instance.CreateSprite("particle_paper2");

            // janky way of starting on a random frame
            int offset = Random.Shared.Next(0, Constants.Particle.PAPER_FRAMES);
            for (int i = 0; i < offset; i++)
            {
                sprite.Update();
            }

            ObjectManager.Instance.AddParticle(this);
            completed = false;
        }

        public void DecelerateX(float deceleration)
        {
            if (velocity.X > 0)
            {
                velocity.X -= deceleration;
            }
            else if (velocity.X < 0)
            {
                velocity.X += deceleration;
            }
            if (velocity.X < deceleration / 2 && velocity.X > -deceleration / 2)
            {
                velocity.X = 0;
            }
        }

        public void Update()
        {
            sprite.Update();

            // Fall
            velocity.Y += Constants.Particle.PAPER_GRAVITY;
            if (velocity.Y > terminalVelocity)
            {
                velocity.Y = terminalVelocity;
            }
            // Slow down horizontally from the initial momentum of the suitcase explosion
            DecelerateX(Constants.Particle.PAPER_X_DECELERATION);

            // Apply velocity and end if below the death barrier
            position += velocity;
            if (position.Y > _level.CurrentRoom.DeathBarrier)
            {
                sprite = null;
                completed = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(position, spriteBatch);
        }

        public bool IsDone()
        {
            return completed;
        }
    }
}
