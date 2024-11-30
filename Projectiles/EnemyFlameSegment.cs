using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using KirbyNightmareInDreamLand.Sprites;
using KirbyNightmareInDreamLand.Actions;
using KirbyNightmareInDreamLand.Entities.Players;

namespace KirbyNightmareInDreamLand.Projectiles
{
    public class EnemyFlameSegment : IProjectile, ICollidable
    {
        public IPlayer player { get => null; } // this projectile never originates from a player
        private Sprite projectileSprite;
        private Vector2 position;
        private Vector2 velocity;
        private float speed;
        private float delay; // Delay before this segment becomes active
        private bool isActive; 
        private static Random random = new Random(); // Random instance for sprite selection
        private int frameCount = -1;

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
        public EnemyFlameSegment(Vector2 startPosition, Vector2 flameDirection, float speed)
        {
            Position = startPosition;
            //this.speed = speed;
            isActive = true;
            this.velocity = flameDirection;

            ObjectManager.Instance.AddProjectile(this);
            projectileSprite = SpriteFactory.Instance.CreateSprite("projectile_hothead_fire");
        }
        public CollisionType GetCollisionType()
        {
            return CollisionType.EnemyAttack;
        }

        public void Update()
        {
            // Only update position if the flame segment is active
            if (isActive)
            {
                Position += Velocity; 

                frameCount++;
                projectileSprite?.Update();
                // Mark the segment as inactive after a certain number of frames
                if (IsDone())
                {
                    EndAttack();
                }
            }
            GetHitBox();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive && projectileSprite != null )
            {
                projectileSprite.Draw(Position, spriteBatch);
            }
        }
        public void EndAttack()
        {
            isActive = false;
            projectileSprite = null; // Set sprite to null to avoid further drawing
            CollisionActive = false;
        }
        public bool IsDone()
        {
            return frameCount >= Constants.EnemyFire.MAX_FRAMES;
        }

        public bool CollisionActive { get; set; } = true;

        public virtual Vector2 CalculateRectanglePoint(Vector2 pos)
        {
            float x = pos.X - Constants.HitBoxes.FLAME_WIDTH /2;
            float y = pos.Y - Constants.HitBoxes.FLAME_HEIGHT + 6;
            Vector2 rectPoint = new Vector2(x, y);
            return rectPoint;
        }
        public virtual Rectangle GetHitBox()
        {
            Vector2 rectPoint = CalculateRectanglePoint(position);
            return new Rectangle((int)rectPoint.X, (int)rectPoint.Y, Constants.HitBoxes.FLAME_WIDTH, Constants.HitBoxes.FLAME_HEIGHT);
        }
        public Vector2 GetPosition()
        {
            return Position;
        }

    }
}
